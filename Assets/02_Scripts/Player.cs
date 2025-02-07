using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //이 조건문 나중에 열거형으로 작성
    public bool isLive = true;

    public Player_Camera cameraShake;
    public float shakeTime; // 흔들림 지속 시간
    public float shakePower; // 흔들림 강도

    private void Start()
    {
        cameraShake = Camera.main.GetComponent<Player_Camera>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy_Attack"))
        {
            string type_name = "Player";
            Monster_Attack monster = other.GetComponent<Monster_Attack>();
            //cameraShake.Camera_Shake(shakeTime, shakePower);
            Base_Chartacter_Essential_Funtion.instance.TakeDamage(gameObject, ref GameManager.Instance.player_hp, monster.monster_Attack_damage, isLive, type_name);
        }
    }
}
