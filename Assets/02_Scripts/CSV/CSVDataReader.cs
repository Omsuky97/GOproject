using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class CSVDataReader : MonoBehaviour
{
    #region Data Tables
    [Tooltip("유물 테이블")]
    public TextAsset RelicsTbl;
    [Tooltip("스테이지 테이블")]
    public TextAsset StageTbl;
    [Tooltip("몬스터 테이블")]
    public TextAsset MonsterTbl;
    [Tooltip("웨이브 테이블")]
    public TextAsset WaveTbl;
    [Tooltip("소환체 테이블")]
    public TextAsset SummonTbl;

    #endregion


    #region Dictionart

    public Dictionary<int,   RelicsData> RelicsaData = new Dictionary<int, RelicsData>();
    public Dictionary<int, StageData> StageaData = new Dictionary<int, StageData>();
    public Dictionary<int,   WaveData> WaveaData = new Dictionary<int, WaveData>();
    public Dictionary<int,   MonsterData> MonsteraData = new Dictionary<int, MonsterData>();
    public Dictionary<int,   SummonData> SummonaData = new Dictionary<int, SummonData>();



    #endregion


    void Start()
    {
        RelicsDataLoad();
        StageDataLoad();
        WaveDataLoad();
        SummonDataLoad();
        MonsterDataLoad();

        Debug.Log("섹스");
    }



    void RelicsDataLoad()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(RelicsTbl); for (int i = 0; i < data.Count; i++)
        {
            RelicsData stData = new RelicsData();
            stData.id = int.Parse(data[i]["id"].ToString());
            stData.RelicsNeam = data[i]["RelicsNeam"].ToString();
            stData.RelicsMaxLv = int.Parse(data[i]["RelicsMaxLv"].ToString());
            stData.BulletsPowUp = float.Parse(data[i]["BulletsPowUp"].ToString());
            stData.BulletPowDown = float.Parse(data[i]["BulletPowDown"].ToString());
            stData.BulletFispeedUp = float.Parse(data[i]["BulletFispeedUp"].ToString());
            stData.BulletFispeedDown = float.Parse(data[i]["BulletFispeedDown"].ToString());
            stData.BulletMoveSpeedUp = float.Parse(data[i]["BulletMoveSpeedUp"].ToString());
            stData.BulletMoveSpeedDown = float.Parse(data[i]["BulletMoveSpeedDown"].ToString());
            stData.BulletsizeUp = float.Parse(data[i]["BulletsizeUp"].ToString());
            stData.BulletsizeDown = float.Parse(data[i]["BulletsizeDown"].ToString());
            stData.BulletPecUp = int.Parse(data[i]["BulletPecUp"].ToString());
            stData.BulletPecDoow = int.Parse(data[i]["BulletPecDoow"].ToString());
            stData.BulletBucUp = int.Parse(data[i]["BulletBucUp"].ToString());
            stData.BulletBucDown = int.Parse(data[i]["BulletBucDown"].ToString());
            stData.BulletNocUp = float.Parse(data[i]["BulletNocUp"].ToString());
            stData.BulletSplitUp = int.Parse(data[i]["BulletSplitUp"].ToString());
            stData.BulletMultiShotUp = int.Parse(data[i]["BulletMultiShotUp"].ToString());
            stData.AimingRangeUp = float.Parse(data[i]["AimingRangeUp"].ToString());
            stData.AimingRangeDown = float.Parse(data[i]["AimingRangeDown"].ToString());
            stData.BulletSpeakerUP = int.Parse(data[i]["BulletSpeakerUP"].ToString());
            stData.BulletReturn = int.Parse(data[i]["BulletReturn"].ToString());
            stData.BulletSniping = int.Parse(data[i]["BulletSniping"].ToString());
            stData.BulletGraSpeed = int.Parse(data[i]["BulletGraSpeed"].ToString());
            stData.BulletBomb = int.Parse(data[i]["BulletBomb"].ToString());
            stData.BulletIdt = int.Parse(data[i]["BulletIdt"].ToString());
            stData.BulletEffectTg = int.Parse(data[i]["BulletEffectTg"].ToString());
            stData.summonsId = int.Parse(data[i]["summonsId"].ToString());
            stData.BulletProjectile = data[i]["BulletProjectile"].ToString();
            stData.BulletHit = data[i]["BulletHit"].ToString();
            stData.BulletMuzzle = data[i]["BulletMuzzle"].ToString();
            stData.RelicsIcon = data[i]["RelicsIcon"].ToString();
            RelicsaData.Add(stData.id, stData);
        }
    }
    void StageDataLoad()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(StageTbl); for (int i = 0; i < data.Count; i++)
        {
            StageData stData = new StageData();
            stData.id = int.Parse(data[i]["id"].ToString());
            stData.StageName = data[i]["StageName"].ToString();
            stData.WaveId_01 = int.Parse(data[i]["WaveId_01"].ToString());
            stData.WaveId_02 = int.Parse(data[i]["WaveId_02"].ToString());
            stData.WaveId_03 = int.Parse(data[i]["WaveId_03"].ToString());
            stData.WaveId_04 = int.Parse(data[i]["WaveId_04"].ToString());
            stData.WaveId_05 = int.Parse(data[i]["WaveId_05"].ToString());
            stData.WaveId_06 = int.Parse(data[i]["WaveId_06"].ToString());
            stData.WaveId_07 = int.Parse(data[i]["WaveId_07"].ToString());
            stData.WaveId_08 = int.Parse(data[i]["WaveId_08"].ToString());
            stData.WaveId_09 = int.Parse(data[i]["WaveId_09"].ToString());
            stData.WaveId_10 = int.Parse(data[i]["WaveId_10"].ToString());
            stData.WaveId_11 = int.Parse(data[i]["WaveId_11"].ToString());
            stData.WaveId_12 = int.Parse(data[i]["WaveId_12"].ToString());
            stData.WaveId_13 = int.Parse(data[i]["WaveId_13"].ToString());
            stData.WaveId_14 = int.Parse(data[i]["WaveId_14"].ToString());
            stData.WaveId_15 = int.Parse(data[i]["WaveId_15"].ToString());
            stData.WaveId_16 = int.Parse(data[i]["WaveId_16"].ToString());
            stData.WaveId_17 = int.Parse(data[i]["WaveId_17"].ToString());
            stData.WaveId_18 = int.Parse(data[i]["WaveId_18"].ToString());
            stData.WaveId_19 = int.Parse(data[i]["WaveId_19"].ToString());
            stData.WaveId_20 = int.Parse(data[i]["WaveId_20"].ToString());
            stData.WaveId_21 = int.Parse(data[i]["WaveId_21"].ToString());
            stData.WaveId_22 = int.Parse(data[i]["WaveId_22"].ToString());
            stData.WaveId_23 = int.Parse(data[i]["WaveId_23"].ToString());
            stData.WaveId_24 = int.Parse(data[i]["WaveId_24"].ToString());
            stData.WaveId_25 = int.Parse(data[i]["WaveId_25"].ToString());
            stData.WaveId_26 = int.Parse(data[i]["WaveId_26"].ToString());
            stData.WaveId_27 = int.Parse(data[i]["WaveId_27"].ToString());
            stData.WaveId_28 = int.Parse(data[i]["WaveId_28"].ToString());
            stData.WaveId_29 = int.Parse(data[i]["WaveId_29"].ToString());
            stData.WaveId_30 = int.Parse(data[i]["WaveId_30"].ToString());
            stData.WaveId_31 = int.Parse(data[i]["WaveId_31"].ToString());
            stData.WaveId_32 = int.Parse(data[i]["WaveId_32"].ToString());
            stData.WaveId_33 = int.Parse(data[i]["WaveId_33"].ToString());
            stData.WaveId_34 = int.Parse(data[i]["WaveId_34"].ToString());
            stData.WaveId_35 = int.Parse(data[i]["WaveId_35"].ToString());
            stData.WaveId_36 = int.Parse(data[i]["WaveId_36"].ToString());
            stData.WaveId_37 = int.Parse(data[i]["WaveId_37"].ToString());
            stData.WaveId_38 = int.Parse(data[i]["WaveId_38"].ToString());
            stData.WaveId_39 = int.Parse(data[i]["WaveId_39"].ToString());
            stData.WaveId_40 = int.Parse(data[i]["WaveId_40"].ToString());
            stData.WaveId_41 = int.Parse(data[i]["WaveId_41"].ToString());
            stData.WaveId_42 = int.Parse(data[i]["WaveId_42"].ToString());
            stData.WaveId_43 = int.Parse(data[i]["WaveId_43"].ToString());
            stData.WaveId_44 = int.Parse(data[i]["WaveId_44"].ToString());
            stData.WaveId_45 = int.Parse(data[i]["WaveId_45"].ToString());
            stData.WaveId_46 = int.Parse(data[i]["WaveId_46"].ToString());
            stData.WaveId_47 = int.Parse(data[i]["WaveId_47"].ToString());
            stData.WaveId_48 = int.Parse(data[i]["WaveId_48"].ToString());
            stData.WaveId_49 = int.Parse(data[i]["WaveId_49"].ToString());
            stData.WaveId_50 = int.Parse(data[i]["WaveId_50"].ToString());
            StageaData.Add(stData.id, stData);
        }
    }
    void WaveDataLoad()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(WaveTbl); for (int i = 0; i < data.Count; i++)
        {
            WaveData stData = new WaveData();
            stData.id = int.Parse(data[i]["id"].ToString());
            stData.WaveType = int.Parse(data[i]["WaveType"].ToString());
            stData.MonsterId_01 = int.Parse(data[i]["MonsterId_01"].ToString());
            stData.MonsterId_02 = int.Parse(data[i]["MonsterId_02"].ToString());
            stData.MonsterId_03 = int.Parse(data[i]["MonsterId_03"].ToString());
            stData.MonsterId_01Spawn = int.Parse(data[i]["MonsterId_01Spawn"].ToString());
            stData.MonsterId_02Spawn = int.Parse(data[i]["MonsterId_02Spawn"].ToString());
            stData.MonsterId_03Spawn = int.Parse(data[i]["MonsterId_03Spawn"].ToString());
            WaveaData.Add(stData.id, stData);
        }
    }
    void MonsterDataLoad()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(MonsterTbl); for (int i = 0; i < data.Count; i++)
        {
            MonsterData stData = new MonsterData();
            stData.id = int.Parse(data[i]["id"].ToString());
            stData.MonsterMoveSpeed = float.Parse(data[i]["MonsterMoveSpeed"].ToString());
            stData.MonsterMaxHp = float.Parse(data[i]["MonsterMaxHp"].ToString());
            stData.MonsterAtk = float.Parse(data[i]["MonsterAtk"].ToString());
            stData.MonsterDef = float.Parse(data[i]["MonsterDef"].ToString());
            stData.MonsterGoid = float.Parse(data[i]["MonsterGoid"].ToString());
            stData.MonsterAtkType = int.Parse(data[i]["MonsterAtkType"].ToString());
            stData.MonsterDrItem = int.Parse(data[i]["MonsterDrItem"].ToString());
            stData.MonsterPrefabs = data[i]["MonsterPrefabs"].ToString();
            MonsteraData.Add(stData.id, stData);
        }
    }
    void SummonDataLoad()
    {
        List<Dictionary<string, object>> data = CSVReader.Read(SummonTbl); for (int i = 0; i < data.Count; i++)
        {
            SummonData stData = new SummonData();
            stData.id = int.Parse(data[i]["id"].ToString());
            stData.SunBulAtk = float.Parse(data[i]["SunBulAtk"].ToString());
            stData.SunBulFispeed = float.Parse(data[i]["SunBulFispeed"].ToString());
            stData.SunBulMoveSpeed = float.Parse(data[i]["SunBulMoveSpeed"].ToString());
            stData.SunBulSpeaker = int.Parse(data[i]["SunBulSpeaker"].ToString());
            stData.SunBomb = int.Parse(data[i]["SunBomb"].ToString());
            stData.SunButsize = float.Parse(data[i]["SunButsize"].ToString());
            stData.SunPrefabs = data[i]["SunPrefabs"].ToString();
            SummonaData.Add(stData.id, stData);
        }
    }



}





