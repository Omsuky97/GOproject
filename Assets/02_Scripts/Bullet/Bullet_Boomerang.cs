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
    public float baseColliderSize = 1f; // 초기 콜리전 크기 저장
    public float baseScale = 1f;      // 초기 크기 저장
    private float fixedY;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        target_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        fixedY = rigid.position.y; // 몬스터가 처음 위치한 Y 값 저장
        Boomerang_damage = Bullet.damage;
        bulletCollider = GetComponent<Collider>(); // 현재 불릿의 콜리전 가져오기
        baseColliderSize = GetColliderSize(bulletCollider); // 초기 콜리전 크기 저장
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
        // 대상까지의 방향 계산 (Y축 제외)
        Vector3 dirvec = target_rigid.position - rigid.position;
        dirvec.y = 0f; // Y축 고정

        // 이동 벡터 계산
        Vector3 nextVec = dirvec.normalized * speed * Time.fixedDeltaTime;

        // 몬스터를 대상 방향으로 회전
        if (dirvec != Vector3.zero)
        {
            // 대상 방향으로 회전 계산
            Quaternion targetRotation = Quaternion.LookRotation(dirvec);

            // 부드럽게 회전 (Y축만 회전)
            rigid.rotation = Quaternion.Slerp(
                rigid.rotation,
                targetRotation,
                Time.fixedDeltaTime * 10f
            );
        }

        // 현재 Y값을 유지한 채 이동 적용
        rigid.MovePosition(new Vector3(
            rigid.position.x + nextVec.x,
            rigid.position.y,  // Y축 고정
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
