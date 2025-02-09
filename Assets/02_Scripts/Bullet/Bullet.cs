using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 60f;           // 탄환 속도

    public int maxBounces = 5;          // 최대 튕길 횟수
    public int bounceCount = 0;        // 현재 튕긴 횟수

    public int max_penetration = 5;
    public int penetration = 0;

    public float damage;
    public int per;
    Rigidbody rigid;
    private ParticleSystem bulletParticles; // 파티클 시스템 참조

    private List<Collider> hitEnemies = new List<Collider>(); // 이미 맞은 적 목록
    private float hitCooldown = 0.5f; // 같은 적을 다시 맞지 않도록 하는 시간(초)

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
        penetration = 0;

        if (per > -1)
        {
            rigid.velocity = dir.normalized * speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // 적을 맞췄다면
        {
            // 같은 적을 연속해서 맞는지 확인
            if (hitEnemies.Contains(other)) return;

            // 적을 맞은 목록에 추가
            hitEnemies.Add(other);
            StartCoroutine(RemoveFromHitListAfterDelay(other)); // 일정 시간 후 제거

            Vector3 contactPoint = GetContactPoint(other); // 충돌 지점 찾기
            Vector3 nextDirection = FindGeneralEnemyDirection(contactPoint);

            if (nextDirection == Vector3.zero)
            {
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

            bounceCount += 1;
            if (penetration >= max_penetration)
            {
                gameObject.SetActive(false);
            }
            penetration += 1;
            StartCoroutine(DestroyAfterDelay(3f)); // 3초 후 자동 삭제
        }
    }

    /// 일정 시간이 지난 후 hitEnemies에서 제거하는 함수
    private IEnumerator RemoveFromHitListAfterDelay(Collider enemy)
    {
        yield return new WaitForSeconds(hitCooldown);
        hitEnemies.Remove(enemy);
    }

    /// 적과 충돌한 위치를 찾는 함수
    Vector3 GetContactPoint(Collider enemy)
    {
        Bounds bounds = enemy.bounds;
        Vector3 hitPosition = bounds.center;  // 기본적으로 적의 중앙 사용
        hitPosition.y = Mathf.Max(bounds.center.y, bounds.min.y + 0.5f); // 너무 낮은 곳에서 나오지 않도록 보정
        return hitPosition;
    }

    /// 3초 후 탄환을 비활성화하는 함수
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableBullet();
    }

    /// 탄환 비활성화 함수 (파티클은 유지)
    private void DisableBullet()
    {
        // 탄환이 비활성화될 때 화면 밖으로 이동 (잔상 방지)
        transform.position = Vector3.one * 1000f;

        // SetActive(false)로 풀링 시스템과 연동
        gameObject.SetActive(false);
    }

    /// 적이 있는 방향을 찾는 함수
    Vector3 FindGeneralEnemyDirection(Vector3 currentPosition)
    {
        float searchRadius = 10f; // 탐색 반경
        Collider[] enemies = Physics.OverlapSphere(currentPosition, searchRadius);

        if (enemies.Length == 0) return Vector3.zero;

        Vector3 generalDirection = Vector3.zero;
        int count = 0;

        foreach (Collider enemy in enemies)
        {
            if (!enemy.CompareTag("Enemy")) continue;
            if (Vector3.Distance(enemy.transform.position, currentPosition) < 0.1f) continue;

            generalDirection += (enemy.transform.position - currentPosition).normalized;
            count++;
        }

        if (count > 0)
        {
            generalDirection /= count;
            generalDirection.y = 0;
            generalDirection.Normalize();
        }

        return generalDirection;
    }
}