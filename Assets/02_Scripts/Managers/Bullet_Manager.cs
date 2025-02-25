using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Manager : MonoBehaviour
{
    public static Bullet_Manager Instance;

    [Header("## -- Bullet -- ##")]
    public float Bullet_Speed = 5;           // ÅºÈ¯ ¼Óµµ

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
    public bool Bullet_Bezier_Type;             //ÃÑ¾Ë º§Áö¿¡

    [Header("## -- Bullet_Fire -- ##")]
    public GameObject[] EffectPrefab; // ¸Â¾ÒÀ» ¶§ ½ÇÇàÇÒ ÆÄÆ¼Å¬ ÇÁ¸®ÆÕ

    [Header("## -- Bullet_Propulsion -- ##")]
    public float Origin_Spped;
    public float Propulsion_Speed;

    [Header("## -- Bullet_Speaker -- ##")]
    public float Origianl_Bullet_Speed; // ¿ø·¡ °ø°Ý µô·¹ÀÌ °ª ÀúÀå
    public float Speaker_Speed = 0.3f; //= °¡¼Ó
    public short Shot_Max_count;

    [Header("## -- Bullet_ShotGun -- ##")]
    public int Bullet_ShotGun_Count;

    [Header("## -- Bullet_Penetrate -- ##")]
    public int max_penetration = 5;
    public int penetration = 0;

    [Header("## -- Bullet_NucBack -- ##")]
    public float NucBack_distance = 5.0f;

    [Header("## -- Bullet_Spirt -- ##")]
    public GameObject Bullet_Spirt;
    public int Bullet_Spirt_Count;
    public float Bullet_Spirt_Offset = 0.5f;

    [Header("## -- Bullet_Speaker -- ##")]
    public int Bullet_Speaker_Count;

    [Header("## -- Bullet_Speaker -- ##")]
    public float Bullet_Scan_Range;

    [Header("## -- Bullet_Bezier -- ##")]
    public int Bullet_Bezier_Count;


    void Awake()
    {
        Instance = this;
    }
    public void Effect_Fire(short bullet_count, Vector3 Fire_Object)
    {
       GameObject effect = Instantiate(EffectPrefab[bullet_count], Fire_Object, Quaternion.identity);
        Destroy(effect, 1f);
    }
}
