using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public Monster_Spawn_Data[] spawnData;

    float timer;

    //열거형으로 정리
    bool spawn_type;
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (GameManager.Instance.spawn_count >= GameManager.Instance.max_spawn_count) return;

        // 스폰 타입이 꺼져 있을 때만 타이머 증가
        if (!spawn_type)
            timer += Time.deltaTime;

        // 스폰 조건 체크
        if (timer > GameManager.Instance.spawn_timer)
        {
            spawn_type = true; // 스폰 중 상태로 전환
            timer = 0f; // 타이머 초기화
            Spawn(); // 스폰 실행
        }
    }
    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        GameManager.Instance.spawn_count += 1;
        enemy.GetComponent<Enumy_Monster>().Init(spawnData[GameManager.Instance.count_day]);
        spawn_type = false;
    }
}
