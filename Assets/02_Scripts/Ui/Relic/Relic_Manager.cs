using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_Manager : MonoBehaviour
{
    public RelicDatabase relicDatabase;  // ScriptableObject �����ͺ��̽� ��������

    public Relic_Data GetRelicById(int id)
    {
        foreach (var relic in relicDatabase.relicsList)
        {
            if (relic.Relics_id == id)
            {
                return relic;  // ID�� ��ġ�ϴ� ���� ������ ��ȯ
            }
        }
        return null;  // ã�� ���ϸ� null ��ȯ
    }
}
