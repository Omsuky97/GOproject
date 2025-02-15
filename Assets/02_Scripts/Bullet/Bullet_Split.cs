using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Split : MonoBehaviour
{
    private HashSet<Collider> Hit_Split_Enemys = new HashSet<Collider>();

    public float Split_damage;
    public Bullet Bullet;


    private void Start()
    {
        Split_damage = Bullet.damage;
    }

    public void Inherit_Hit_Monsters(HashSet<Collider> parentHitMonsters)
    {
        Hit_Split_Enemys = new HashSet<Collider>(parentHitMonsters); // �θ� źȯ�� ���� ���� ������ ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Hit_Split_Enemys.Contains(other)) return; //�̹� ���� ���Ͷ�� ����
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
