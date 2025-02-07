using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Bullet : MonoBehaviour
{
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

        if(per > -1)
        {
            rigid.velocity = dir.normalized * 45;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") || per == -1) return;
            per--;
            if (per == -1)
            {
                rigid.velocity = Vector3.zero;
                gameObject.SetActive(false);
            }
    }
}
