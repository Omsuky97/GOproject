using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;  // 기존 코드 유지
    List<GameObject>[] pools;     // 기존 코드 유지

    private Dictionary<string, List<GameObject>> monsterPools = new Dictionary<string, List<GameObject>>();  // 몬스터 풀 추가

    private void Awake()
    {
        // 기존 초기화 코드 유지
        pools = new List<GameObject>[prefabs.Length];
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    // 기존의 Get(int index) 메서드 유지 (총알 등에서 사용)
   

    public GameObject Bullet_Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    // 새로운 Get(string prefabPath) 메서드 추가 (몬스터 생성용)
    public GameObject Get(string prefabPath)
    {
        List<GameObject> pool;
        if (!monsterPools.TryGetValue(prefabPath, out pool))
        {
            pool = new List<GameObject>();
            monsterPools.Add(prefabPath, pool);
        }

        GameObject select = null;
        foreach (GameObject item in pool)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                return null;
            }
            select = Instantiate(prefab, transform);
            pool.Add(select);
        }

        return select;
    }
}
