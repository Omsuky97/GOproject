using CW.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    public static List<Collider> Hit_Bounce_Enemys = new List<Collider>(); // 이미 맞은 적 목록
    public HashSet<Collider> Hit_Split_Enemys = new HashSet<Collider>(); // 이미 맞은 몬스터 저장
    public List<Transform> enemyList = new List<Transform>(); // 현재 반사 대상 리스트
    public int enemyIndex = 0; // 리스트에서 현재 타겟 인덱스
    private Rigidbody rigid;
    public Player_Scaner player; // 플레이어 위치 참조
    private ParticleSystem bulletParticles; // 파티클 시스템 참조
    public GameObject Bullet_Boomerang_Prefab;
    public float damage;
    public Vector3 Bullet_dir;
    public float rotateSpeed = 5f;   // 회전 속도

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
    public bool Bullet_Guided_Type;             //총알 유도

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Origin_Spped = GameManager.Instance.Bullet_Speed;

    }
    private void Start()
    {
        rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
    private void LateUpdate()
    {
        if (Bullet_Propulsion_Type)
        {
            Propulsion_Speed = Propulsion_Speed + 1;
            rigid.velocity = Bullet_dir.normalized * Propulsion_Speed;
        }
        if (rigid.velocity.magnitude < GameManager.Instance.Bullet_Speed * 0.9f)
        {
            rigid.velocity = rigid.velocity.normalized * GameManager.Instance.Bullet_Speed;
        }
    }
    // 데미지, 갯수, 속도
    public void Init(float dmg, Vector3 dir)
    {
        Bullet_Boom.gameObject.SetActive(false);
        damage = dmg;
        Bullet_dir = dir;
        // 탄환이 새롭게 활성화될 때 파티클 재생
        if (bulletParticles != null) bulletParticles.Play();
        bounceCount = 0;
        Hit_Bounce_Enemys.Clear();
        penetration = 0;
        enemyList.Clear();
        enemyIndex = 0;
        Propulsion_Speed = Origin_Spped;
        rigid.velocity = Bullet_dir.normalized * Origin_Spped;
    }
    private void OnEnable()
    {
        // 적과 충돌했으므로, 맞은 적 리스트에 추가
        Hit_Bounce_Enemys.Clear();
        penetration = 0;
        bounceCount = 0;
        enemyList.Clear();
        enemyIndex = 0;
        ResetChildRotation(); // 총알이 활성화될 때 하위 오브젝트 회전 초기화
    }
    private void OnDisable()
    {
        Hit_Bounce_Enemys.Clear();
        enemyList.Clear();
        bounceCount = 0;
        enemyIndex = 0;
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

                if (Bullet_Guided_Type) Bullet_bounce_Guided(other);
                else if (!Bullet_Guided_Type) Bullet_bounce(other);
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
            if (newRb != null) newRb.velocity = nextDirection * GameManager.Instance.Bullet_Speed;
            Destroy(newBullet, 3f);
        }
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
        if (bounceCount >= maxBounces)
        {
            gameObject.SetActive(false);
            return;
        }

        if (Hit_Bounce_Enemys.Contains(other)) return; // 이미 맞았으면 더 이상 진행하지 않음
        Hit_Bounce_Enemys.Add(other);

        Vector3 contactPoint = GetContactPoint(other);
        Vector3 nextDirection = FindGeneralEnemyDirection(contactPoint);

        if (nextDirection == Vector3.zero)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = contactPoint + (nextDirection.normalized * Bullet_Spirt_Offset);
        transform.rotation = Quaternion.LookRotation(nextDirection);
        foreach (Transform child in transform)
        {
            Vector3 childEuler = child.localEulerAngles;
            child.localEulerAngles = new Vector3(0, childEuler.y, childEuler.z);
        }
        rigid.velocity = nextDirection * GameManager.Instance.Bullet_Speed;
        bounceCount += 1;

        Hit_Bounce_Enemys.Remove(other);

        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3초 후 자동 삭제
        }
    }
    private void Bullet_bounce_Guided(Collider other)
    {
        if (bounceCount >= maxBounces)
        {
            gameObject.SetActive(false);
            return;
        }

        bounceCount += 1;

        if (!Hit_Bounce_Enemys.Contains(other))
        {
            Hit_Bounce_Enemys.Add(other);
        }

        Vector3 contactPoint = GetContactPoint(other);

        Transform nearestEnemy = GetNextEnemy();

        if (nearestEnemy == null)
        {
            enemyIndex = 0;
            nearestEnemy = GetNextEnemy();
        }

        if (nearestEnemy == null)
        {
            ScanEnemies(contactPoint, 30f);
            nearestEnemy = GetNextEnemy();
        }

        if (nearestEnemy == null)
        {
            gameObject.SetActive(false);
            return;
        }
        Vector3 nextDirection = (nearestEnemy.position - transform.position).normalized;
        float fixedSpeed = GameManager.Instance.Bullet_Speed; // 속도 고정
        rigid.velocity = nextDirection * fixedSpeed;
        transform.rotation = Quaternion.LookRotation(nextDirection);

        enemyIndex++;

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f));
        }
    }
    private Transform GetNextEnemy()
    {
        if (enemyList.Count == 0)
        {
            ScanEnemies(transform.position, 30f);
            enemyIndex = 0;
        }

        if (enemyIndex >= enemyList.Count)
        {
            enemyIndex = 0;
        }
        while (enemyIndex < enemyList.Count)
        {
            Transform enemy = enemyList[enemyIndex];
            enemyIndex++;

            if (enemy != null && !Hit_Bounce_Enemys.Contains(enemy.GetComponent<Collider>()))
            {
                return enemy;
            }
        }

        return null;
    }
    private void ScanEnemies(Vector3 position, float range)
    {
        enemyList.Clear();
        enemyIndex = 0;

        Collider[] hitColliders = new Collider[20]; // 최대 감지 수 증가
        int numColliders = Physics.OverlapSphereNonAlloc(position, range, hitColliders);

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag("Enemy") && !Hit_Bounce_Enemys.Contains(hitColliders[i]) && hitColliders[i].gameObject.activeSelf)
            {
                enemyList.Add(hitColliders[i].transform);
            }
        }
    }
    private Vector3 GetContactPoint(Collider other)
    {
        return other.bounds.center;  // 정확도를 높이기 위해 중심점을 사용
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