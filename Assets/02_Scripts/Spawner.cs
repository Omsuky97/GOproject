using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public Monster_Spawn_Data[] spawnData;

    // CSVDataReader ����
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
        // ���� ID�� ����
        int monsterId = spawnData[GameManager.Instance.count_day].monsterId;

        // MonsterData�� ������
        MonsterData data;
        if (dataReader.MonsteraData.TryGetValue(monsterId, out data))
        {
            // ������ ��θ� ����Ͽ� ������Ʈ ����
            GameObject enemy = GameManager.Instance.pool.Get(data.MonsterPrefabs);
            if (enemy == null)
            {
                Debug.LogError($"���͸� ������ �� �����ϴ�. ������ ���: {data.MonsterPrefabs}");
                return;
            }

            // ��ġ ����
            if (spawnPoint != null && spawnPoint.Length > 1)
            {
                int index = Random.Range(1, spawnPoint.Length);
                enemy.transform.position = spawnPoint[index].position;
            }
            else
            {
                Debug.LogError("���� ����Ʈ�� �����Ǿ� ���� �ʽ��ϴ�.");
                return;
            }

            GameManager.Instance.spawn_count += 1;

            // ���� �ʱ�ȭ
            enemy.GetComponent<Enumy_Monster>().Init(data);

            spawn_type = false;
        }
        else
        {
            Debug.LogError($"MonsterData�� ã�� �� �����ϴ�. ID: {monsterId}");
        }
    }
}