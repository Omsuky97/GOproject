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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        target_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Boomerang_damage = Bullet.damage;
    }
    private void FixedUpdate()
    {
        Target_Move_Rotator();
    }
    void Target_Move_Rotator()
    {
        // �������� ���� ��� (Y�� ����)
        Vector3 dirvec = target_rigid.position - rigid.position;
        dirvec.y = -3f; // ���� ���� ����
        // �̵� ���� ���
        Vector3 nextVec = dirvec.normalized * speed * Time.fixedDeltaTime;
        // ���͸� ��� �������� ȸ��
        if (dirvec != Vector3.zero)
        {
            dirvec.y = 0f; // ���� ���� ����
            // ��� �������� ȸ�� ���
            Quaternion targetRotation = Quaternion.LookRotation(dirvec);

            // �ε巴�� ȸ�� (Y�ุ ȸ��)
            rigid.rotation = Quaternion.Slerp(
                rigid.rotation,
                targetRotation,
                Time.fixedDeltaTime * 10f
            );
        }
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "pl")
        {
            Destroy(gameObject);
        }
    }
}
