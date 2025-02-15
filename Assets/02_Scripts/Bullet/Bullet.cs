using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    private List<Collider> Hit_Bounce_Enemys = new List<Collider>(); // �̹� ���� �� ���
    private HashSet<Collider> Hit_Split_Enemys = new HashSet<Collider>(); // �̹� ���� ���� ����
    private float hitCooldown = 0.5f; // ���� ���� �ٽ� ���� �ʵ��� �ϴ� �ð�(��)
    private Rigidbody rigid;
    public Player_Scaner player; // �÷��̾� ��ġ ����
    private ParticleSystem bulletParticles; // ��ƼŬ �ý��� ����
    public GameObject Bullet_Boomerang_Prefab;

    [Header("## -- Bullet -- ##")]
    public float speed = 5;           // źȯ �ӵ�
    public float damage;
    public int Bullet_per;
    public Vector3 Bullet_dir;



    [Header("## -- Bullet_Bounce -- ##")]
    public int maxBounces = 5;          // �ִ� ƨ�� Ƚ��
    public int bounceCount = 0;         // ���� ƨ�� Ƚ��
    public float Bullet_Bounce_Spawn_Offset = 1.0f;    //�浹 ��ġ���� �̵��� �Ÿ�

    [Header("## -- Bullet_Penetrate -- ##")]
    public int max_penetration = 5;
    public int penetration = 0;

    [Header("## -- Bullet_Propulsion -- ##")]
    public float Origin_Spped;
    public float Propulsion_Speed;

    [Header("## -- BulletBoom -- ##")]
    public GameObject Bullet_Boom;

    [Header("## -- Bullet_Spirt -- ##")]
    public GameObject Bullet_Spirt;
    public int Bullet_Spirt_Count;
    public float Bullet_Spirt_Offset = 0.5f;

    [Header("## -- Bullet_Bool_Type -- ##")]
    public bool Bullet_bounce_Type;             //�Ѿ�ƨ��
    public bool BUllet_penetrate_Type;          //�Ѿ˰���
    public bool Bullet_NucBack_Type;            //�Ѿ˳˹�
    public bool Bullet_Boomerang_Type;          //�θ޶�
    public bool Bullet_Propulsion_Type;         //�Ѿ�����
    public bool Bullet_Boom_Type;               //�Ѿ�����
    public bool Bullet_Spirt_Type;              //�Ѿ˺п�

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Origin_Spped = speed;
    }
    private void LateUpdate()
    {
        if (Bullet_Propulsion_Type)
        {
            Propulsion_Speed = Propulsion_Speed + 1;
            rigid.velocity = Bullet_dir.normalized * Propulsion_Speed;
        }
    }
    // ������, ����, �ӵ�
    public void Init(float dmg, int per, Vector3 dir)
    {
        Bullet_Boom.gameObject.SetActive(false);
        damage = dmg;
        Bullet_per = per;
        Bullet_dir = dir;
        // źȯ�� ���Ӱ� Ȱ��ȭ�� �� ��ƼŬ ���
        if (bulletParticles != null) bulletParticles.Play();
        bounceCount = 0;
        penetration = 0;
        Propulsion_Speed = Origin_Spped;
        rigid.velocity = Bullet_dir.normalized * Origin_Spped;
    }
    private void OnEnable()
    {
        ResetChildRotation(); // �Ѿ��� Ȱ��ȭ�� �� ���� ������Ʈ ȸ�� �ʱ�ȭ
    }

    private void ResetChildRotation()
    {
        foreach (Transform child in transform)
        {
            Vector3 childEuler = child.localEulerAngles;
            child.localEulerAngles = new Vector3(-90, childEuler.y, childEuler.z); // X���� 0���� �ʱ�ȭ
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // ���� ����ٸ�
        {
            //�˹� ���δ� ���Ϳ��� ����
            //���Ÿ� ���δ� �÷��̾� ��ĵ���� ����
            //���� ���δ� ���Ͷ� �Ҹ� ���ÿ� ���ٰ�
            //źȯ�� ���ݷ¿� ���� ũ�� �� �˹� ����
            //������ ������ ��̶�� ���� Ȱ���� ��
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            if (Bullet_Boom_Type) Bullet_Boom.gameObject.SetActive(true);
            if (Bullet_Spirt_Type)
            {
                Hit_Split_Enemys.Add(other);
                Bullet_Split(contactPoint, Hit_Split_Enemys, other);
            }
            if (Bullet_bounce_Type)
            {
                if (Hit_Bounce_Enemys.Any(e => e.gameObject == other.gameObject)) return;

                Hit_Bounce_Enemys.Add(other);
                Bullet_bounce(other);
            }
            if (BUllet_penetrate_Type) BUllet_penetrate();
            if (Bullet_Boomerang_Type) Bullet_Boomerang(other);
            if(!Bullet_bounce_Type && !Bullet_Boom_Type) gameObject.SetActive(false);
        }
    }
    # region Bullet_Split
    private void Bullet_Split(Vector3 contactPoint, HashSet<Collider> parentHitMonsters, Collider other)
    {
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3�� �� �ڵ� ����
        }

        Vector3 originalDirection = Vector3.forward;
        Bullet_Spirt_Count = Mathf.Clamp(Bullet_Spirt_Count, 1, 5);
        float angleStep = 360f / Bullet_Spirt_Count;
        float offsetAngle = (Bullet_Spirt_Count % 2 == 0) ? angleStep / 2 : 0; // ¦�� ������ ��� �߽� ����
        for (int i = 0; i < Bullet_Spirt_Count; i++)
        {
            float angleOffset = (i * angleStep) + offsetAngle;
            Vector3 nextDirection = Quaternion.Euler(0, angleOffset, 0) * originalDirection;
            Vector3 spawnPosition = contactPoint + (nextDirection * Bullet_Spirt_Offset);
            GameObject newBullet = Instantiate(Bullet_Spirt, spawnPosition, Quaternion.LookRotation(nextDirection));

            newBullet.SetActive(true);
            Bullet_Split newBulletScript = newBullet.GetComponent<Bullet_Split>();
            if (newBulletScript != null) newBulletScript.Inherit_Hit_Monsters(parentHitMonsters);


            Rigidbody newRb = newBullet.GetComponent<Rigidbody>();
            if (newRb != null) newRb.velocity = nextDirection * speed;
            Destroy(newBullet, 3f);
        }
    }
    private IEnumerator Hit_List_Split_Remove(Collider enemy)
    {
        yield return new WaitForSeconds(hitCooldown);
        Hit_Split_Enemys.Remove(enemy);
    }
    #endregion
    #region BUllet_penetrate
    private void BUllet_penetrate()
    {
        if (penetration >= max_penetration) gameObject.SetActive(false);
        else penetration += 1;
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3�� �� �ڵ� ����
        }

    }
    #endregion
    #region Bullet_bounce
    private void Bullet_bounce(Collider other)
    {
        // �ִ� ƨ�� Ƚ���� �ʰ��ϸ� �Ѿ� ��Ȱ��ȭ
        if (bounceCount >= maxBounces)
        {
            gameObject.SetActive(false);
            return;
        }

        // �浹 ���� �� ���ο� ���� ã��
        Vector3 contactPoint = GetContactPoint(other);
        Vector3 nextDirection = FindGeneralEnemyDirection(contactPoint);

        // ƨ�� ������ ������ ��Ȱ��ȭ
        if (nextDirection == Vector3.zero)
        {
            gameObject.SetActive(false);
            return;
        }
        // ���� �Ѿ��� ��ġ �� ���� ���� (�θ�� �״��)
        transform.position = contactPoint + (nextDirection.normalized * Bullet_Spirt_Offset);
        transform.rotation = Quaternion.LookRotation(nextDirection);

        // **���� ������Ʈ���� X�� ȸ���� 90���� ����**
        foreach (Transform child in transform)
        {
            Vector3 childEuler = child.localEulerAngles;
            child.localEulerAngles = new Vector3(0, childEuler.y, childEuler.z);
        }

        // ���� �Ѿ��� Rigidbody �ӵ��� ������Ʈ�Ͽ� ƨ��� ��
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = nextDirection * speed;
        }

        bounceCount += 1;
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3�� �� �ڵ� ����
        }
    }
    private Vector3 GetContactPoint(Collider other)
    {
        return other.ClosestPoint(transform.position);
    }
    Vector3 FindGeneralEnemyDirection(Vector3 currentPosition)
    {
        float searchRadius = 10f; // Ž�� �ݰ�
        Collider[] enemies = Physics.OverlapSphere(currentPosition, searchRadius);

        if (enemies.Length == 0) return Vector3.zero;

        Vector3 generalDirection = Vector3.zero;
        int count = 0;

        foreach (Collider enemy in enemies)
        {
            if (!enemy.CompareTag("Enemy")) continue;
            if (Vector3.Distance(enemy.transform.position, currentPosition) < 0.1f) continue;

            generalDirection += (enemy.transform.position - currentPosition).normalized;
            count++;
        }

        if (count > 0)
        {
            generalDirection /= count;
            generalDirection.y = 0;
            generalDirection.Normalize();
        }

        return generalDirection;
    }
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    #endregion
    #region Bullet_Boomerang
    private void Bullet_Boomerang(Collider other)
    {
        Vector3 contactPoint = GetContactPoint(other);
        // �Ѿ��� ������ ��ġ ���
        Vector3 spawnPosition = contactPoint - other.transform.forward * 3f;
        // �Ѿ� ����
        Instantiate(Bullet_Boomerang_Prefab, spawnPosition, Quaternion.identity);
    }
    #endregion
}