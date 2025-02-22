using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Split : MonoBehaviour
{
    private HashSet<Collider> Hit_Split_Enemys = new HashSet<Collider>();

    public float rotateSpeed = 5f;   // 회전 속도
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
        if (Player_Scaner.nearestTarget == null) return; // 타겟이 없으면 실행 안 함

        Vector3 targetPosition = Player_Scaner.nearestTarget.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        Vector3 newDirection = Vector3.Lerp(rb.velocity.normalized, direction, rotateSpeed * Time.fixedDeltaTime).normalized;
        newDirection.y = 0;

        rb.velocity = newDirection * GameManager.Instance.Bullet_Speed;
    }
    public void Inherit_Hit_Monsters(HashSet<Collider> parentHitMonsters)
    {
        Hit_Split_Enemys = new HashSet<Collider>(parentHitMonsters); // 부모 탄환의 맞은 몬스터 정보를 복사
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Hit_Split_Enemys.Contains(other)) return; //이미 맞은 몬스터라면 무시
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
