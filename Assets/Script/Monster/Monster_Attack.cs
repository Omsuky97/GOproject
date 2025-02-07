using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Audio_Manager;

public class Monster_Attack : MonoBehaviour
{
    bool attack_type = true;
    public Enumy_Monster monster;
    public float monster_Attack_damage;

    private void Start()
    {
        monster_Attack_damage = monster.monster_damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !attack_type) return;
        if (other.CompareTag("Player") && attack_type)
        {
            attack_type = false;
        }
    }

    public void Monster_Attack_Player()
    {
        if (attack_type == false)
        {
            attack_type = true;
        }
    }
    public void Monster_Attack_End_Player()
    {
        attack_type = false;
    }
}
