using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoBehaviour
{
    public static Bullet_Manager Instance;

    [Header("## -- Bullet_Type -- ##")]
    public bool Bullet_bounce_Type;             //�Ѿ�ƨ��
    public bool BUllet_penetrate_Type;          //�Ѿ˰���
    public bool Bullet_NucBack_Type;            //�Ѿ˳˹�
    public bool Bullet_Boomerang_Type;          //�θ޶�
    public bool Bullet_Propulsion_Type;         //�Ѿ�����
    public bool Bullet_Boom_Type;               //�Ѿ�����
    public bool Bullet_Spirt_Type;              //�Ѿ˺п�
    public bool Bullet_Guided_Type;             //�Ѿ� ����
    public bool Bullet_ShotGun_Type;            //�Ѿ� ����
    public bool Bullet_Speaker_Type;            //�Ѿ� ����
    public bool Bullet_Target_type;             //�Ѿ� �ָ� �ִ� �� ���� ��� ��

    [Header("## -- Bullet_Propulsion -- ##")]
    public float Origin_Spped;
    public float Propulsion_Speed;

    [Header("## -- Bullet_Speaker -- ##")]
    public float Origianl_Bullet_Speed; // ���� ���� ������ �� ����
    public float Speaker_Speed = 0.3f; //= ����
    public short Shot_Max_count;

    void Awake()
    {
        Instance = this;
    }
}
