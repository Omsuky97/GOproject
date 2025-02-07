using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public Monster_Spawn_Data[] spawnData;

    float timer;

    //���������� ����
    bool spawn_type;
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (GameManager.Instance.spawn_count >= GameManager.Instance.max_spawn_count) return;

        // ���� Ÿ���� ���� ���� ���� Ÿ�̸� ����
        if (!spawn_type)
            timer += Time.deltaTime;

        // ���� ���� üũ
        if (timer > GameManager.Instance.spawn_timer)
        {
            spawn_type = true; // ���� �� ���·� ��ȯ
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
            Spawn(); // ���� ����
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
