using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ShotGun : MonoBehaviour
{
    [Header("## -- Bullet_ShortGun -- ##")]
    public float ShortGun_damage;
    public Bullet Bullet;
    public GameObject Fire_Point;
    public static Vector3 Bullet_Target;
    private Rigidbody rigid;
    public float rotateSpeed = 5f;   // 회전 속도

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void LateUpdate()
    {
        if (Bullet_Manager.Instance.Bullet_Guided_Type)
        {
            if (Bullet_Target == new Vector3(0, 0, 0)) Bullet_Target = Player_Scaner.nearestTarget.position;
            Vector3 targetPosition = Bullet_Target;
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 newDirection = Vector3.Lerp(rigid.velocity.normalized, direction, rotateSpeed * Time.fixedDeltaTime).normalized;

            rigid.velocity = newDirection * GameManager.Instance.Bullet_Speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) gameObject.SetActive(false);
    }
}
