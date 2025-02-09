using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RelicDatabase", menuName = "Scriptable Object/Relic Database")]
public class RelicDatabase : ScriptableObject
{
    public List<Relic_Data> relicsList;  // 모든 Relic_Data 저장
}

[CreateAssetMenu(fileName = "Relic", menuName = "Scriptble Object/Relics_Data")]
public class Relic_Data : ScriptableObject
{
    [Header("## -- Relics -- ##")]
    public short Relics_id;             //유물아이디
    public string Relics_Name;          //유물이름
    public int Relics_Lv;               //유물 레벨
    public int Relics_Max_Lv;           //유물 최대 레벨
    [TextArea]
    public string item_desc;
    public string Relic_lcon;          //유물아이콘 리소스 경로

    [Header("## -- Player -- ##")]

    [Tooltip("공격력 증가")]
    public float[] Bullet_Pow_UP;         //공격력 증가
    [Tooltip("공격력 감소")]
    public float[] Bullet_Pow_Down;       //공격력 감소
    [Tooltip("발사 속도 증가")]
    public float[] Bullet_Fispeed_UP;     //발사 속도 증가
    [Tooltip("발사 속도 감소")]
    public float[] Bullet_Fispeed_Down;   //발사 속도 감소
    [Tooltip("총알 이동 속도 증가")]
    public float[] Bullet_Move_Speed_Up;  //총알 이동 속도 증가
    [Tooltip("총알 이동 속도 감소")]
    public float[] Bullet_Move_Speed_Down;//총알 이동 속도 감소
    [Tooltip("탄환 크기 증가 ")]
    public float[] Bullet_Size_Up;        //탄환 크기 증가 
    [Tooltip("탄환 크기 감소")]
    public float[] Bullet_Size_Down;      //탄환 크기 감소
    [Tooltip("관통 횟수 증가")]
    public int[] Bullet_Pec_Up;           //관통 횟수 증가
    [Tooltip("관통 횟수 감소")]
    public int[] Bullet_Pec_Down;         //관통 횟수 감소
    [Tooltip("튕김 횟수 증가")]
    public int[] Bullet_Buc_Up;           //튕김 횟수 증가
    [Tooltip("튕김 횟수 감소")]
    public int[] Bullet_Buc_Down;         //튕김 횟수 감소
    [Tooltip("넉백 증가")]
    public float[] Bullet_Noc_Up;         //넉백 증가
    [Tooltip("총알 분열체 수 증가")]
    public int[] Bullet_Split_Up;         //총알 분열체 수 증가
    [Tooltip("총알 발사체 수 증가")]
    public int[] Bullet_Multi_Shot_Up;    //총알 발사체 수 증가
    [Tooltip("조준 범위 증가")]
    public float[] Aiming_Range_Up;       //조준 범위 증가
    [Tooltip("조준 범위 감소")]
    public float[] Aiming_Range_Down;     //조준 범위 감소
    [Tooltip("총알 연속 발사 수")]
    public int[] Bullet_Speaker_Up;       //총알 연속 발사 수
    [Tooltip("총알 타겟 명중 후 되돌아오는 여부")]
    public bool Bullet_Return;            //총알 타겟 명중 후 되돌아오는 여부
    [Tooltip("가장멀리있는 적 사격 여부")]
    public bool Bullet_Sniping;           //가장멀리있는 적 사격 여부
    [Tooltip("총알 점진적 이동 여부")]
    public bool Bullet_Gra_Speed;         //총알 점진적 이동 여부
    [Tooltip("총알 타겟 명중 시 범위 피해 여부")]
    public bool Bullet_Bomb;              //총알 타겟 명중 시 범위 피해 여부
    [Tooltip("총알 유도 여부")]
    public bool Bullet_Idt;               //총알 유도 여부
    [Tooltip("총알 효과 트리거 횟수")]
    public int[] Bullet_Effec_Tg;         //총알 효과 트리거 횟수
    [Tooltip("소환체 참조 id")]
    public int[] Summons_ld;              //소환체 참조 id

    [Header("## -- Channel -- ##")]
    public string Bullet_Projec_tile;   //총알 투사체 파티클리소스 경로
    public string Bullet_Hit;           //총알 명중 파티클 리소스 경로
    public string Bullet_Muzzle;        //총알 발사 파티클 리소스 경로
}
