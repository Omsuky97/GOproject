using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Scaner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit[] targets_monster;
    public Transform nearestTarget;

    public GameObject Fire_Point;
    public GameManager player_Statas;

    public bool target_type = false;
    private float originalAttackDelay; // 원래 공격 딜레이 값 저장

    public float rotationSpeed = 5f; // 회전 속도
    public float Bullet_Speaker_Speed = 0.3f; //= 가속
    public float boostDuration_start = 3f;         // 가속 유지 시간
    public float boostDuration_End = 3f;         // 가속 유지 시간
    public bool Bullet_Speaker_Type;
    public bool Bullet_Speaker_isBoosted;
    public short Bullet_Speaker_Shot_count;
    public short Bullet_Speaker_Shot_Max_count;

    public float timer;
    public float boostDuration_End_Timer;
    bool player_attack = false;
    Vector3 targetPos;

    //파티클
    public GameObject FireEffectPrefab; // 맞았을 때 실행할 파티클 프리팹

    private void Start()
    {
        originalAttackDelay = player_Statas.attack_delay; // 시작 시 원래 값 저장
    }
    private void FixedUpdate()
    {
        targets_monster = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, scanRange, targetLayer);
        if (!target_type) nearestTarget = GetNearest();
        else if (target_type) nearestTarget = GetFarthest();
        if (nearestTarget != null)
        {
            Player_Rotator();

            if (!player_attack)
            {
                timer += Time.deltaTime;
            }

            if (Bullet_Speaker_Type)
            {
                //이거 레벨업 할때마다 6을 줄여서 확률 높여줄 것
                int randomValue = Random.Range(0, 6); // 0~5 사이의 랜덤 값
                if (timer > player_Statas.attack_delay && randomValue == 0)
                {
                    Bullet_Speaker();
                }
                else if (timer > player_Statas.attack_delay)
                {
                    timer = 0f;
                    Fire();
                }
            }
            else
            {
                if (timer > player_Statas.attack_delay)
                {
                    timer = 0f;
                    Fire();
                }
            }
        }
    }
    #region Bullet_Speaker
    private void Bullet_Speaker()
    {
        Debug.Log("히히 분사");
        player_Statas.attack_delay *= Bullet_Speaker_Speed;
        Debug.Log(player_Statas.attack_delay);

        StartCoroutine(FireBurst());
        Bullet_Speaker_Shot_count = 0;
        player_Statas.attack_delay = originalAttackDelay; // 공격 속도 원상복구
    }
    IEnumerator FireBurst()
    {
        for (int short_count = 0; short_count < Bullet_Speaker_Shot_Max_count; short_count++)
        {
            Fire(); // 총알 발사
            yield return new WaitForSeconds(player_Statas.attack_delay); // 0.3초 딜레이 후 반복
        }
    }
    #endregion
    Transform GetFarthest()
    {
        Transform result = null;
        float maxDiff = 0; // 최소 거리 대신 최대 거리 추적

        foreach (RaycastHit target in targets_monster)
        {
            Vector3 mypos = transform.position;
            Vector3 targetpos = target.transform.position;
            float curDiff = Vector3.Distance(mypos, targetpos);

            if (curDiff > maxDiff) // 더 먼 적을 찾으면 업데이트
            {
                maxDiff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit target in targets_monster)
        {
            Vector3 mypos = transform.position;
            Vector3 targetpos = target.transform.position;
            float curDiff = Vector3.Distance(mypos, targetpos);

            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
    public void Fire()
    {
        Bullet_Speaker_Shot_count += 1;
        if (player_attack) return;
        if (!nearestTarget) return;

        player_attack = true;
        Audio_Manager.instance.PlaySfx(Audio_Manager.SFX.atk);
        targetPos = nearestTarget.position;
        Vector3 bullet_dir = targetPos - Fire_Point.transform.position;
        Transform bullet = GameManager.Instance.pool.Bullet_Get(0).transform;
        bullet.position = Fire_Point.transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, bullet_dir);
        // 총알의 현재 위치에서 이펙트 생성
        //GameObject effect = Instantiate(FireEffectPrefab, Fire_Point.transform.position, Quaternion.identity);
        //// 일정 시간 후 파티클 삭제
        //Destroy(effect, 1f);
        bullet.GetComponent<Bullet>().Init(player_Statas.bullet_damage, player_Statas.bullet_count, bullet_dir);
        StartCoroutine(ResetFire());
    }

    public void Player_Rotator()
    {
        // 타겟 방향 계산
        targetPos = nearestTarget.position;
        Vector3 dir = targetPos - transform.position; // 캐릭터 중심에서 타겟 방향 계산
        dir.y = 0; // 수직 방향 제거 (수평 회전만)
        // 기본 목표 회전 계산
        Quaternion targetRotation = Quaternion.LookRotation(dir.normalized * Time.deltaTime); // 정규화된 방향 벡터로 목표 회전 계산

        // Y축에 35도 추가
        Quaternion adjustedRotation = targetRotation * Quaternion.Euler(0, 45f, 0); // Y축 +35도 추가
        // 부드러운 회전 (현재 회전에서 목표 회전으로 점진적으로 회전)
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * 5f); // 5f는 회전 속도
    }
    private IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(player_Statas.attack_delay);
        Audio_Manager.instance.PlaySfx(Audio_Manager.SFX.cocked);
        player_attack = false;
    }
}