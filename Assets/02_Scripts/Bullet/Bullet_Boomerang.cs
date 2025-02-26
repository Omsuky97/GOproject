using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet_Boomerang : MonoBehaviour
{
    public float speed = 10f;
    public float Boomerang_damage;
    public Bullet Bullet;
    public Transform target;
    private Rigidbody rigid;
    public Rigidbody target_rigid;

    public Transform objectTransform;
    public Collider objectCollider;
    private Collider bulletCollider;
    public float baseColliderSize = 1f; // �ʱ� �ݸ��� ũ�� ����
    public float baseScale = 1f;      // �ʱ� ũ�� ����

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        target_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Boomerang_damage = Bullet.damage;
        bulletCollider = GetComponent<Collider>(); // ���� �Ҹ��� �ݸ��� ��������
        baseColliderSize = GetColliderSize(bulletCollider); // �ʱ� �ݸ��� ũ�� ����
        IncreaseSizeBasedOnAttack(GameManager.Instance.bullet_damage);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(Bullet_Manager.Instance.Bullet_Active_false));
        }
    }
    private void FixedUpdate()
    {
        Target_Move_Rotator();
    }
    void Target_Move_Rotator()
    {
        // �������� ���� ��� (Y�� ����)
        Vector3 dirvec = target_rigid.position - rigid.position;
        dirvec.y = 0f; // Y�� ����

        // �̵� ���� ���
        Vector3 nextVec = dirvec.normalized * speed * Time.fixedDeltaTime;

        // ���͸� ��� �������� ȸ��
        if (dirvec != Vector3.zero)
        {
            // ��� �������� ȸ�� ���
            Quaternion targetRotation = Quaternion.LookRotation(dirvec);

            // �ε巴�� ȸ�� (Y�ุ ȸ��)
            rigid.rotation = Quaternion.Slerp(
                rigid.rotation,
                targetRotation,
                Time.fixedDeltaTime * 10f
            );
        }

        // ���� Y���� ������ ä �̵� ����
        rigid.MovePosition(new Vector3(
            rigid.position.x + nextVec.x,
            rigid.position.y,  // Y�� ����
            rigid.position.z + nextVec.z
        ));

        rigid.velocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player")) gameObject.SetActive(false);
    }
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
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
