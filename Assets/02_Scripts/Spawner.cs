using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public Monster_Spawn_Data[] spawnData;

    // CSVDataReader 참조
    public CSVDataReader dataReader;

    float timer;

    bool spawn_type;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (GameManager.Instance.spawn_count >= GameManager.Instance.max_spawn_count) return;

        if (!spawn_type)
            timer += Time.deltaTime;

        if (timer > GameManager.Instance.spawn_timer)
        {
            spawn_type = true;
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        // 몬스터 ID를 결정
        int monsterId = spawnData[GameManager.Instance.count_day].monsterId;

        // MonsterData를 가져옴
        MonsterData data;
        if (dataReader.MonsteraData.TryGetValue(monsterId, out data))
        {
            // 프리팹 경로를 사용하여 오브젝트 생성
            GameObject enemy = GameManager.Instance.pool.Get(data.MonsterPrefabs);
            int index = Random.Range(1, spawnPoint.Length);
            enemy.transform.position = spawnPoint[index].position;
            GameManager.Instance.spawn_count += 1;
            enemy.GetComponent<Enumy_Monster>().Init(data);

            spawn_type = false;
        }
    }
}