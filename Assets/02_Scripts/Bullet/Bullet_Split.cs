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
        Hit_Split_Enemys = new HashSet<Collider>(parentHitMonsters); // 부모 탄환의 맞은 몬스터 정보를 복사
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Hit_Split_Enemys.Contains(other)) return; //이미 맞은 몬스터라면 무시
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
