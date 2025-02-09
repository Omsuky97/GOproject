using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 60f;           // źȯ �ӵ�
    public int maxBounces = 1;          // �ִ� ƨ�� Ƚ��
    public int bounceCount = 0;        // ���� ƨ�� Ƚ��

    public float damage;
    public int per;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // ������, ����, �ӵ�
    public void Init(float dmg, int per ,Vector3 dir)
    {
        this.damage = dmg;
        this.per = per;

        bounceCount = 0;

        if (per > -1)
        {
            rigid.velocity = dir.normalized * speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // ���� ����ٸ�
        {
            Debug.Log("���� �浹! ���� �̵� ���� ã��");
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Debug.Log("�ִ� ƨ�� Ƚ�� ����! źȯ ����");
                gameObject.SetActive(false); // ���� źȯ ����
                return;
            }

            Vector3 contactPoint = GetContactPoint(other); // �浹 ���� ã��
            Vector3 nextDirection = FindGeneralEnemyDirection(contactPoint);

            if (nextDirection == Vector3.zero)
            {
                Debug.Log("���� �̵� ���� ����. źȯ ����");
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
            if (newRb != null)
            {
                newRb.velocity = nextDirection * speed;
            }

            Debug.Log("�� źȯ ����! ����: " + nextDirection);
        }
    }

    /// ���� �浹�� ��ġ�� ã�� �Լ� (���� �߽� ���̿��� ����)
    Vector3 GetContactPoint(Collider enemy)
    {
        Bounds bounds = enemy.bounds;
        Vector3 hitPosition = bounds.center;  // �⺻������ ���� �߾� ���

        // Y���� ���� �߽ɺ��� �������� �ʰ� ����
        hitPosition.y = Mathf.Max(bounds.center.y, bounds.min.y + 0.5f);

        Debug.Log($"�浹 ��ġ (������): {hitPosition}");
        return hitPosition;
    }

    /// ���� �ִ� �뷫���� ������ ã�� �Լ� (Ÿ���� X, ���� ���� �������� �̵�)
    Vector3 FindGeneralEnemyDirection(Vector3 currentPosition)
    {
        float searchRadius = 10f; // Ž�� �ݰ�
        Collider[] enemies = Physics.OverlapSphere(currentPosition, searchRadius); // LayerMask ����

        Debug.Log("��ó �� ����: " + enemies.Length);

        if (enemies.Length == 0)
        {
            Debug.Log("���� ���� ����. źȯ�� ������� ��!");
            return Vector3.zero;
        }

        Vector3 generalDirection = Vector3.zero;
        int count = 0;

        foreach (Collider enemy in enemies)
        {
            if (!enemy.CompareTag("Enemy")) continue;
            Debug.Log("������ ��: " + enemy.name);

            if (Vector3.Distance(enemy.transform.position, currentPosition) < 0.1f) continue;

            generalDirection += (enemy.transform.position - currentPosition).normalized;
            count++;
        }

        if (count > 0)
        {
            generalDirection /= count;  // ���� �� ������ ����� ���
            generalDirection.y = 0; // Y�� �̵� ���� (���鿡 ������ �ʵ���)   
            generalDirection.Normalize();
            Debug.Log("���� �̵� ���� ����: " + generalDirection);
        }
        else
        {
            Debug.Log("���� �̵� ���� ����");
        }

        return generalDirection;
    }
}