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

    public float rotationSpeed = 5f; // 회전 속도

    public float timer;
    bool player_attack = false;
    Vector3 targetPos;

    //파티클
    public GameObject FireEffectPrefab; // 맞았을 때 실행할 파티클 프리팹

    private void FixedUpdate()
    {
        targets_monster = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, scanRange, targetLayer);
        nearestTarget = GetNearest();
        if(nearestTarget != null)
        {
            Player_Rotator();
            if (!player_attack)
            {
                timer += Time.deltaTime;
            }
            if (timer > player_Statas.attack_delay)
            {
                timer = 0f;
                Fire();
            }
        }
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
