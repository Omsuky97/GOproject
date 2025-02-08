using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Relic", menuName = "Scriptble Object/Relics_Data")]
public class Relic_Data : ScriptableObject
{
    [Header("## -- Relics -- ##")]
    public int Relics_id;               //�������̵�
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

    public int weight;
    public Relic_Data(Relic_Data relic)
    {
        this.Relics_id = relic.Relics_id;
        this.Relics_Name = relic.Relics_Name;
        this.Relics_Max_Lv = relic.Relics_Max_Lv;

        this.Bullet_Pow_UP = relic.Bullet_Pow_UP;
        this.Bullet_Pow_Down = relic.Bullet_Pow_Down;
        this.Bullet_Fispeed_UP = relic.Bullet_Fispeed_UP;
        this.Bullet_Fispeed_Down = relic.Bullet_Fispeed_Down;
        this.Bullet_Size_Up = relic.Bullet_Size_Up;
        this.Bullet_Size_Down = relic.Bullet_Size_Down;
        this.Bullet_Pec_Up = relic.Bullet_Pec_Up;
        this.Bullet_Pec_Down = relic.Bullet_Pec_Down;
        this.Bullet_Buc_Up = relic.Bullet_Buc_Up;
        this.Bullet_Buc_Down = relic.Bullet_Buc_Down;
        this.Bullet_Noc_Up = relic.Bullet_Noc_Up;
        this.Bullet_Split_Up = relic.Bullet_Split_Up;
        this.Bullet_Multi_Shot_Up = relic.Bullet_Multi_Shot_Up;
        this.Aiming_Range_Up = relic.Aiming_Range_Up;
        this.Aiming_Range_Down = relic.Aiming_Range_Down;
        this.Bullet_Speaker_Up = relic.Bullet_Speaker_Up;
        this.Bullet_Return = relic.Bullet_Return;
        this.Bullet_Sniping = relic.Bullet_Sniping;
        this.Bullet_Gra_Speed = relic.Bullet_Gra_Speed;
        this.Bullet_Bomb = relic.Bullet_Bomb;
        this.Bullet_Idt = relic.Bullet_Idt;
        this.Bullet_Effec_Tg = relic.Bullet_Effec_Tg;
        this.Summons_ld = relic.Summons_ld;

        this.Bullet_Projec_tile = relic.Bullet_Projec_tile;
        this.Bullet_Hit = relic.Bullet_Hit;
        this.Bullet_Muzzle = relic.Bullet_Muzzle;
        this.Relic_lcon = relic.Relic_lcon;
    }

    public void Bullet_Power_Up()
    {
        GameManager.Instance.bullet_damage = Bullet_Pow_UP[Relics_Lv];
    }
}
