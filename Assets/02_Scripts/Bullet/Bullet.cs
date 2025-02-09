using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 60f;           // 탄환 속도
    public int maxBounces = 1;          // 최대 튕길 횟수
    public int bounceCount = 0;        // 현재 튕긴 횟수

    public float damage;
    public int per;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // 데미지, 갯수, 속도
    public void Init(float dmg, int per ,Vector3 dir)
    {
        this.damage = dmg;
        this.per = per;

        bounceCount = 0;

        if (per > -1)
        {
            rigid.velocity = dir.normalized * speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // 적을 맞췄다면
        {
            Debug.Log("적과 충돌! 다음 이동 방향 찾기");
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Debug.Log("최대 튕김 횟수 도달! 탄환 삭제");
                gameObject.SetActive(false); // 기존 탄환 삭제
                return;
            }

            Vector3 contactPoint = GetContactPoint(other); // 충돌 지점 찾기
            Vector3 nextDirection = FindGeneralEnemyDirection(contactPoint);

            if (nextDirection == Vector3.zero)
            {
                Debug.Log("다음 이동 방향 없음. 탄환 삭제");
                gameObject.SetActive(false);
                return;
            }

            // 기존 탄환 삭제 후 새 탄환 생성
            gameObject.SetActive(false);
            Transform newBullet = GameManager.Instance.pool.Bullet_Get(0).transform;

            newBullet.position = contactPoint;  // 적과 충돌한 위치에서 생성
            newBullet.position = new Vector3(newBullet.position.x, contactPoint.y, newBullet.position.z); // Y값 강제 조정
            newBullet.rotation = Quaternion.LookRotation(nextDirection); // 방향 설정

            Rigidbody newRb = newBullet.GetComponent<Rigidbody>();
            if (newRb != null)
            {
                newRb.velocity = nextDirection * speed;
            }

            Debug.Log("새 탄환 생성! 방향: " + nextDirection);
        }
    }

    /// 적과 충돌한 위치를 찾는 함수 (적의 중심 높이에서 생성)
    Vector3 GetContactPoint(Collider enemy)
    {
        Bounds bounds = enemy.bounds;
        Vector3 hitPosition = bounds.center;  // 기본적으로 적의 중앙 사용

        // Y값을 적의 중심보다 낮아지지 않게 설정
        hitPosition.y = Mathf.Max(bounds.center.y, bounds.min.y + 0.5f);

        Debug.Log($"충돌 위치 (보정됨): {hitPosition}");
        return hitPosition;
    }

    /// 적이 있는 대략적인 방향을 찾는 함수 (타겟팅 X, 적이 많은 방향으로 이동)
    Vector3 FindGeneralEnemyDirection(Vector3 currentPosition)
    {
        float searchRadius = 10f; // 탐색 반경
        Collider[] enemies = Physics.OverlapSphere(currentPosition, searchRadius); // LayerMask 제거

        Debug.Log("근처 적 개수: " + enemies.Length);

        if (enemies.Length == 0)
        {
            Debug.Log("적이 전혀 없음. 탄환은 사라져야 함!");
            return Vector3.zero;
        }

        Vector3 generalDirection = Vector3.zero;
        int count = 0;

        foreach (Collider enemy in enemies)
        {
            if (!enemy.CompareTag("Enemy")) continue;
            Debug.Log("감지된 적: " + enemy.name);

            if (Vector3.Distance(enemy.transform.position, currentPosition) < 0.1f) continue;

            generalDirection += (enemy.transform.position - currentPosition).normalized;
            count++;
        }

        if (count > 0)
        {
            generalDirection /= count;  // 여러 적 방향의 평균을 계산
            generalDirection.y = 0; // Y축 이동 방지 (지면에 박히지 않도록)   
            generalDirection.Normalize();
            Debug.Log("다음 이동 방향 설정: " + generalDirection);
        }
        else
        {
            Debug.Log("다음 이동 방향 없음");
        }

        return generalDirection;
    }
}