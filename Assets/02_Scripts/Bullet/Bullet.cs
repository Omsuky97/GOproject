using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    public Player_Scaner player; // �÷��̾� ��ġ ����

    public float speed = 60f;           // źȯ �ӵ�

    public int maxBounces = 5;          // �ִ� ƨ�� Ƚ��
    public int bounceCount = 0;        // ���� ƨ�� Ƚ��

    public int max_penetration = 5;
    public int penetration = 0;

    public float disableTime = 0.5f; // �ִϸ��̼� ���� �ð�

    public bool Bullet_bounce_Type;
    public bool BUllet_penetrate_Type;
    public bool Bullet_NucBack_Type;

    public float damage;
    public int per;
    Rigidbody rigid;
    private ParticleSystem bulletParticles; // ��ƼŬ �ý��� ����

    private List<Collider> hitEnemies = new List<Collider>(); // �̹� ���� �� ���
    private float hitCooldown = 0.5f; // ���� ���� �ٽ� ���� �ʵ��� �ϴ� �ð�(��)

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

        if (per > -1)
        {
            rigid.velocity = dir.normalized * speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // ���� ����ٸ�
        {
            if (Bullet_bounce_Type) Bullet_bounce(other);
            if (BUllet_penetrate_Type) BUllet_penetrate();
            //if (Bullet_NucBack_Type) Bullet_NucBack(hitPoint, bulletDirection, NucBack_distance); // �˹� ����
            if (!Bullet_bounce_Type &&  !BUllet_penetrate_Type || Bullet_NucBack_Type) gameObject.SetActive(false);
            
        }
    }
    private void BUllet_penetrate()
    {
        if (penetration >= max_penetration) gameObject.SetActive(false);
        else penetration += 1;
        StartCoroutine(DestroyAfterDelay(3f)); // 3�� �� �ڵ� ����
    }
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

    /// 3�� �� źȯ�� ��Ȱ��ȭ�ϴ� �Լ�
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableBullet();
    }

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
}