using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Split : MonoBehaviour
{
    private HashSet<Collider> Hit_Split_Enemys = new HashSet<Collider>();

    public float rotateSpeed = 5f;   // ȸ�� �ӵ�
    public float Split_damage;
    public Bullet Bullet;
    private Rigidbody rb;

    private void Start()
    {
        Split_damage = Bullet.damage;
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (Player_Scaner.nearestTarget == null) return; // Ÿ���� ������ ���� �� ��

        Vector3 targetPosition = Player_Scaner.nearestTarget.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        Vector3 newDirection = Vector3.Lerp(rb.velocity.normalized, direction, rotateSpeed * Time.fixedDeltaTime).normalized;
        newDirection.y = 0;

        // �ӵ� ����
        rb.velocity = newDirection * GameManager.Instance.Bullet_Speed;
    }
    public void Inherit_Hit_Monsters(HashSet<Collider> parentHitMonsters)
    {
        Hit_Split_Enemys = new HashSet<Collider>(parentHitMonsters); // �θ� źȯ�� ���� ���� ������ ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Hit_Split_Enemys.Contains(other)) return; //�̹� ���� ���Ͷ�� ����
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
