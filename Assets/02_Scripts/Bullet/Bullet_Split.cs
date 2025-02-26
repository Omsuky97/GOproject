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

    public Transform objectTransform;
    public Collider objectCollider;
    private Collider bulletCollider;
    public float baseColliderSize = 1f; // 초기 콜리전 크기 저장
    public float baseScale = 1f;      // 초기 크기 저장

    private void Start()
    {
        Split_damage = Bullet.damage;
        rb = GetComponent<Rigidbody>();

        bulletCollider = GetComponent<Collider>(); // 현재 불릿의 콜리전 가져오기
        baseColliderSize = GetColliderSize(bulletCollider); // 초기 콜리전 크기 저장
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
            if (Player_Scaner.nearestTarget == null) return; // 타겟이 없으면 실행 안 함

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
    // 공격력이 증가하면 불릿 크기 & 콜리전 크기 증가
    public void IncreaseSizeBasedOnAttack(float attackPower)
    {
        float scaleMultiplier = 1f + (attackPower / 50f) * 0.25f; // 50 증가할 때마다 25% 증가
        transform.localScale = Vector3.one * (baseScale * scaleMultiplier);

        // 콜리전 크기 2배 증가
        float newColliderSize = baseColliderSize * 2f;
        SetColliderSize(bulletCollider, newColliderSize);
    }
    // 콜리전 크기 가져오기 (Collider 타입에 따라 크기 반환)
    private float GetColliderSize(Collider col)
    {
        if (col is BoxCollider boxCol)
        {
            return boxCol.size.x; // 박스 콜리전 크기 반환
        }
        else if (col is SphereCollider sphereCol)
        {
            return sphereCol.radius; // 구체 콜리전 크기 반환
        }
        else if (col is CapsuleCollider capsuleCol)
        {
            return capsuleCol.height; // 캡슐 콜리전 크기 반환
        }
        return 1f; // 기본값
    }

    // 콜리전 크기 설정 (Collider 타입에 따라 크기 변경)
    private void SetColliderSize(Collider col, float newSize)
    {
        if (col is BoxCollider boxCol)
        {
            boxCol.size = Vector3.one * newSize; // 박스 콜리전 크기 설정
        }
        else if (col is SphereCollider sphereCol)
        {
            sphereCol.radius = newSize; // 구체 콜리전 크기 설정
        }
        else if (col is CapsuleCollider capsuleCol)
        {
            capsuleCol.height = newSize; // 캡슐 콜리전 크기 설정
        }
    }
}
