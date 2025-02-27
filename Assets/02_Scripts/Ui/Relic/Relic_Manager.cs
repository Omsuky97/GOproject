using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_Manager : MonoBehaviour
{
    private static Relic_Manager instance;
    public RelicDatabase relicDatabase;  // ScriptableObject 데이터베이스 가져오기

    private void Awake()
    {
        instance = this;
    }
    public Relic_Data GetRelicById(int id)
    {
        foreach (var relic in relicDatabase.relicsList)
        {
            if (relic.Relics_id == id)
            {
                return relic;  // ID와 일치하는 유물 데이터 반환
            }
        }
        return null;  // 찾지 못하면 null 반환
    }
}
