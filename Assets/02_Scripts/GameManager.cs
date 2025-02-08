using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Audio_Manager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PoolManager pool;
    public Player player;

    [Header("## -- Game_System -- ##")]
    public short count_day = 30;

    [Header("## -- Game_Stage_List -- ##")]
    public int spawn_count;
    public float spawn_timer;
    public int max_spawn_count;
    public int kill_enemy_count;

    [Header("## -- Player_Statas_List -- ##")]
    public float player_hp;
    public float player_max_hp;
    public float bullet_damage;
    public int bullet_count;
    public float attack_delay;
    public int gold_count;

    [Header("## -- Player_Hit_HUD -- ##")]
    public Image hit_color_Image; // UI �̹��� ����
    public Color hit_Color = new Color(1f, 0f, 0f, 0.3f); // ������ ������
    public Color originalColor; // ���� ����
    public float colorFadeSpeed = 1f; // ���� ���̵� �ӵ�

    [Header("## -- Player_Hit_HUD -- ##")]
    public float boss_hp;
    public float boss_max_hp;

    [Header("## -- Player_Game_Over_HUD -- ##")]
    //public GameObject player_statas;
    //public Canvas game_option;
    public Canvas game_over;
    public GameObject die_player;

    void Awake()
    {
        Instance = this;
    }
    public void Stage_Level_UP()
    {
        if (kill_enemy_count == max_spawn_count)
        {
            spawn_count = 0;
            kill_enemy_count = 0;
            count_day -= 1;
        }

    }

    public void Game_Over()
    {
        // ���� �ð� �Ŀ� ���� ���߱�
        Invoke("StopGame", 1.0f);
    }
    private void StopGame()
    {
        //player_statas.gameObject.SetActive(false);
        //game_option.gameObject.SetActive(true);
        game_over.gameObject.SetActive(true);
        // ���� ���� (�ð� ����)
        Time.timeScale = 0;
    }
}

public interface IEssential_funtion
{
    //Text_pro, target�̸�, ������, ���� ����
    void TakeDamage(GameObject take_object, ref float Health, float damage, bool live, string type);
    //Text_pro, target�̸�, ������
    void Take_Hit_Text_Damage(GameObject damage_text_pro, GameObject target_objectstring, string text_position_target, float damage);
    //�¾��� ��� ���� ��ƼŬ ��ȣ(Audio_Manager Ŭ����)
    void Hit_Sound(SFX hit_Number);
    //��ƼŬ ������, ��ƼŬ���
    void Hit_Palticle(GameObject hitEffectPrefab, GameObject take_object);
}