public class RelicsData
{

    public int id;// 유물 아이디
    public string RelicsNeam;// 유물 이름
    public int RelicsMaxLv;// 유물 최대 레벨
    public float BulletsPowUp;// 공격력 증가
    public float BulletPowDown;// 공격력 감소
    public float BulletFispeedUp;// 발사 속도 증가
    public float BulletFispeedDown;// 발사 속도 감소
    public float BulletMoveSpeedUp;// 총알 이동 속도 증가
    public float BulletMoveSpeedDown;// 총알 이동 속도 감소
    public float BulletsizeUp;// 탄환크기 증가
    public float BulletsizeDown;// 탄환크기 감소
    public int BulletPecUp;// 관통 횟수증가
    public int BulletPecDoow;// 관통 횟수감소
    public int BulletBucUp;// 튕김 횟수증가
    public int BulletBucDown;// 튕김 횟수감소
    public float BulletNocUp;// 넉백증가
    public int BulletSplitUp;// 총알 분열체 수 증가
    public int BulletMultiShotUp;// 총알 발사체 수 증가
    public float AimingRangeUp;// 조준범위 증가
    public float AimingRangeDown;// 조준범위 감소
    public int BulletSpeakerUP;// 총알 연속 발사 수
    public int BulletReturn;// 총알 타겟 명중 후 되돌아오는 여부
    public int BulletSniping;// 가장멀리있는 적 사격여부
    public int BulletGraSpeed;// 총알점진적이동여부
    public int BulletBomb;// 총알 타겟 명중 시 범위 피해 여부
    public int BulletIdt;// 총알 유도 여부
    public int BulletEffectTg;// 총알 효과 트리거 횟수
    public int summonsId;// 소환체 참조 id
    public string BulletProjectile;// 총알투사체파티클리소스 경로
    public string BulletHit;// 총알명중파티클리소스 경로
    public string BulletMuzzle;// 총알발사파티클리소스 경로
    public string RelicsIcon;// 유물아이콘리소스경로
}
public class StageData
{

