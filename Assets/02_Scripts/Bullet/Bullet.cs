using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    private List<Collider> Hit_Bounce_Enemys = new List<Collider>(); // 이미 맞은 적 목록
    private HashSet<Collider> Hit_Split_Enemys = new HashSet<Collider>(); // 이미 맞은 몬스터 저장
    private float hitCooldown = 0.5f; // 같은 적을 다시 맞지 않도록 하는 시간(초)
    private Rigidbody rigid;
    public Player_Scaner player; // 플레이어 위치 참조
    private ParticleSystem bulletParticles; // 파티클 시스템 참조
    public GameObject Bullet_Boomerang_Prefab;

    [Header("## -- Bullet -- ##")]
    public float speed = 5;           // 탄환 속도
    public float damage;
    public int Bullet_per;
    public Vector3 Bullet_dir;



    [Header("## -- Bullet_Bounce -- ##")]
    public int maxBounces = 5;          // 최대 튕길 횟수
    public int bounceCount = 0;         // 현재 튕긴 횟수
    public float Bullet_Bounce_Spawn_Offset = 1.0f;    //충돌 위치에서 이동할 거리

    [Header("## -- Bullet_Penetrate -- ##")]
    public int max_penetration = 5;
    public int penetration = 0;

    [Header("## -- Bullet_Propulsion -- ##")]
    public float Origin_Spped;
    public float Propulsion_Speed;

    [Header("## -- BulletBoom -- ##")]
    public GameObject Bullet_Boom;

    [Header("## -- Bullet_Spirt -- ##")]
    public GameObject Bullet_Spirt;
    public int Bullet_Spirt_Count;
    public float Bullet_Spirt_Offset = 0.5f;

    [Header("## -- Bullet_Bool_Type -- ##")]
    public bool Bullet_bounce_Type;             //총알튕김
    public bool BUllet_penetrate_Type;          //총알관통
    public bool Bullet_NucBack_Type;            //총알넉백
    public bool Bullet_Boomerang_Type;          //부메랑
    public bool Bullet_Propulsion_Type;         //총알추진
    public bool Bullet_Boom_Type;               //총알폭발
    public bool Bullet_Spirt_Type;              //총알분열

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Origin_Spped = speed;
    }
    private void LateUpdate()
    {
        if (Bullet_Propulsion_Type)
        {
            Propulsion_Speed = Propulsion_Speed + 1;
            rigid.velocity = Bullet_dir.normalized * Propulsion_Speed;
        }
    }
    // 데미지, 갯수, 속도
    public void Init(float dmg, int per, Vector3 dir)
    {
        Bullet_Boom.gameObject.SetActive(false);
        damage = dmg;
        Bullet_per = per;
        Bullet_dir = dir;
        // 탄환이 새롭게 활성화될 때 파티클 재생
        if (bulletParticles != null) bulletParticles.Play();
        bounceCount = 0;
        penetration = 0;
        Propulsion_Speed = Origin_Spped;
        rigid.velocity = Bullet_dir.normalized * Origin_Spped;
    }
    private void OnEnable()
    {
        ResetChildRotation(); // 총알이 활성화될 때 하위 오브젝트 회전 초기화
    }

    private void ResetChildRotation()
    {
        foreach (Transform child in transform)
        {
            Vector3 childEuler = child.localEulerAngles;
            child.localEulerAngles = new Vector3(-90, childEuler.y, childEuler.z); // X축을 0으로 초기화
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // 적을 맞췄다면
        {
            //넉백 여부는 몬스터에서 설정
            //원거리 여부는 플레이어 스캔에서 설정
            //폭발 여부는 몬스터랑 불릿 동시에 켜줄것
            //탄환의 공격력에 따라 크기 및 넉백 설정
            //정령은 베지에 곡선이라는 것을 활용할 것
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            if (Bullet_Boom_Type) Bullet_Boom.gameObject.SetActive(true);
            if (Bullet_Spirt_Type)
            {
                Hit_Split_Enemys.Add(other);
                Bullet_Split(contactPoint, Hit_Split_Enemys, other);
            }
            if (Bullet_bounce_Type)
            {
                if (Hit_Bounce_Enemys.Any(e => e.gameObject == other.gameObject)) return;

                Hit_Bounce_Enemys.Add(other);
                Bullet_bounce(other);
            }
            if (BUllet_penetrate_Type) BUllet_penetrate();
            if (Bullet_Boomerang_Type) Bullet_Boomerang(other);
            if(!Bullet_bounce_Type && !Bullet_Boom_Type) gameObject.SetActive(false);
        }
    }
    # region Bullet_Split
    private void Bullet_Split(Vector3 contactPoint, HashSet<Collider> parentHitMonsters, Collider other)
    {
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3초 후 자동 삭제
        }

        Vector3 originalDirection = Vector3.forward;
        Bullet_Spirt_Count = Mathf.Clamp(Bullet_Spirt_Count, 1, 5);
        float angleStep = 360f / Bullet_Spirt_Count;
        float offsetAngle = (Bullet_Spirt_Count % 2 == 0) ? angleStep / 2 : 0; // 짝수 개수일 경우 중심 맞춤
        for (int i = 0; i < Bullet_Spirt_Count; i++)
        {
            float angleOffset = (i * angleStep) + offsetAngle;
            Vector3 nextDirection = Quaternion.Euler(0, angleOffset, 0) * originalDirection;
            Vector3 spawnPosition = contactPoint + (nextDirection * Bullet_Spirt_Offset);
            GameObject newBullet = Instantiate(Bullet_Spirt, spawnPosition, Quaternion.LookRotation(nextDirection));

            newBullet.SetActive(true);
            Bullet_Split newBulletScript = newBullet.GetComponent<Bullet_Split>();
            if (newBulletScript != null) newBulletScript.Inherit_Hit_Monsters(parentHitMonsters);


            Rigidbody newRb = newBullet.GetComponent<Rigidbody>();
            if (newRb != null) newRb.velocity = nextDirection * speed;
            Destroy(newBullet, 3f);
        }
    }
    private IEnumerator Hit_List_Split_Remove(Collider enemy)
    {
        yield return new WaitForSeconds(hitCooldown);
        Hit_Split_Enemys.Remove(enemy);
    }
    #endregion
    #region BUllet_penetrate
    private void BUllet_penetrate()
    {
        if (penetration >= max_penetration) gameObject.SetActive(false);
        else penetration += 1;
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3초 후 자동 삭제
        }

    }
    #endregion
    #region Bullet_bounce
    private void Bullet_bounce(Collider other)
    {
        // 최대 튕김 횟수를 초과하면 총알 비활성화
        if (bounceCount >= maxBounces)
        {
            gameObject.SetActive(false);
            return;
        }

        // 충돌 지점 및 새로운 방향 찾기
        Vector3 contactPoint = GetContactPoint(other);
        Vector3 nextDirection = FindGeneralEnemyDirection(contactPoint);

        // 튕길 방향이 없으면 비활성화
        if (nextDirection == Vector3.zero)
        {
            gameObject.SetActive(false);
            return;
        }
        // 기존 총알의 위치 및 방향 변경 (부모는 그대로)
        transform.position = contactPoint + (nextDirection.normalized * Bullet_Spirt_Offset);
        transform.rotation = Quaternion.LookRotation(nextDirection);

        // **하위 오브젝트들의 X축 회전을 90도로 고정**
        foreach (Transform child in transform)
        {
            Vector3 childEuler = child.localEulerAngles;
            child.localEulerAngles = new Vector3(0, childEuler.y, childEuler.z);
        }

        // 기존 총알의 Rigidbody 속도를 업데이트하여 튕기게 함
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = nextDirection * speed;
        }

        bounceCount += 1;
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3초 후 자동 삭제
        }
    }
    private Vector3 GetContactPoint(Collider other)
    {
        return other.ClosestPoint(transform.position);
    }
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
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    #endregion
    #region Bullet_Boomerang
    private void Bullet_Boomerang(Collider other)
    {
        Vector3 contactPoint = GetContactPoint(other);
        // 총알을 생성할 위치 계산
        Vector3 spawnPosition = contactPoint - other.transform.forward * 3f;
        // 총알 생성
        Instantiate(Bullet_Boomerang_Prefab, spawnPosition, Quaternion.identity);
    }
    #endregion
}