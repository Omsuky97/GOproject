using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;  // ���� �ڵ� ����
    List<GameObject>[] pools;     // ���� �ڵ� ����

    private Dictionary<string, List<GameObject>> monsterPools = new Dictionary<string, List<GameObject>>();  // ���� Ǯ �߰�

    private void Awake()
    {
        // ���� �ʱ�ȭ �ڵ� ����
        pools = new List<GameObject>[prefabs.Length];
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    // ������ Get(int index) �޼��� ���� (�Ѿ� ��� ���)
   

    public GameObject Get(int index)
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

    // ���ο� Get(string prefabPath) �޼��� �߰� (���� ������)
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
                Debug.LogError($"�������� �ε��� �� �����ϴ�. ���: {prefabPath}");
                return null;
            }
            select = Instantiate(prefab, transform);
            pool.Add(select);
        }

        return select;
    }
}
