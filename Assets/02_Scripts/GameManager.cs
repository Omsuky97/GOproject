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
    public Image hit_color_Image; // UI 이미지 참조
    public Color hit_Color = new Color(1f, 0f, 0f, 0.3f); // 빨간색 반투명
    public Color originalColor; // 원래 색상
    public float colorFadeSpeed = 1f; // 색상 페이드 속도

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
        // 일정 시간 후에 게임 멈추기
        Invoke("StopGame", 1.0f);
    }
    private void StopGame()
    {
        //player_statas.gameObject.SetActive(false);
        //game_option.gameObject.SetActive(true);
        game_over.gameObject.SetActive(true);
        // 게임 멈춤 (시간 정지)
        Time.timeScale = 0;
    }
}

public interface IEssential_funtion
{
    //Text_pro, target이름, 데미지, 생존 여부
    void TakeDamage(GameObject take_object, ref float Health, float damage, bool live, string type);
    //Text_pro, target이름, 데미지
    void Take_Hit_Text_Damage(GameObject damage_text_pro, GameObject target_objectstring, string text_position_target, float damage);
    //맞았을 경우 나올 파티클 번호(Audio_Manager 클래스)
    void Hit_Sound(SFX hit_Number);
    //파티클 프리펩, 파티클대상
    void Hit_Palticle(GameObject hitEffectPrefab, GameObject take_object);
}