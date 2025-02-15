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
        // 대상까지의 방향 계산 (Y축 제외)
        Vector3 dirvec = target_rigid.position - rigid.position;
        dirvec.y = -3f; // 수직 방향 제거
        // 이동 벡터 계산
        Vector3 nextVec = dirvec.normalized * speed * Time.fixedDeltaTime;
        // 몬스터를 대상 방향으로 회전
        if (dirvec != Vector3.zero)
        {
            dirvec.y = 0f; // 수직 방향 제거
            // 대상 방향으로 회전 계산
            Quaternion targetRotation = Quaternion.LookRotation(dirvec);

            // 부드럽게 회전 (Y축만 회전)
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
