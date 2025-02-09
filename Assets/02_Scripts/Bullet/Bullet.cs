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

            Vector3 nextDirection = FindGeneralEnemyDirection(other.transform.position);

            if (nextDirection == Vector3.zero)
            {
                Debug.Log("���� �̵� ���� ����. źȯ ����");
                gameObject.SetActive(false);
                return;
            }

            // ���� źȯ ���� �� �� źȯ ����
            gameObject.SetActive(false);
            Transform newBullet = GameManager.Instance.pool.Bullet_Get(0).transform;

            newBullet.position = other.transform.position;  // ���� ��ġ���� ����
            newBullet.rotation = Quaternion.LookRotation(nextDirection); // ���� ����

            Rigidbody newRb = newBullet.GetComponent<Rigidbody>();
            if (newRb != null)
            {
                newRb.velocity = nextDirection * speed;
            }

            Debug.Log("�� źȯ ����! ����: " + nextDirection);
        }
    }

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