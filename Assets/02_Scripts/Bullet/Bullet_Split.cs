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

    public Transform objectTransform;
    public Collider objectCollider;
    private Collider bulletCollider;
    public float baseColliderSize = 1f; // �ʱ� �ݸ��� ũ�� ����
    public float baseScale = 1f;      // �ʱ� ũ�� ����

    private void Start()
    {
        Split_damage = Bullet.damage;
        rb = GetComponent<Rigidbody>();

        bulletCollider = GetComponent<Collider>(); // ���� �Ҹ��� �ݸ��� ��������
        baseColliderSize = GetColliderSize(bulletCollider); // �ʱ� �ݸ��� ũ�� ����
        IncreaseSizeBasedOnAttack(GameManager.Instance.bullet_damage);

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(Bullet_Manager.Instance.Bullet_Active_false));
        }
    }
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    private void LateUpdate()
    {
        if (Bullet_Manager.Instance.Bullet_Guided_Type)
        {
            if (Player_Scaner.nearestTarget == null) return; // Ÿ���� ������ ���� �� ��

            Vector3 targetPosition = Player_Scaner.nearestTarget.position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            Vector3 newDirection = Vector3.Lerp(rb.velocity.normalized, direction, rotateSpeed * Time.fixedDeltaTime).normalized;
            newDirection.y = 0;

            rb.velocity = newDirection * Bullet_Manager.Instance.Bullet_Speed;
        }
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
    // ���ݷ��� �����ϸ� �Ҹ� ũ�� & �ݸ��� ũ�� ����
    public void IncreaseSizeBasedOnAttack(float attackPower)
    {
        float scaleMultiplier = 1f + (attackPower / 50f) * 0.25f; // 50 ������ ������ 25% ����
        transform.localScale = Vector3.one * (baseScale * scaleMultiplier);

        // �ݸ��� ũ�� 2�� ����
        float newColliderSize = baseColliderSize * 2f;
        SetColliderSize(bulletCollider, newColliderSize);
    }
    // �ݸ��� ũ�� �������� (Collider Ÿ�Կ� ���� ũ�� ��ȯ)
    private float GetColliderSize(Collider col)
    {
        if (col is BoxCollider boxCol)
        {
            return boxCol.size.x; // �ڽ� �ݸ��� ũ�� ��ȯ
        }
        else if (col is SphereCollider sphereCol)
        {
            return sphereCol.radius; // ��ü �ݸ��� ũ�� ��ȯ
        }
        else if (col is CapsuleCollider capsuleCol)
        {
            return capsuleCol.height; // ĸ�� �ݸ��� ũ�� ��ȯ
        }
        return 1f; // �⺻��
    }

    // �ݸ��� ũ�� ���� (Collider Ÿ�Կ� ���� ũ�� ����)
    private void SetColliderSize(Collider col, float newSize)
    {
        if (col is BoxCollider boxCol)
        {
            boxCol.size = Vector3.one * newSize; // �ڽ� �ݸ��� ũ�� ����
        }
        else if (col is SphereCollider sphereCol)
        {
            sphereCol.radius = newSize; // ��ü �ݸ��� ũ�� ����
        }
        else if (col is CapsuleCollider capsuleCol)
        {
            capsuleCol.height = newSize; // ĸ�� �ݸ��� ũ�� ����
        }
    }
}
