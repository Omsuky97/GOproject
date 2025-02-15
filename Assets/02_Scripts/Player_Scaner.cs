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
    private float originalAttackDelay; // ���� ���� ������ �� ����

    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    public float Bullet_Speaker_Speed = 0.3f; //= ����
    public float boostDuration_start = 3f;         // ���� ���� �ð�
    public float boostDuration_End = 3f;         // ���� ���� �ð�
    public bool Bullet_Speaker_Type;
    public bool Bullet_Speaker_isBoosted;
    public short Bullet_Speaker_Shot_count;
    public short Bullet_Speaker_Shot_Max_count;

    public float timer;
    public float boostDuration_End_Timer;
    bool player_attack = false;
    Vector3 targetPos;

    //��ƼŬ
    public GameObject FireEffectPrefab; // �¾��� �� ������ ��ƼŬ ������

    private void Start()
    {
        originalAttackDelay = player_Statas.attack_delay; // ���� �� ���� �� ����
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
                //�̰� ������ �Ҷ����� 6�� �ٿ��� Ȯ�� ������ ��
                int randomValue = Random.Range(0, 6); // 0~5 ������ ���� ��
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
        Debug.Log("���� �л�");
        player_Statas.attack_delay *= Bullet_Speaker_Speed;
        Debug.Log(player_Statas.attack_delay);

        StartCoroutine(FireBurst());
        Bullet_Speaker_Shot_count = 0;
        player_Statas.attack_delay = originalAttackDelay; // ���� �ӵ� ���󺹱�
    }
    IEnumerator FireBurst()
    {
        for (int short_count = 0; short_count < Bullet_Speaker_Shot_Max_count; short_count++)
        {
            Fire(); // �Ѿ� �߻�
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
        Vector3 bullet_dir = new Vector3(targetPos.x - Fire_Point.transform.position.x, 0f, targetPos.z - Fire_Point.transform.position.z);
        Transform bullet = GameManager.Instance.pool.Bullet_Get(0).transform;
        bullet.position = Fire_Point.transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, bullet_dir);
        // �Ѿ��� ���� ��ġ���� ����Ʈ ����
        //GameObject effect = Instantiate(FireEffectPrefab, Fire_Point.transform.position, Quaternion.identity);
        //// ���� �ð� �� ��ƼŬ ����
        //Destroy(effect, 1f);
        bullet.GetComponent<Bullet>().Init(player_Statas.bullet_damage, player_Statas.bullet_count, bullet_dir);
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
}