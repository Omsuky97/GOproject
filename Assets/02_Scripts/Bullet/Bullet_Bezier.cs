using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Bezier : MonoBehaviour
{
    [Header("## -- Bullet_Bezier -- ##")]
    public float Bezier_damage;
    public Bullet Bullet;
    public GameObject Fire_Point;

    public Transform objectTransform;
    public Collider objectCollider;
    private Collider bulletCollider;
    public float baseColliderSize = 1f; // 초기 콜리전 크기 저장
    public float baseScale = 1f;      // 초기 크기 저장

    private void Start()
    {
        bulletCollider = GetComponent<Collider>(); // 현재 불릿의 콜리전 가져오기
        baseColliderSize = GetColliderSize(bulletCollider); // 초기 콜리전 크기 저장
    }
    private void OnEnable()
    {
        IncreaseSizeBasedOnAttack(GameManager.Instance.bullet_damage);
        gameObject.transform.position = Fire_Point.transform.position;
        bulletCollider = GetComponent<Collider>(); // 현재 불릿의 콜리전 가져오기
        baseColliderSize = GetColliderSize(bulletCollider); // 초기 콜리전 크기 저장
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) gameObject.SetActive(false);
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
