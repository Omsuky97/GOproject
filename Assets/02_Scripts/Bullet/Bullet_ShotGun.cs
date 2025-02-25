using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ShotGun : MonoBehaviour
{
    public static Bullet_ShotGun Instance;

    public HashSet<Collider> Hit_Split_Enemys = new HashSet<Collider>(); // 이미 맞은 몬스터 저장
    public static List<Collider> Hit_Bounce_Enemys = new List<Collider>(); // 이미 맞은 적 목록
    public int enemyIndex = 0; // 리스트에서 현재 타겟 인덱스
    public List<Transform> enemyList = new List<Transform>(); // 현재 반사 대상 리스트
    [Header("## -- Bullet_ShortGun -- ##")]
    public float ShortGun_damage;
    public Bullet Bullet;
    public GameObject Fire_Point;
    public static Vector3 Bullet_Target;
    private Rigidbody rigid;
    public float rotateSpeed = 5f;   // 회전 속도
    public Vector3 Bullet_dir;
    public float damage;

    [Header("## -- Size -- ##")]
    public Transform objectTransform;
    public Collider objectCollider;
    private Collider bulletCollider;
    public float baseColliderSize = 1f; // 초기 콜리전 크기 저장
    public float baseScale = 1f;      // 초기 크기 저장

    [Header("## -- BulletBoom -- ##")]
    public GameObject Bullet_Boom;

    public int maxBounces = 5;          // 최대 튕길 횟수
    public int bounceCount = 0;         // 현재 튕긴 횟수
    public float Bullet_Bounce_Spawn_Offset = 1.0f;    //충돌 위치에서 이동할 거리

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>(); // 현재 불릿의 콜리전 가져오기
        baseColliderSize = GetColliderSize(bulletCollider); // 초기 콜리전 크기 저장
        IncreaseSizeBasedOnAttack(GameManager.Instance.bullet_damage);

        bounceCount = 0;
        Hit_Bounce_Enemys.Clear();
        Bullet_Manager.Instance.penetration = 0;
        enemyList.Clear();
        enemyIndex = 0;
        Bullet_Manager.Instance.Propulsion_Speed = Bullet_Manager.Instance.Origin_Spped;
    }
    private void OnEnable()
    {
        // 적과 충돌했으므로, 맞은 적 리스트에 추가
        Hit_Bounce_Enemys.Clear();
        Bullet_Manager.Instance.penetration = 0;
        bounceCount = 0;
        enemyList.Clear();
        enemyIndex = 0;
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
    private void OnDisable()
    {
        Hit_Bounce_Enemys.Clear();
        enemyList.Clear();
        bounceCount = 0;
        enemyIndex = 0;
    }
    private void LateUpdate()
    {
        if (Bullet_Manager.Instance.Bullet_Propulsion_Type)
        {
            Bullet_Manager.Instance.Propulsion_Speed = Bullet_Manager.Instance.Propulsion_Speed + 1;
            rigid.velocity = Bullet_dir.normalized * Bullet_Manager.Instance.Propulsion_Speed;
        }
        if (rigid.velocity.magnitude < Bullet_Manager.Instance.Bullet_Speed * 0.9f) rigid.velocity = rigid.velocity.normalized * Bullet_Manager.Instance.Bullet_Speed;
        if (Bullet_Manager.Instance.Bullet_Guided_Type)
        {
            if (Bullet_Target == new Vector3(0, 0, 0)) Bullet_Target = Player_Scaner.nearestTarget.position;
            Vector3 targetPosition = Bullet_Target;
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 newDirection = Vector3.Lerp(rigid.velocity.normalized, direction, rotateSpeed * Time.fixedDeltaTime).normalized;

            rigid.velocity = newDirection * Bullet_Manager.Instance.Bullet_Speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // 적을 맞췄다면
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            if (Bullet_Manager.Instance.Bullet_Boom_Type) Bullet_Boom.gameObject.SetActive(true);
            if (Bullet_Manager.Instance.Bullet_Spirt_Type)
            {
                Hit_Split_Enemys.Add(other);
                Bullet_Split(contactPoint, Hit_Split_Enemys, other);
            }
            if (Bullet_Manager.Instance.Bullet_bounce_Type)
            {

                if (Bullet_Manager.Instance.Bullet_Guided_Type) Bullet_bounce_Guided(other);
                else if (!Bullet_Manager.Instance.Bullet_Guided_Type) Bullet_bounce(other);
            }
            if (Bullet_Manager.Instance.BUllet_penetrate_Type) BUllet_penetrate();
            if (Bullet_Manager.Instance.Bullet_Boomerang_Type) Bullet_Boomerang(other);
            if (!Bullet_Manager.Instance.Bullet_bounce_Type && !Bullet_Manager.Instance.Bullet_Boom_Type) gameObject.SetActive(false);
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
    #region Bullet_Split
    private void Bullet_Split(Vector3 contactPoint, HashSet<Collider> parentHitMonsters, Collider other)
    {
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(DestroyAfterDelay(3f)); // 3초 후 자동 삭제
        }

        Vector3 originalDirection = Vector3.forward;
        Bullet_Manager.Instance.Bullet_Spirt_Count = Mathf.Clamp(Bullet_Manager.Instance.Bullet_Spirt_Count, 1, 5);
        float angleStep = 360f / Bullet_Manager.Instance.Bullet_Spirt_Count;
        float offsetAngle = (Bullet_Manager.Instance.Bullet_Spirt_Count % 2 == 0) ? angleStep / 2 : 0; // 짝수 개수일 경우 중심 맞춤
        for (int i = 0; i < Bullet_Manager.Instance.Bullet_Spirt_Count; i++)
        {
            float angleOffset = (i * angleStep) + offsetAngle;
            Vector3 nextDirection = Quaternion.Euler(0, angleOffset, 0) * originalDirection;
            Vector3 spawnPosition = contactPoint + (nextDirection * Bullet_Manager.Instance.Bullet_Spirt_Offset);
            GameObject newBullet = Instantiate(Bullet_Manager.Instance.Bullet_Spirt, spawnPosition, Quaternion.LookRotation(nextDirection));

            newBullet.SetActive(true);
            Bullet_Split newBulletScript = newBullet.GetComponent<Bullet_Split>();
            if (newBulletScript != null) newBulletScript.Inherit_Hit_Monsters(parentHitMonsters);


            Rigidbody newRb = newBullet.GetComponent<Rigidbody>();
            if (newRb != null) newRb.velocity = nextDirection * Bullet_Manager.Instance.Bullet_Speed;
            Destroy(newBullet, 3f);
        }
    }
    #endregion
    #region BUllet_penetrate
    private void BUllet_penetrate()
    {
        if (Bullet_Manager.Instance.penetration >= Bullet_Manager.Instance.max_penetration) gameObject.SetActive(false);
        else Bullet_Manager.Instance.penetration += 1;
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

        transform.position = contactPoint + (nextDirection.normalized * Bullet_Manager.Instance.Bullet_Spirt_Offset);
        transform.rotation = Quaternion.LookRotation(nextDirection);
        foreach (Transform child in transform)
        {
            Vector3 childEuler = child.localEulerAngles;
            child.localEulerAngles = new Vector3(0, childEuler.y, childEuler.z);
        }
        if (rigid != null)
        {
            rigid.velocity = nextDirection * Bullet_Manager.Instance.Bullet_Speed;
        }
        else
        {
            gameObject.SetActive(false);
        }
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
        float fixedSpeed = Bullet_Manager.Instance.Bullet_Speed; // 속도 고정
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
        // 충돌 지점 계산
        Vector3 contactPoint = GetContactPoint(other);

        // 총알이 향해야 할 방향 계산 (대상을 바라보도록)
        Vector3 bulletDirection = (other.transform.position - contactPoint).normalized;

        // 총알을 생성할 위치 계산 (충돌 지점에서 뒤로 이동)
        Vector3 spawnPosition = contactPoint - bulletDirection;

        // 오브젝트 풀에서 총알 가져오기
        Transform bullet = GameManager.Instance.pool.Bullet_Get(4).transform;

        // 총알의 위치와 방향 설정
        bullet.position = spawnPosition;
        bullet.rotation = Quaternion.LookRotation(bulletDirection);
    }
    #endregion
}
