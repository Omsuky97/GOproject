using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

public class Player_Scaner : MonoBehaviour
{
    public List<Transform> Target_List = new List<Transform>(); // 타겟 저장 리스트

    public LayerMask targetLayer;
    public RaycastHit[] targets_monster;
    public static Transform nearestTarget;

    public GameObject Fire_Point;
    public GameManager player_Statas;
    public float rotationSpeed = 5f; // 회전 속도
    public float timer;
    bool player_attack = false;
    Vector3 targetPos;
    public Animator anim;
    public bool Fire_Anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        Bullet_Manager.Instance.Origianl_Bullet_Speed = player_Statas.Attack_Delay; // 시작 시 원래 값 저장
    }
    private void FixedUpdate()
    {
        targets_monster = Physics.SphereCastAll(transform.position, Bullet_Manager.Instance.Bullet_Scan_Range, Vector3.forward, Bullet_Manager.Instance.Bullet_Scan_Range, targetLayer);
        if (!Bullet_Manager.Instance.Bullet_Target_type) nearestTarget = GetNearest();
        else if (Bullet_Manager.Instance.Bullet_Target_type) nearestTarget = GetFarthest();
        if (nearestTarget != null)
        {
            Player_Rotator();
            Fire_Anim = true;
            if (!player_attack) timer += Time.deltaTime;
            if (Bullet_Manager.Instance.Bullet_Speaker_Type)
            {
                //이거 레벨업 할때마다 6을 줄여서 확률 높여줄 것
                int Random_Speaker_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Speaker_Count); // 0~5 사이의 랜덤 값
                if (timer > player_Statas.Attack_Delay && Random_Speaker_Value == 0)
                {
                    Bullet_Speaker();
                }
                else
                {
                    if (Bullet_Manager.Instance.Bullet_ShotGun_Type)
                        if (timer > player_Statas.Attack_Delay)
                        {
                            timer = 0f;
                            Bullet_ShotGun();
                        }
                        else if (!Bullet_Manager.Instance.Bullet_ShotGun_Type)
                            if (timer > player_Statas.Attack_Delay)
                            {
                                timer = 0f;
                                Fire();
                                int Random_Bezier_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Bezier_Count); // 0~5 사이의 랜덤 값
                                if (Bullet_Manager.Instance.Bullet_Bezier_Type && Random_Bezier_Value == 0) Bullet_Fire_Bezier();
                            }
                }
            }
            else if (!Bullet_Manager.Instance.Bullet_Speaker_Type && Bullet_Manager.Instance.Bullet_ShotGun_Type)
            {
                if (timer > player_Statas.Attack_Delay)
                {
                    timer = 0f;
                    Bullet_ShotGun();
                }
                else
                {
                    if (timer > player_Statas.Attack_Delay)
                    {
                        timer = 0f;
                        Fire();
                        int Random_Bezier_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Bezier_Count); // 0~5 사이의 랜덤 값
                        if (Bullet_Manager.Instance.Bullet_Bezier_Type && Random_Bezier_Value == 0) Bullet_Fire_Bezier();
                    }
                }
            }
            else
            {
                if (timer > player_Statas.Attack_Delay)
                {
                    timer = 0f;
                    Fire();
                    int Random_Bezier_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Bezier_Count); // 0~5 사이의 랜덤 값
                    if (Bullet_Manager.Instance.Bullet_Bezier_Type && Random_Bezier_Value == 0) Bullet_Fire_Bezier();
                }
            }
        }
    }
    #region Bullet_Speaker
    private void Bullet_Speaker()
    {
        player_Statas.Attack_Delay *= Bullet_Manager.Instance.Speaker_Speed;
        StartCoroutine(FireBurst());
        player_Statas.Attack_Delay = Bullet_Manager.Instance.Origianl_Bullet_Speed; // 공격 속도 원상복구
    }
    IEnumerator FireBurst()
    {
        for (int short_count = 0; short_count < Bullet_Manager.Instance.Shot_Max_count; short_count++)
        {
            if (Bullet_Manager.Instance.Bullet_ShotGun_Type) Bullet_ShotGun();
            else if (!Bullet_Manager.Instance.Bullet_ShotGun_Type) Fire(); // 총알 발사
            yield return new WaitForSeconds(player_Statas.Attack_Delay); // 0.3초 딜레이 후 반복
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

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
                Target_List.Add(target.transform);
            }
        }
        return result;
    }
    public void Fire()
    {
        if (player_attack) return;
        if (!nearestTarget) return;

        anim.SetBool("Fire", Fire_Anim);
        player_attack = true;
        Audio_Manager.instance.GetAttack_Sound();
        targetPos = nearestTarget.position;
        Vector3 bullet_dir = new Vector3(targetPos.x - Fire_Point.transform.position.x, 0f, targetPos.z - Fire_Point.transform.position.z);
        Transform bullet = GameManager.Instance.pool.Bullet_Get(0).transform;
        bullet.position = Fire_Point.transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, bullet_dir);
        short Fire_Effect_num = 0;
        Bullet_Manager.Instance.Effect_Fire(Fire_Effect_num, Fire_Point.transform.position);
        bullet.GetComponent<Bullet>().Init(player_Statas.bullet_damage, bullet_dir);
        Target_List.RemoveAll(t => t == null || !t.gameObject.activeSelf); // 비활성화된 모든 오브젝트 제거
        StartCoroutine(ResetFire());
    }
    private void Bullet_ShotGun()
    {
        // 플레이어가 공격 시작
        player_attack = true;
        anim.SetBool("Fire", Fire_Anim);
        Audio_Manager.instance.GetAttack_Sound();
        targetPos = nearestTarget.position;

        Vector3 bullet_dir = (targetPos - Fire_Point.transform.position).normalized; // 타겟 방향 계산

        Bullet_Manager.Instance.Bullet_ShotGun_Count = Mathf.Clamp(Bullet_Manager.Instance.Bullet_ShotGun_Count, 1, 5);
        float angleStep = 45f / (Bullet_Manager.Instance.Bullet_ShotGun_Count - 1);

        short Fire_Effect_num = 1;
        Bullet_Manager.Instance.Effect_Fire(Fire_Effect_num, Fire_Point.transform.position);
        for (int i = 0; i < Bullet_Manager.Instance.Bullet_ShotGun_Count; i++)
        {
            float angleOffset = (i * angleStep) - 22.5f; // 중심을 기준으로 좌우 균등 분배
            Vector3 nextDirection = Quaternion.Euler(0, angleOffset, 0) * bullet_dir; // 타겟 방향을 기준으로 퍼지게 조정
            Vector3 spawnPosition = Fire_Point.transform.position + nextDirection;
            Transform bullet = GameManager.Instance.pool.Bullet_Get(3).transform;
            bullet.position = spawnPosition;
            Quaternion lookAtTarget = Quaternion.LookRotation(targetPos - spawnPosition); // 타겟을 바라보는 방향
            bullet.rotation = Quaternion.Euler(90, lookAtTarget.eulerAngles.y, lookAtTarget.eulerAngles.z); // X축 90도 고정
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = nextDirection * Bullet_Manager.Instance.Bullet_Speed;
        }
        // 리셋
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
        Quaternion adjustedRotation = targetRotation * Quaternion.Euler(0, 47f, 0); // Y축 +35도 추가
        // 부드러운 회전 (현재 회전에서 목표 회전으로 점진적으로 회전)
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * 5f); // 5f는 회전 속도
    }
    private IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(player_Statas.Attack_Delay);
        Fire_Anim = false;
        anim.SetBool("Fire", Fire_Anim);
        player_attack = false;
    }
    #region Bullet_Fire_Bezier
    //벨지에 곡선 총알 발사
    public void Bullet_Fire_Bezier()
    {
        if (Target_List.Count == 0) return; // 타겟이 없으면 종료
        Target_List.RemoveAll(t => t == null || !t.gameObject.activeSelf); // 비활성화된 모든 오브젝트 제거
        Transform randomTarget = Target_List[Random.Range(0, Target_List.Count)]; // 랜덤 타겟 선택

        player_attack = true;

        Vector3 start = Fire_Point.transform.position;
        Vector3 end = randomTarget.position;

        // **제어점 설정 (출발점과 타겟 사이의 중간, 좌우로 꺾이도록 조정)**
        Vector3 midPoint = (start + end) / 2f;
        midPoint.z += Random.Range(0, 2) == 0 ? -10f : 10f; // Z축 방향으로 -10 또는 10 추가
        Vector3 controlPoint = midPoint;

        // **오브젝트 풀링을 사용한 총알 생성**
        Transform bulletObj = GameManager.Instance.pool.Bullet_Get(2).transform;

        if (bulletObj != null)
        {
            bulletObj.gameObject.SetActive(true); // 총알 활성화
            bulletObj.transform.position = start;
            bulletObj.transform.rotation = Quaternion.identity;

            Transform bullet = bulletObj.transform;

            // **목표 방향을 향하도록 회전 설정**
            Quaternion targetRotation = Quaternion.LookRotation(end - start);
            bullet.rotation = targetRotation;
            // **코루틴 실행 (베지어 곡선 따라 이동)**
            StartCoroutine(MoveAlongBezierCurve(bullet, start, controlPoint, end, Bullet_Manager.Instance.Bullet_Speed, randomTarget));
        }
    }
    private IEnumerator MoveAlongBezierCurve(Transform bullet, Vector3 start, Vector3 control, Vector3 end, float speed, Transform target)
    {
        float time = 0;
        float duration = 1f; // 총알 이동 시간
                             // **Y축을 고정된 값으로 유지**
        // **각 총알마다 새로운 목표 지점을 설정하여 중복 방지**
        Vector3 fixedStart = start;
        float fixedY = fixedStart.y; // 처음 시작한 Y값을 고정
        Vector3 fixedEnd = end + (end - control).normalized * 30f;

        // **Y축 고정**
        fixedStart.y = fixedY;
        fixedEnd.y = fixedY;
        control.y = fixedY;

        // **A → B (베지어 곡선 이동 + 직진)**
        while (time < duration + 2f) // 총알이 직진까지 포함하여 이동하도록 시간 확장
        {
            float t = Mathf.Clamp01(time / duration); // t 값을 0 ~ 1로 제한
            t = t * t; // 가속 적용 (천천히 시작해서 점점 빨라짐)

            // **베지어 곡선 공식 적용 (고정된 목표 지점 사용)**
            Vector3 bezierPoint = Mathf.Pow(1 - t, 2) * fixedStart +
                                  2 * (1 - t) * t * control + 
                                  Mathf.Pow(t, 2) * fixedEnd;

            // **End 지점까지 이동하는 동안만 베지어 곡선 적용**
            if (time < duration)
            {
                bullet.position = bezierPoint;

                // **총알이 진행 방향을 바라보도록 회전**
                Vector3 tangent = 2 * (1 - t) * (control - fixedStart) + 2 * t * (fixedEnd - control);
                bullet.rotation = Quaternion.LookRotation(tangent);
            }
            else // **End 지점 이후에는 같은 방향으로 즉시 계속 직진**
            {
                Vector3 direction = (fixedEnd - control).normalized; // 마지막 곡선 진행 방향
                bullet.position += direction * speed * Time.deltaTime; // 즉시 직진
            }

            time += Time.deltaTime;
            yield return null;
        }

        // **총알 비활성화**
        yield return new WaitForSeconds(3f);
        bullet.gameObject.SetActive(false);

        // **비활성화된 타겟을 리스트에서 제거**
        Target_List.RemoveAll(t => t == null || !t.gameObject.activeSelf);
    }
    #endregion
}