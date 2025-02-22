using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoBehaviour
{
    public static Bullet_Manager Instance;

    //[Header("## -- Bullet_Targets -- ##")]


    void Awake()
    {
        Instance = this;
    }


    public void SpawnNewBullet(Vector3 targetPosition, int currentBounceCount)
    {
        Transform bullet = GameManager.Instance.pool.Bullet_Get(0).transform; // 오브젝트 풀에서 가져옴
        if (bullet != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>(); // 새로운 총알의 스크립트 가져오기
            bulletScript.bounceCount = currentBounceCount; // 현재 튕긴 횟수 유지

            bullet.position = transform.position; // 현재 총알 위치에서 시작
            bullet.rotation = Quaternion.LookRotation(targetPosition - transform.position);
            bullet.gameObject.SetActive(true);

            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = (targetPosition - transform.position).normalized * GameManager.Instance.Bullet_Speed; // 속도 유지
        }
    }
    public Vector3 GetContactPoint(Collider other)
    {
        return other.transform.position;
    }
}