    public int id;// 스테이지 아이디
    public string StageName;// 스테이지 이름
    public int WaveId_01;// 웨이브 아이디 참조 컬럼
    public int WaveId_02;// 웨이브 아이디 참조 컬럼
    public int WaveId_03;// 웨이브 아이디 참조 컬럼
    public int WaveId_04;// 웨이브 아이디 참조 컬럼
    public int WaveId_05;// 웨이브 아이디 참조 컬럼
    public int WaveId_06;// 웨이브 아이디 참조 컬럼
    public int WaveId_07;// 웨이브 아이디 참조 컬럼
    public int WaveId_08;// 웨이브 아이디 참조 컬럼
    public int WaveId_09;// 웨이브 아이디 참조 컬럼
    public int WaveId_10;// 웨이브 아이디 참조 컬럼
    public int WaveId_11;// 웨이브 아이디 참조 컬럼
    public int WaveId_12;// 웨이브 아이디 참조 컬럼
    public int WaveId_13;// 웨이브 아이디 참조 컬럼
    public int WaveId_14;// 웨이브 아이디 참조 컬럼
    public int WaveId_15;// 웨이브 아이디 참조 컬럼
    public int WaveId_16;// 웨이브 아이디 참조 컬럼
    public int WaveId_17;// 웨이브 아이디 참조 컬럼
    public int WaveId_18;// 웨이브 아이디 참조 컬럼
    public int WaveId_19;// 웨이브 아이디 참조 컬럼
    public int WaveId_20;// 웨이브 아이디 참조 컬럼
    public int WaveId_21;// 웨이브 아이디 참조 컬럼
    public int WaveId_22;// 웨이브 아이디 참조 컬럼
    public int WaveId_23;// 웨이브 아이디 참조 컬럼
    public int WaveId_24;// 웨이브 아이디 참조 컬럼
    public int WaveId_25;// 웨이브 아이디 참조 컬럼
    public int WaveId_26;// 웨이브 아이디 참조 컬럼
    public int WaveId_27;// 웨이브 아이디 참조 컬럼
    public int WaveId_28;// 웨이브 아이디 참조 컬럼
    public int WaveId_29;// 웨이브 아이디 참조 컬럼
    public int WaveId_30;// 웨이브 아이디 참조 컬럼
    public int WaveId_31;// 웨이브 아이디 참조 컬럼
    public int WaveId_32;// 웨이브 아이디 참조 컬럼
    public int WaveId_33;// 웨이브 아이디 참조 컬럼
    public int WaveId_34;// 웨이브 아이디 참조 컬럼
    public int WaveId_35;// 웨이브 아이디 참조 컬럼
    public int WaveId_36;// 웨이브 아이디 참조 컬럼
    public int WaveId_37;// 웨이브 아이디 참조 컬럼
    public int WaveId_38;// 웨이브 아이디 참조 컬럼
    public int WaveId_39;// 웨이브 아이디 참조 컬럼
    public int WaveId_40;// 웨이브 아이디 참조 컬럼
    public int WaveId_41;// 웨이브 아이디 참조 컬럼
    public int WaveId_42;// 웨이브 아이디 참조 컬럼
    public int WaveId_43;// 웨이브 아이디 참조 컬럼
    public int WaveId_44;// 웨이브 아이디 참조 컬럼
    public int WaveId_45;// 웨이브 아이디 참조 컬럼
    public int WaveId_46;// 웨이브 아이디 참조 컬럼
    public int WaveId_47;// 웨이브 아이디 참조 컬럼
    public int WaveId_48;// 웨이브 아이디 참조 컬럼
    public int WaveId_49;// 웨이브 아이디 참조 컬럼
    public int WaveId_50;// 웨이브 아이디 참조 컬럼
}
public class WaveData
{

