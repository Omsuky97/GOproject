using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public Transform[] spawnPoint;
    public Monster_Spawn_Data[] spawnData;

    // CSVDataReader 참조
    public CSVDataReader dataReader;

    public float timer;
    public float Day_Time;
    MonsterData data;
    public bool spawn_type;
    public GameObject boss_hp_Slider;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (GameManager.Instance.Waiting_Time_Type) Day_Time += Time.deltaTime;
            if (GameManager.Instance.spawn_count >= spawnData[GameManager.Instance.count_day].spawnMaxCount) return;
        if (!spawn_type)
            timer += Time.deltaTime;

        if (Day_Time > GameManager.Instance.Fade_TIme +2.0f)
        {
            Day_Time = 0.0f;
            spawn_type = true;
            GameManager.Instance.Waiting_Time_Type = false;
        }

        if (GameManager.Instance.Waiting_Time_Type) return;

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

        if (dataReader.MonsteraData.TryGetValue(monsterId, out data) || !GameManager.Instance.Waiting_Time_Type)
        {
            if (GameManager.Instance.count_day % 5 == 0)
            {
                boss_hp_Slider.SetActive(true);
            }
            else
            {
                boss_hp_Slider.SetActive(false);
            }
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