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
    public short Relics_id;             //�������̵�
    public string Relics_Name;          //�����̸�
    public int Relics_Lv;               //���� ����
    public int Relics_Max_Lv;           //���� �ִ� ����
    [TextArea]
    public string item_desc;           //--������ ���� �߰��ؾ���--
    public string Relic_lcon;          //���������� ���ҽ� ���

    [Header("## -- Player_Stat -- ##")]

    [Tooltip("���ݷ� ����")]
    public float[] Bullet_Pow_UP;         //���ݷ� ����
    [Tooltip("���ݷ� ����")]
    public float[] Bullet_Pow_Down;       //���ݷ� ����
    [Tooltip("�߻� �ӵ� ����")]
    public float[] Bullet_Fispeed_UP;     //�߻� �ӵ� ����
    [Tooltip("�߻� �ӵ� ����")]
    public float[] Bullet_Fispeed_Down;   //�߻� �ӵ� ����
    [Tooltip("�Ѿ� �̵� �ӵ� ����")]
    public float[] Bullet_Move_Speed_Up;  //�Ѿ� �̵� �ӵ� ���� --�߰�
    [Tooltip("�Ѿ� �̵� �ӵ� ����")]
    public float[] Bullet_Move_Speed_Down;//�Ѿ� �̵� �ӵ� ���� --�߰�
    [Tooltip("źȯ ũ�� ���� ")]
    public float[] Bullet_Size_Up;        //źȯ ũ�� ���� 
    [Tooltip("źȯ ũ�� ����")]
    public float[] Bullet_Size_Down;      //źȯ ũ�� ����
    [Tooltip("���� Ƚ�� ����")]
    public int[] Bullet_Pec_Up;           //���� Ƚ�� ����
    [Tooltip("���� Ƚ�� ����")]
    public int[] Bullet_Pec_Down;         //���� Ƚ�� ����
    [Tooltip("ƨ�� Ƚ�� ����")]
    public int[] Bullet_Buc_Up;           //ƨ�� Ƚ�� ����
    [Tooltip("ƨ�� Ƚ�� ����")]
    public int[] Bullet_Buc_Down;         //ƨ�� Ƚ�� ����
    [Tooltip("�˹� ����")]
    public float[] Bullet_Noc_Up;         //�˹� ����
    [Tooltip("�Ѿ� �п�ü �� ����")]
    public int[] Bullet_Split_Up;         //�Ѿ� �п�ü �� ����
    [Tooltip("�Ѿ� �߻�ü �� ����")]
    public int[] Bullet_Multi_Luck_Up;    //�Ѿ� �߻� Ȯ�� ����
    [Tooltip("���� ���� ����")]
    public float[] Aiming_Range_Up;       //���� ���� ����
    [Tooltip("���� ���� ����")]
    public float[] Aiming_Range_Down;     //���� ���� ����
    [Tooltip("�Ѿ� ���� �߻� ����")]
    public int[] Bullet_ShotGun_Count;         //�Ѿ� ������ �̵� ����
    [Tooltip("�Ѿ� ���� �߻� ��")]
    public int[] Bullet_Speaker_Up;       //�Ѿ� ���� �߻� ��

    [Header("## -- Bullet_Type -- ##")]
    [Tooltip("�Ѿ� ȿ�� Ʈ���� Ƚ��")]
    public int[] Bullet_Effec_Tg;         //�Ѿ� ȿ�� Ʈ���� Ƚ��
    [Tooltip("�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����")]
    public bool Bullet_Return;            //�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
    [Tooltip("�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����")]
    public bool Bullet_Bounce;            //�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
    [Tooltip("�Ѿ� ���� ����")]
    public bool Bullet_Pec;           //����ָ��ִ� �� ��� ����
    [Tooltip("����ָ��ִ� �� ��� ����")]
    public bool Bullet_Sniping;           //����ָ��ִ� �� ��� ����
    [Tooltip("�Ѿ� ������ �̵� ����")]
    public bool Bullet_Gra_Speed;         //�Ѿ� ������ �̵� ����
    [Tooltip("�Ѿ� ���� ����")]
    public bool Bullet_ShotGun_Type;         //�Ѿ� ������ �̵� ����
    [Tooltip("�Ѿ� Ÿ�� ���� �� ���� ���� ����")]
    public bool Bullet_Bomb;              //�Ѿ� Ÿ�� ���� �� ���� ���� ����
    [Tooltip("�Ѿ� �п� ����")]
    public bool Bullet_Spirt_Type;               //�Ѿ� ���� ����
    [Tooltip("�Ѿ� ���� ����")]
    public bool Bullet_Idt;               //�Ѿ� ���� ����
    [Tooltip("�Ѿ� �˹� ����")]
    public bool Bullet_Noc;               //�Ѿ� �˹� ���� --�߰�
    [Tooltip("�Ѿ� ȿ�� Ʈ���� Ƚ��")]
    public bool Bullet_Effec_Type;         //�Ѿ� ȿ�� Ʈ���� Ƚ��
    [Tooltip("�Ѿ� ���� �߻� ����")]
    public bool Bullet_Speaker_Type;       //�Ѿ� ���� �߻� ����
    [Tooltip("��ȯü ���� id")]
    public int[] Summons_ld;              //��ȯü ���� id

    [Header("## -- Channel -- ##")]
    public string Bullet_Projec_tile;   //�Ѿ� ����ü ��ƼŬ���ҽ� ���
    public string Bullet_Hit;           //�Ѿ� ���� ��ƼŬ ���ҽ� ���
    public string Bullet_Muzzle;        //�Ѿ� �߻� ��ƼŬ ���ҽ� ���
    public void ResetLevel()
    {
        Relics_Lv = 1;
    }
}