    public int id;// 웨이브 아이디
    public int WaveType;// 웨이브 타입 1=일반 2=보스
    public int MonsterId_01;// 몬스터 아이디 참조1
    public int MonsterId_02;// 몬스터 아이디 참조2
    public int MonsterId_03;// 몬스터 아이디 참조3
    public int MonsterId_01Spawn;// 몬스터 아이디 1 생성 수
    public int MonsterId_02Spawn;// 몬스터 아이디 2 생성 수
    public int MonsterId_03Spawn;// 몬스터 아이디 3 생성 수
}
public class MonsterData
{

    public int id;// 몬스터 아이디
    public float MonsterMoveSpeed;// 몬스터 이동 속도
    public float MonsterMaxHp;// 몬스터 체력
    public float MonsterAtk;// 몬스터 공격력
    public float MonsterDef;// 몬스터 방어력
    public float MonsterGoid;// 몬스터 처치 골드
    public int MonsterAtkType;// 몬스터 공격 타입
    public int MonsterDrItem;// 몬스터 처치 시 획득 아이템
    public string MonsterPrefabs;// 몬스터 프리펩 경로
}
public class SummonData
{

    public int id;// 소환체 아이디
    public float SunBulAtk;// 소환체 공격력 배율
    public float SunBulFispeed;// 소환체 공격속도
    public float SunBulMoveSpeed;// 소환체 투사체 이동 속도
    public int SunBulSpeaker;// 소환체 투사체 연속 발사 횟수
    public int SunBomb;// 소환체 투사체 범위 피해 여부
    public float SunButsize;// 소환체 투사체 범위 크기
    public string SunPrefabs;// 소환체 프리펩 경로
}
