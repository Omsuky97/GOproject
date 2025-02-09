using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class CSVDataReader : MonoBehaviour
{
    #region Data Tables
    [Tooltip("���� ���̺�")]
    public TextAsset RelicsTbl;
    [Tooltip("�������� ���̺�")]
    public TextAsset StageTbl;
    [Tooltip("���� ���̺�")]
    public TextAsset MonsterTbl;
    [Tooltip("���̺� ���̺�")]
    public TextAsset WaveTbl;
    [Tooltip("��ȯü ���̺�")]
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

        Debug.Log("����");
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
        List<Dictionary<string, object>> data = CSVReader.Read(MonsterTbl);
        for (int i = 0; i < data.Count; i++)
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

    public int id;// ���� ���̵�
    public string RelicsNeam;// ���� �̸�
    public int RelicsMaxLv;// ���� �ִ� ����
    public float BulletsPowUp;// ���ݷ� ����
    public float BulletPowDown;// ���ݷ� ����
    public float BulletFispeedUp;// �߻� �ӵ� ����
    public float BulletFispeedDown;// �߻� �ӵ� ����
    public float BulletMoveSpeedUp;// �Ѿ� �̵� �ӵ� ����
    public float BulletMoveSpeedDown;// �Ѿ� �̵� �ӵ� ����
    public float BulletsizeUp;// źȯũ�� ����
    public float BulletsizeDown;// źȯũ�� ����
    public int BulletPecUp;// ���� Ƚ������
    public int BulletPecDoow;// ���� Ƚ������
    public int BulletBucUp;// ƨ�� Ƚ������
    public int BulletBucDown;// ƨ�� Ƚ������
    public float BulletNocUp;// �˹�����
    public int BulletSplitUp;// �Ѿ� �п�ü �� ����
    public int BulletMultiShotUp;// �Ѿ� �߻�ü �� ����
    public float AimingRangeUp;// ���ع��� ����
    public float AimingRangeDown;// ���ع��� ����
    public int BulletSpeakerUP;// �Ѿ� ���� �߻� ��
    public int BulletReturn;// �Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
    public int BulletSniping;// ����ָ��ִ� �� ��ݿ���
    public int BulletGraSpeed;// �Ѿ��������̵�����
    public int BulletBomb;// �Ѿ� Ÿ�� ���� �� ���� ���� ����
    public int BulletIdt;// �Ѿ� ���� ����
    public int BulletEffectTg;// �Ѿ� ȿ�� Ʈ���� Ƚ��
    public int summonsId;// ��ȯü ���� id
    public string BulletProjectile;// �Ѿ�����ü��ƼŬ���ҽ� ���
    public string BulletHit;// �Ѿ˸�����ƼŬ���ҽ� ���
    public string BulletMuzzle;// �Ѿ˹߻���ƼŬ���ҽ� ���
    public string RelicsIcon;// ���������ܸ��ҽ����
}
public class StageData
{

