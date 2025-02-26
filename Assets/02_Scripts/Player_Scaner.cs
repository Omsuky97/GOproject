using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

public class Player_Scaner : MonoBehaviour
{
    public List<Transform> Target_List = new List<Transform>(); // Ÿ�� ���� ����Ʈ

    public LayerMask targetLayer;
    public RaycastHit[] targets_monster;
    public static Transform nearestTarget;

    public GameObject Fire_Point;
    public GameManager player_Statas;
    public float rotationSpeed = 5f; // ȸ�� �ӵ�
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
        Bullet_Manager.Instance.Origianl_Bullet_Speed = player_Statas.Attack_Delay; // ���� �� ���� �� ����
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
                //�̰� ������ �Ҷ����� 6�� �ٿ��� Ȯ�� ������ ��
                int Random_Speaker_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Speaker_Count); // 0~5 ������ ���� ��
                if (timer > player_Statas.Attack_Delay && Random_Speaker_Value == 0)
                {
                    timer = 0f;
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
                                int Random_Bezier_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Bezier_Count); // 0~5 ������ ���� ��
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
                        int Random_Bezier_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Bezier_Count); // 0~5 ������ ���� ��
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
                    int Random_Bezier_Value = Random.Range(0, Bullet_Manager.Instance.Bullet_Bezier_Count); // 0~5 ������ ���� ��
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
        player_Statas.Attack_Delay = Bullet_Manager.Instance.Origianl_Bullet_Speed; // ���� �ӵ� ���󺹱�
    }
    IEnumerator FireBurst()
    {
        for (int short_count = 0; short_count < Bullet_Manager.Instance.Shot_Max_count; short_count++)
        {
            if (Bullet_Manager.Instance.Bullet_ShotGun_Type) Bullet_ShotGun();
            else if (!Bullet_Manager.Instance.Bullet_ShotGun_Type) Fire(); // �Ѿ� �߻�
            yield return new WaitForSeconds(player_Statas.Attack_Delay); // 0.3�� ������ �� �ݺ�
        }
    }
    #endregion

    Transform GetFarthest()
    {
        Transform result = null;
        float maxDiff = 0; // �ּ� �Ÿ� ��� �ִ� �Ÿ� ����

        foreach (RaycastHit target in targets_monster)
        {
            Vector3 mypos = transform.position;
            Vector3 targetpos = target.transform.position;
            float curDiff = Vector3.Distance(mypos, targetpos);

            if (curDiff > maxDiff) // �� �� ���� ã���� ������Ʈ
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
        Target_List.RemoveAll(t => t == null || !t.gameObject.activeSelf); // ��Ȱ��ȭ�� ��� ������Ʈ ����
        StartCoroutine(ResetFire());
    }
    private void Bullet_ShotGun()
    {
        // �÷��̾ ���� ����
        player_attack = true;
        anim.SetBool("Fire", Fire_Anim);
        Audio_Manager.instance.GetAttack_Sound();
        targetPos = nearestTarget.position;

        Vector3 bullet_dir = (targetPos - Fire_Point.transform.position).normalized;
        bullet_dir.y = 0; // Y�� ����

        Bullet_Manager.Instance.Bullet_ShotGun_Count = Mathf.Clamp(Bullet_Manager.Instance.Bullet_ShotGun_Count, 1, 5);
        float angleStep = 45f / (Bullet_Manager.Instance.Bullet_ShotGun_Count - 1);

        short Fire_Effect_num = 1;
        Bullet_Manager.Instance.Effect_Fire(Fire_Effect_num, Fire_Point.transform.position);

        for (int i = 0; i < Bullet_Manager.Instance.Bullet_ShotGun_Count; i++)
        {
            float angleOffset = (i * angleStep) - 22.5f; // �߽��� �������� �¿� �յ� �й�
            Vector3 nextDirection = Quaternion.Euler(0, angleOffset, 0) * bullet_dir; // Ÿ�� ������ �������� ������ ����
            nextDirection.y = 0; // Y�� ����

            Vector3 spawnPosition = Fire_Point.transform.position + nextDirection;
            Transform bullet = GameManager.Instance.pool.Bullet_Get(3).transform;
            bullet.position = spawnPosition;

            Quaternion lookAtTarget = Quaternion.LookRotation(new Vector3(targetPos.x, spawnPosition.y, targetPos.z) - spawnPosition); // Y���� ������ ���� ���
            bullet.rotation = Quaternion.Euler(90, lookAtTarget.eulerAngles.y, lookAtTarget.eulerAngles.z); // X�� 90�� ����

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = nextDirection.normalized * Bullet_Manager.Instance.Bullet_Speed;
        }
        // ����
        StartCoroutine(ResetFire());
    }
    public void Player_Rotator()
    {
        // Ÿ�� ���� ���
        targetPos = nearestTarget.position;
        Vector3 dir = targetPos - transform.position; // ĳ���� �߽ɿ��� Ÿ�� ���� ���
        dir.y = 0; // ���� ���� ���� (���� ȸ����)
        // �⺻ ��ǥ ȸ�� ���
        Quaternion targetRotation = Quaternion.LookRotation(dir.normalized * Time.deltaTime); // ����ȭ�� ���� ���ͷ� ��ǥ ȸ�� ���

        // Y�࿡ 35�� �߰�
        Quaternion adjustedRotation = targetRotation * Quaternion.Euler(0, -15f, 0); // Y�� +35�� �߰�
        // �ε巯�� ȸ�� (���� ȸ������ ��ǥ ȸ������ ���������� ȸ��)
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * 5f); // 5f�� ȸ�� �ӵ�
    }
    private IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(player_Statas.Attack_Delay);
        Fire_Anim = false;
        anim.SetBool("Fire", Fire_Anim);
        player_attack = false;
    }
    #region Bullet_Fire_Bezier
    //������ � �Ѿ� �߻�
    public void Bullet_Fire_Bezier()
    {
        if (Target_List.Count == 0) return; // Ÿ���� ������ ����
        Target_List.RemoveAll(t => t == null || !t.gameObject.activeSelf); // ��Ȱ��ȭ�� ��� ������Ʈ ����
        Transform randomTarget = Target_List[Random.Range(0, Target_List.Count)]; // ���� Ÿ�� ����

        player_attack = true;

        Vector3 start = Fire_Point.transform.position;
        Vector3 end = randomTarget.position;

        // **������ ���� (������� Ÿ�� ������ �߰�, �¿�� ���̵��� ����)**
        Vector3 midPoint = (start + end) / 2f;
        midPoint.z += Random.Range(0, 2) == 0 ? -10f : 10f; // Z�� �������� -10 �Ǵ� 10 �߰�
        Vector3 controlPoint = midPoint;

        // **������Ʈ Ǯ���� ����� �Ѿ� ����**
        Transform bulletObj = GameManager.Instance.pool.Bullet_Get(2).transform;

        if (bulletObj != null)
        {
            bulletObj.gameObject.SetActive(true); // �Ѿ� Ȱ��ȭ
            bulletObj.transform.position = start;
            bulletObj.transform.rotation = Quaternion.identity;

            Transform bullet = bulletObj.transform;

            // **��ǥ ������ ���ϵ��� ȸ�� ����**
            Quaternion targetRotation = Quaternion.LookRotation(end - start);
            bullet.rotation = targetRotation;
            // **�ڷ�ƾ ���� (������ � ���� �̵�)**
            StartCoroutine(MoveAlongBezierCurve(bullet, start, controlPoint, end, Bullet_Manager.Instance.Bullet_Speed, randomTarget));
        }
    }
    private IEnumerator MoveAlongBezierCurve(Transform bullet, Vector3 start, Vector3 control, Vector3 end, float speed, Transform target)
    {
        float time = 0;
        float duration = 1f; // �Ѿ� �̵� �ð�
                             // **Y���� ������ ������ ����**
        // **�� �Ѿ˸��� ���ο� ��ǥ ������ �����Ͽ� �ߺ� ����**
        Vector3 fixedStart = start;
        float fixedY = fixedStart.y; // ó�� ������ Y���� ����
        Vector3 fixedEnd = end + (end - control).normalized * 30f;

        // **Y�� ����**
        fixedStart.y = fixedY;
        fixedEnd.y = fixedY;
        control.y = fixedY;

        // **A �� B (������ � �̵� + ����)**
        while (time < duration + 2f) // �Ѿ��� �������� �����Ͽ� �̵��ϵ��� �ð� Ȯ��
        {
            float t = Mathf.Clamp01(time / duration); // t ���� 0 ~ 1�� ����
            t = t * t; // ���� ���� (õõ�� �����ؼ� ���� ������)

            // **������ � ���� ���� (������ ��ǥ ���� ���)**
            Vector3 bezierPoint = Mathf.Pow(1 - t, 2) * fixedStart +
                                  2 * (1 - t) * t * control + 
                                  Mathf.Pow(t, 2) * fixedEnd;

            // **End �������� �̵��ϴ� ���ȸ� ������ � ����**
            if (time < duration)
            {
                bullet.position = bezierPoint;

                // **�Ѿ��� ���� ������ �ٶ󺸵��� ȸ��**
                Vector3 tangent = 2 * (1 - t) * (control - fixedStart) + 2 * t * (fixedEnd - control);
                bullet.rotation = Quaternion.LookRotation(tangent);
            }
            else // **End ���� ���Ŀ��� ���� �������� ��� ��� ����**
            {
                Vector3 direction = (fixedEnd - control).normalized; // ������ � ���� ����
                bullet.position += direction * speed * Time.deltaTime; // ��� ����
            }

            time += Time.deltaTime;
            yield return null;
        }

        // **�Ѿ� ��Ȱ��ȭ**
        yield return new WaitForSeconds(3f);
        bullet.gameObject.SetActive(false);

        // **��Ȱ��ȭ�� Ÿ���� ����Ʈ���� ����**
        Target_List.RemoveAll(t => t == null || !t.gameObject.activeSelf);
    }
    #endregion
}