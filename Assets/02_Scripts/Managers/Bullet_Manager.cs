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
        Transform bullet = GameManager.Instance.pool.Bullet_Get(0).transform; // ������Ʈ Ǯ���� ������
        if (bullet != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>(); // ���ο� �Ѿ��� ��ũ��Ʈ ��������
            bulletScript.bounceCount = currentBounceCount; // ���� ƨ�� Ƚ�� ����

            bullet.position = transform.position; // ���� �Ѿ� ��ġ���� ����
            bullet.rotation = Quaternion.LookRotation(targetPosition - transform.position);
            bullet.gameObject.SetActive(true);

            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = (targetPosition - transform.position).normalized * GameManager.Instance.Bullet_Speed; // �ӵ� ����
        }
    }
    public Vector3 GetContactPoint(Collider other)
    {
        return other.transform.position;
    }
}
