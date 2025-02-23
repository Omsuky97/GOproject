using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Boom : MonoBehaviour
{
    public float boom_damage;
    public Bullet Bullet;

    public int segments = 32; // ���� �ػ�
    private LineRenderer lineRenderer;
    private SphereCollider sphereCollider;

    public Transform objectTransform;
    public Collider objectCollider;
    private Collider bulletCollider;
    public float baseColliderSize = 1f; // �ʱ� �ݸ��� ũ�� ����
    public float baseScale = 1f;      // �ʱ� ũ�� ����

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        bulletCollider = GetComponent<Collider>(); // ���� �Ҹ��� �ݸ��� ��������
        baseColliderSize = GetColliderSize(bulletCollider); // �ʱ� �ݸ��� ũ�� ����

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        boom_damage =  Bullet.damage;
        IncreaseSizeBasedOnAttack(GameManager.Instance.bullet_damage);
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
