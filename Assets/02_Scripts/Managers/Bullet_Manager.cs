using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoBehaviour
{
    public static Bullet_Manager Instance;

    [Header("## -- Bullet_Type -- ##")]
    public bool Bullet_bounce_Type;             //ÃÑ¾ËÆ¨±è
    public bool BUllet_penetrate_Type;          //ÃÑ¾Ë°üÅë
    public bool Bullet_NucBack_Type;            //ÃÑ¾Ë³Ë¹é
    public bool Bullet_Boomerang_Type;          //ºÎ¸Þ¶û
    public bool Bullet_Propulsion_Type;         //ÃÑ¾ËÃßÁø
    public bool Bullet_Boom_Type;               //ÃÑ¾ËÆø¹ß
    public bool Bullet_Spirt_Type;              //ÃÑ¾ËºÐ¿­
    public bool Bullet_Guided_Type;             //ÃÑ¾Ë À¯µµ
    public bool Bullet_ShotGun_Type;            //ÃÑ¾Ë ¼¦°Ç
    public bool Bullet_Speaker_Type;            //ÃÑ¾Ë Á¡»ç
    public bool Bullet_Target_type;             //ÃÑ¾Ë ¸Ö¸® ÀÖ´Â °Í ºÎÅÍ ½î´Â °Í

    [Header("## -- Bullet_Propulsion -- ##")]
    public float Origin_Spped;
    public float Propulsion_Speed;

    [Header("## -- Bullet_Speaker -- ##")]
    public float Origianl_Bullet_Speed; // ¿ø·¡ °ø°Ý µô·¹ÀÌ °ª ÀúÀå
    public float Speaker_Speed = 0.3f; //= °¡¼Ó
    public short Shot_Max_count;

    void Awake()
    {
        Instance = this;
    }
}