    public int id;// �������� ���̵�
    public string StageName;// �������� �̸�
    public int WaveId_01;// ���̺� ���̵� ���� �÷�
    public int WaveId_02;// ���̺� ���̵� ���� �÷�
    public int WaveId_03;// ���̺� ���̵� ���� �÷�
    public int WaveId_04;// ���̺� ���̵� ���� �÷�
    public int WaveId_05;// ���̺� ���̵� ���� �÷�
    public int WaveId_06;// ���̺� ���̵� ���� �÷�
    public int WaveId_07;// ���̺� ���̵� ���� �÷�
    public int WaveId_08;// ���̺� ���̵� ���� �÷�
    public int WaveId_09;// ���̺� ���̵� ���� �÷�
    public int WaveId_10;// ���̺� ���̵� ���� �÷�
    public int WaveId_11;// ���̺� ���̵� ���� �÷�
    public int WaveId_12;// ���̺� ���̵� ���� �÷�
    public int WaveId_13;// ���̺� ���̵� ���� �÷�
    public int WaveId_14;// ���̺� ���̵� ���� �÷�
    public int WaveId_15;// ���̺� ���̵� ���� �÷�
    public int WaveId_16;// ���̺� ���̵� ���� �÷�
    public int WaveId_17;// ���̺� ���̵� ���� �÷�
    public int WaveId_18;// ���̺� ���̵� ���� �÷�
    public int WaveId_19;// ���̺� ���̵� ���� �÷�
    public int WaveId_20;// ���̺� ���̵� ���� �÷�
    public int WaveId_21;// ���̺� ���̵� ���� �÷�
    public int WaveId_22;// ���̺� ���̵� ���� �÷�
    public int WaveId_23;// ���̺� ���̵� ���� �÷�
    public int WaveId_24;// ���̺� ���̵� ���� �÷�
    public int WaveId_25;// ���̺� ���̵� ���� �÷�
    public int WaveId_26;// ���̺� ���̵� ���� �÷�
    public int WaveId_27;// ���̺� ���̵� ���� �÷�
    public int WaveId_28;// ���̺� ���̵� ���� �÷�
    public int WaveId_29;// ���̺� ���̵� ���� �÷�
    public int WaveId_30;// ���̺� ���̵� ���� �÷�
    public int WaveId_31;// ���̺� ���̵� ���� �÷�
    public int WaveId_32;// ���̺� ���̵� ���� �÷�
    public int WaveId_33;// ���̺� ���̵� ���� �÷�
    public int WaveId_34;// ���̺� ���̵� ���� �÷�
    public int WaveId_35;// ���̺� ���̵� ���� �÷�
    public int WaveId_36;// ���̺� ���̵� ���� �÷�
    public int WaveId_37;// ���̺� ���̵� ���� �÷�
    public int WaveId_38;// ���̺� ���̵� ���� �÷�
    public int WaveId_39;// ���̺� ���̵� ���� �÷�
    public int WaveId_40;// ���̺� ���̵� ���� �÷�
    public int WaveId_41;// ���̺� ���̵� ���� �÷�
    public int WaveId_42;// ���̺� ���̵� ���� �÷�
    public int WaveId_43;// ���̺� ���̵� ���� �÷�
    public int WaveId_44;// ���̺� ���̵� ���� �÷�
    public int WaveId_45;// ���̺� ���̵� ���� �÷�
    public int WaveId_46;// ���̺� ���̵� ���� �÷�
    public int WaveId_47;// ���̺� ���̵� ���� �÷�
    public int WaveId_48;// ���̺� ���̵� ���� �÷�
    public int WaveId_49;// ���̺� ���̵� ���� �÷�
    public int WaveId_50;// ���̺� ���̵� ���� �÷�
}
public class WaveData
{

    public int id;// ���̺� ���̵�
    public int WaveType;// ���̺� Ÿ�� 1=�Ϲ� 2=����
    public int MonsterId_01;// ���� ���̵� ����1
    public int MonsterId_02;// ���� ���̵� ����2
    public int MonsterId_03;// ���� ���̵� ����3
    public int MonsterId_01Spawn;// ���� ���̵� 1 ���� ��
    public int MonsterId_02Spawn;// ���� ���̵� 2 ���� ��
    public int MonsterId_03Spawn;// ���� ���̵� 3 ���� ��
}
public class MonsterData
{

    public int id;// ���� ���̵�
    public float MonsterMoveSpeed;// ���� �̵� �ӵ�
    public float MonsterMaxHp;// ���� ü��
    public float MonsterAtk;// ���� ���ݷ�
    public float MonsterDef;// ���� ����
    public float MonsterGoid;// ���� óġ ���
    public int MonsterAtkType;// ���� ���� Ÿ��
    public int MonsterDrItem;// ���� óġ �� ȹ�� ������
    public string MonsterPrefabs;// ���� ������ ���
}
public class SummonData
{

    public int id;// ��ȯü ���̵�
    public float SunBulAtk;// ��ȯü ���ݷ� ����
    public float SunBulFispeed;// ��ȯü ���ݼӵ�
    public float SunBulMoveSpeed;// ��ȯü ����ü �̵� �ӵ�
    public int SunBulSpeaker;// ��ȯü ����ü ���� �߻� Ƚ��
    public int SunBomb;// ��ȯü ����ü ���� ���� ����
    public float SunButsize;// ��ȯü ����ü ���� ũ��
    public string SunPrefabs;// ��ȯü ������ ���
}
