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

    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit[] targets_monster;
    public Transform nearestTarget;

    public GameObject Fire_Point;
    public GameManager player_Statas;
    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    public float timer;
    bool player_attack = false;
    Vector3 targetPos;

    [Header("## -- Bullet_Target_Type -- ##")]
    public bool target_type = false;

    [Header("## -- Bullet_Participle -- ##")]
    public bool Bullet_Participle_Type;
    public float Bullet_Speaker_Speed = 0.3f; //= ����
    public float boostDuration_start = 3f;         // ���� ���� �ð�
    public float boostDuration_End = 3f;         // ���� ���� �ð�

    [Header("## -- Bullet_Bezier -- ##")]
    public bool Bullet_Bezier_Type;
    public GameObject Bullet_Bezier;

    [Header("## -- Bullet_Speaker -- ##")]
    public bool Bullet_Speaker_Type;
    private float originalAttackDelay; // ���� ���� ������ �� ����
    public short Bullet_Speaker_Shot_count;
    public short Bullet_Speaker_Shot_Max_count;

    //��ƼŬ
    public GameObject FireEffectPrefab; // �¾��� �� ������ ��ƼŬ ������

    private void Start()
    {
        originalAttackDelay = player_Statas.attack_delay; // ���� �� ���� �� ����
    }
    private void FixedUpdate()
    {

        //������ �����°� Ȯ�� �� ������ ����� ����
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
                //�̰� ������ �Ҷ����� 6�� �ٿ��� Ȯ�� ������ ��
                int Random_Speaker_Value = Random.Range(0, 6); // 0~5 ������ ���� ��
                if (timer > player_Statas.attack_delay && Random_Speaker_Value == 0) Bullet_Speaker();
                else
                {
                    if (Bullet_Participle_Type)
                        if (timer > player_Statas.attack_delay)
                        {
                            timer = 0f;
                            Bullet_Participle();
                        }
                        else if (!Bullet_Participle_Type)
                            if (timer > player_Statas.attack_delay)
                            {
                                timer = 0f;
                                Fire();
                                int Random_Bezier_Value = Random.Range(0, 1); // 0~5 ������ ���� ��
                                if (Bullet_Bezier_Type && Random_Bezier_Value == 0) Bullet_Fire_Bezier();
                            }
                }
            }
            else if (!Bullet_Speaker_Type && Bullet_Participle_Type)
                if (timer > player_Statas.attack_delay)
                {
                    timer = 0f;
                    Bullet_Participle();
                }
                else
                {
                    if (timer > player_Statas.attack_delay)
                    {
                        timer = 0f;
                        Fire();
                        int Random_Bezier_Value = Random.Range(0, 1); // 0~5 ������ ���� ��
                        if (Bullet_Bezier_Type && Random_Bezier_Value == 0) Bullet_Fire_Bezier();
                    }
                }
            else
            {
                if (timer > player_Statas.attack_delay)
                {
                    timer = 0f;
                    Fire();
                    int Random_Bezier_Value = Random.Range(0, 1); // 0~5 ������ ���� ��
                    if (Bullet_Bezier_Type && Random_Bezier_Value == 0) Bullet_Fire_Bezier();
                }
            }
        }
    }
    #region Bullet_Speaker
    private void Bullet_Speaker()
    {
        player_Statas.attack_delay *= Bullet_Speaker_Speed;

        StartCoroutine(FireBurst());
        Bullet_Speaker_Shot_count = 0;
        player_Statas.attack_delay = originalAttackDelay; // ���� �ӵ� ���󺹱�
    }
    IEnumerator FireBurst()
    {
        for (int short_count = 0; short_count < Bullet_Speaker_Shot_Max_count; short_count++)
        {
            if (Bullet_Participle_Type) Bullet_Participle();
            else if (!Bullet_Participle_Type) Fire(); // �Ѿ� �߻�
            yield return new WaitForSeconds(player_Statas.attack_delay); // 0.3�� ������ �� �ݺ�
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
            }
            Target_List.Add(target.transform);
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
        Vector3 bullet_dir = new Vector3(targetPos.x - Fire_Point.transform.position.x, 0f, targetPos.z - Fire_Point.transform.position.z);
        Transform bullet = GameManager.Instance.pool.Bullet_Get(0).transform;
        bullet.position = Fire_Point.transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, bullet_dir);
        // �Ѿ��� ���� ��ġ���� ����Ʈ ����
        //GameObject effect = Instantiate(FireEffectPrefab, Fire_Point.transform.position, Quaternion.identity);
        //// ���� �ð� �� ��ƼŬ ����
        //Destroy(effect, 1f);
        bullet.GetComponent<Bullet>().Init(player_Statas.bullet_damage, bullet_dir);
        Target_List.RemoveAll(t => t == null || !t.gameObject.activeSelf); // ��Ȱ��ȭ�� ��� ������Ʈ ����
        StartCoroutine(ResetFire());
    }


    private void Bullet_Participle()
    {
        // �÷��̾ ���� ����
        player_attack = true;
        Audio_Manager.instance.PlaySfx(Audio_Manager.SFX.atk);
        targetPos = nearestTarget.position;

        Vector3 bullet_dir = (targetPos - Fire_Point.transform.position).normalized; // Ÿ�� ���� ���

        player_Statas.bullet_count = Mathf.Clamp(player_Statas.bullet_count, 1, 5);
        float angleStep = 45f / (player_Statas.bullet_count - 1);

        for (int i = 0; i < player_Statas.bullet_count; i++)
        {
            float angleOffset = (i * angleStep) - 22.5f; // �߽��� �������� �¿� �յ� �й�
            Vector3 nextDirection = Quaternion.Euler(0, angleOffset, 0) * bullet_dir; // Ÿ�� ������ �������� ������ ����

            // �߻� ��ġ ���
            Vector3 spawnPosition = Fire_Point.transform.position + nextDirection;

            // �Ѿ� ���� �� ���� ����
            Transform bullet = GameManager.Instance.pool.Bullet_Get(0).transform;
            bullet.position = spawnPosition;
            bullet.rotation = Quaternion.LookRotation(nextDirection);

            // Bullet ������Ʈ �ʱ�ȭ �� �ӵ� ����
            bullet.GetComponent<Bullet>().Init(player_Statas.bullet_damage, nextDirection);
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
        Quaternion adjustedRotation = targetRotation * Quaternion.Euler(0, 45f, 0); // Y�� +35�� �߰�
        // �ε巯�� ȸ�� (���� ȸ������ ��ǥ ȸ������ ���������� ȸ��)
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * 5f); // 5f�� ȸ�� �ӵ�
    }
    private IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(player_Statas.attack_delay);
        Audio_Manager.instance.PlaySfx(Audio_Manager.SFX.cocked);
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
        Audio_Manager.instance.PlaySfx(Audio_Manager.SFX.atk);

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
            StartCoroutine(MoveAlongBezierCurve(bullet, start, controlPoint, end, GameManager.Instance.Bullet_Speed, randomTarget));
        }
    }
    //Target_List.Remove(tarfget);
    ////������ �
    /// <summary>

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