using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Audio_Manager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PoolManager pool;
    public Player player;

    [Header("## -- Relic_List_LV_1 -- ##")]
    public Relic_Data[] allRelics;

    [Header("## -- Game_System -- ##")]
    public short count_day = 1;
    public bool Waiting_Time_Type = false;

    [Header("## -- Game_Stage_List -- ##")]
    public int spawn_count;
    public float spawn_timer;
    public int kill_enemy_count;

    [Header("## -- Player_Statas_List -- ##")]
    public float player_hp;
    public float player_max_hp;
    public float bullet_damage;
    public float Attack_Delay;
    public int gold_count;

    [Header("## -- Player_Hit_HUD -- ##")]
    public Image Hit_Image; // 빨간색 UI 이미지

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

    [Header("## -- Game_Fade_UI -- ##")]
    public TextMeshProUGUI Day_Text;
    public float fadeDuration = 1f; // 페이드 시간
    public float Fade_TIme;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        Hit_Image.color = new Color(1, 0, 0, 0); // 처음엔 완전 투명
        ResetAllRelicLevels();
    }
    void ResetAllRelicLevels()
    {
        foreach (Relic_Data relic in allRelics)
        {
            if (relic != null)
            {
                relic.ResetLevel();
            }
        }
    }
    public void Stage_Level_UP()
    {
        if (kill_enemy_count == Spawner.instance.spawnData[count_day].spawnMaxCount)
        {
            //WaitAndFadeIn();
            spawn_count = 0;
            kill_enemy_count = 0;
            count_day += 1;
            Waiting_Time_Type = true;
            Day_Text.text = $"{count_day}일";
            StartCoroutine(FadeSequence(Day_Text));
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
    IEnumerator FadeInText(TextMeshProUGUI text)
    {
        if (text == null) yield break; // 텍스트가 null이면 중단

        text.gameObject.SetActive(true); // 페이드 인 전에 오브젝트 활성화
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        text.alpha = 1; // 최종적으로 완전히 보이게 설정
    }

    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        if (text == null) yield break; // 텍스트가 null이면 중단

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null;
        }

        text.alpha = 0;
        text.gameObject.SetActive(false); // 필요하면 텍스트 비활성화
    }

    // 인 → 2초 대기 → 아웃 실행 코루틴 추가
    IEnumerator FadeSequence(TextMeshProUGUI text)
    {
        yield return StartCoroutine(FadeInText(text)); // 먼저 페이드 인
        yield return new WaitForSeconds(Fade_TIme); // 2초 유지
        yield return StartCoroutine(FadeOutText(text)); // 그 후 페이드 아웃
    }
}



public interface IEssential_funtion
{
    //Text_pro, target이름, 데미지, 생존 여부
    void TakeDamage(GameObject take_object, ref float Health, float damage, bool live, string type, GameObject Did_Effect);
    //Text_pro, target이름, 데미지
    void Take_Hit_Text_Damage(GameObject damage_text_pro, GameObject target_objectstring, string text_position_target, float damage);
    //파티클 프리펩, 파티클대상
    void Hit_Palticle(GameObject hitEffectPrefab, GameObject take_object);
}