using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ShotGun : MonoBehaviour
{
    [Header("## -- Bullet_ShortGun -- ##")]
    public float ShortGun_damage;
    public Bullet Bullet;
    public GameObject Fire_Point;
    public static Vector3 Bullet_Target;
    private Rigidbody rigid;
    public float rotateSpeed = 5f;   // ȸ�� �ӵ�

    public Transform objectTransform;
    public Collider objectCollider;
    private Collider bulletCollider;
    public float baseColliderSize = 1f; // �ʱ� �ݸ��� ũ�� ����
    public float baseScale = 1f;      // �ʱ� ũ�� ����
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>(); // ���� �Ҹ��� �ݸ��� ��������
        baseColliderSize = GetColliderSize(bulletCollider); // �ʱ� �ݸ��� ũ�� ����
        IncreaseSizeBasedOnAttack(GameManager.Instance.bullet_damage);
    }
    private void LateUpdate()
    {
        if (Bullet_Manager.Instance.Bullet_Guided_Type)
        {
            if (Bullet_Target == new Vector3(0, 0, 0)) Bullet_Target = Player_Scaner.nearestTarget.position;
            Vector3 targetPosition = Bullet_Target;
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 newDirection = Vector3.Lerp(rigid.velocity.normalized, direction, rotateSpeed * Time.fixedDeltaTime).normalized;

            rigid.velocity = newDirection * Bullet_Manager.Instance.Bullet_Speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) gameObject.SetActive(false);
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
