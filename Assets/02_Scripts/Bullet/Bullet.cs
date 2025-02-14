using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    private List<Collider> hitEnemies = new List<Collider>(); // �̹� ���� �� ���
    private float hitCooldown = 0.5f; // ���� ���� �ٽ� ���� �ʵ��� �ϴ� �ð�(��)
    private Rigidbody rigid;
    public Player_Scaner player; // �÷��̾� ��ġ ����
    private ParticleSystem bulletParticles; // ��ƼŬ �ý��� ����
    public GameObject Bullet_Boomerang_Prefab;

    [Header("## -- Bullet -- ##")]
    public float speed = 60f;           // źȯ �ӵ�
    public float damage;
    public int per;


    [Header("## -- Bullet_Bounce -- ##")]
    public int maxBounces = 5;          // �ִ� ƨ�� Ƚ��
    public int bounceCount = 0;        // ���� ƨ�� Ƚ��

    [Header("## -- Bullet_Penetrate -- ##")]
    public int max_penetration = 5;
    public int penetration = 0;

    [Header("## -- Bullet_Bool_Type -- ##")]
    public bool Bullet_bounce_Type;
    public bool BUllet_penetrate_Type;
    public bool Bullet_NucBack_Type;
    public bool Bullet_Boomerang_Type;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // ������, ����, �ӵ�
    public void Init(float dmg, int per, Vector3 dir)
    {
        this.damage = dmg;
        this.per = per;

        // źȯ�� ���Ӱ� Ȱ��ȭ�� �� ��ƼŬ ���
        if (bulletParticles != null)
        {
            bulletParticles.Play();
        }

        bounceCount = 0;
        penetration = 0;
        rigid.velocity = dir.normalized * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // ���� ����ٸ�
        {
            //�˹� ���δ� ���Ϳ��� ����
            //���Ÿ� ���δ� �÷��̾� ��ĵ���� ����

            //źȯ�� ���ݷ¿� ���� ũ�� �� �˹� ����
            //������ ������ ��̶�� ���� Ȱ���� ��
            if (Bullet_bounce_Type) Bullet_bounce(other);
            if (BUllet_penetrate_Type) BUllet_penetrate();
            if (Bullet_Boomerang_Type) Bullet_Boomerang(other);
            if (!Bullet_bounce_Type &&  !BUllet_penetrate_Type || Bullet_NucBack_Type) gameObject.SetActive(false);
        }
    }
    #region BUllet_penetrate
    private void BUllet_penetrate()
    {
        if (penetration >= max_penetration) gameObject.SetActive(false);
        else penetration += 1;
        StartCoroutine(DestroyAfterDelay(3f)); // 3�� �� �ڵ� ����
    }
    #endregion
    #region Bullet_bounce
    private void Bullet_bounce(Collider other)
    {
        // ���� ���� �����ؼ� �´��� Ȯ��
        if (hitEnemies.Contains(other)) return;

        // ���� ���� ��Ͽ� �߰�
        hitEnemies.Add(other);
        StartCoroutine(RemoveFromHitListAfterDelay(other)); // ���� �ð� �� ����

        Vector3 contactPoint = GetContactPoint(other); // �浹 ���� ã��
        Vector3 nextDirection = FindGeneralEnemyDirection(contactPoint);

        if (nextDirection == Vector3.zero)
        {
            gameObject.SetActive(false);
            return;
        }

        // ���� źȯ ���� �� �� źȯ ����
        gameObject.SetActive(false);
        Transform newBullet = GameManager.Instance.pool.Bullet_Get(0).transform;

        newBullet.position = contactPoint;  // ���� �浹�� ��ġ���� ����
        newBullet.position = new Vector3(newBullet.position.x, contactPoint.y, newBullet.position.z); // Y�� ���� ����
        newBullet.rotation = Quaternion.LookRotation(nextDirection); // ���� ����

        Rigidbody newRb = newBullet.GetComponent<Rigidbody>();
        if (newRb != null) newRb.velocity = nextDirection * speed;
        bounceCount += 1;
        StartCoroutine(DestroyAfterDelay(3f)); // 3�� �� �ڵ� ����
    }
    /// ���� �ð��� ���� �� hitEnemies���� �����ϴ� �Լ�
    private IEnumerator RemoveFromHitListAfterDelay(Collider enemy)
    {
        yield return new WaitForSeconds(hitCooldown);
        hitEnemies.Remove(enemy);
    }
    /// ���� �浹�� ��ġ�� ã�� �Լ�
    Vector3 GetContactPoint(Collider enemy)
    {
        Bounds bounds = enemy.bounds;
        Vector3 hitPosition = bounds.center;  // �⺻������ ���� �߾� ���
        hitPosition.y = Mathf.Max(bounds.center.y, bounds.min.y + 0.5f); // �ʹ� ���� ������ ������ �ʵ��� ����
        return hitPosition;
    }
    /// ���� �ִ� ������ ã�� �Լ�
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
    /// 3�� �� źȯ�� ��Ȱ��ȭ�ϴ� �Լ�
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableBullet();
    }
    #endregion
    #region Bullet_bounce
    private void Bullet_Boomerang(Collider other)
    {
        Vector3 contactPoint = GetContactPoint(other);
        // �Ѿ��� ������ ��ġ ���
        Vector3 spawnPosition = contactPoint - other.transform.forward * 1.5f;

        // �Ѿ� ����
        Instantiate(Bullet_Boomerang_Prefab, spawnPosition, Quaternion.identity);
    }
    #endregion

    /// źȯ ��Ȱ��ȭ �Լ� (��ƼŬ�� �����ϵ�, �ܻ� ����)
    private void DisableBullet()
    {
        // ��ƼŬ�� ���ߵ�, ���� ��ƼŬ�� �������� ���� (�ܻ� ����)
        if (bulletParticles != null)
        {
            bulletParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // źȯ�� ȭ�� ������ �̵��Ͽ� �ܻ��� ������ �ʵ��� ó��
        transform.position = Vector3.one * 1000f;

        // SetActive(false)�� Ǯ�� �ý��۰� ����
        gameObject.SetActive(false);
    }
}