using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RelicDatabase", menuName = "Scriptable Object/Relic Database")]
public class RelicDatabase : ScriptableObject
{
    public List<Relic_Data> relicsList;  // ��� Relic_Data ����
}

[CreateAssetMenu(fileName = "Relic", menuName = "Scriptble Object/Relics_Data")]
public class Relic_Data : ScriptableObject
{
    [Header("## -- Relics -- ##")]
    public short Relics_id;               //�������̵�
    public string Relics_Name;          //�����̸�
    public int Relics_Lv;               //���� ����
    public int Relics_Max_Lv;           //���� �ִ� ����
    [TextArea]
    public string item_desc;
    public string Relic_lcon;          //���������� ���ҽ� ���

    [Header("## -- Player -- ##")]
    public float[] Bullet_Pow_UP;         //���ݷ� ����
    public float[] Bullet_Pow_Down;       //���ݷ� ����
    public float[] Bullet_Fispeed_UP;     //�߻� �ӵ� ����
    public float[] Bullet_Fispeed_Down;   //�߻� �ӵ� ����
    public float[] Bullet_Size_Up;        //źȯ ũ�� ���� 
    public float[] Bullet_Size_Down;      //źȯ ũ�� ����
    public int[] Bullet_Pec_Up;           //���� Ƚ�� ����
    public int[] Bullet_Pec_Down;         //���� Ƚ�� ����
    public int[] Bullet_Buc_Up;           //ƨ�� Ƚ�� ����
    public int[] Bullet_Buc_Down;         //ƨ�� Ƚ�� ����
    public float[] Bullet_Noc_Up;         //�˹� ����
    public int[] Bullet_Split_Up;         //�Ѿ� �п�ü �� ����
    public int[] Bullet_Multi_Shot_Up;    //�Ѿ� �߻�ü �� ����
    public float[] Aiming_Range_Up;       //���� ���� ����
    public float[] Aiming_Range_Down;     //���� ���� ����
    public int[] Bullet_Speaker_Up;       //�Ѿ� ���� �߻� ��
    public bool Bullet_Return;           //�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
    public bool Bullet_Sniping;          //����ָ��ִ� �� ��� ����
    public bool Bullet_Gra_Speed;        //�Ѿ� ������ �̵� ����
    public bool Bullet_Bomb;             //�Ѿ� Ÿ�� ���� �� ���� ���� ����
    public bool Bullet_Idt;              //�Ѿ� ���� ����
    public int[] Bullet_Effec_Tg;         //�Ѿ� ȿ�� Ʈ���� Ƚ��
    public int[] Summons_ld;              //��ȯü ���� id

    [Header("## -- Channel -- ##")]
    public string Bullet_Projec_tile;   //�Ѿ� ����ü ��ƼŬ���ҽ� ���
    public string Bullet_Hit;           //�Ѿ� ���� ��ƼŬ ���ҽ� ���
    public string Bullet_Muzzle;        //�Ѿ� �߻� ��ƼŬ ���ҽ� ���
}
