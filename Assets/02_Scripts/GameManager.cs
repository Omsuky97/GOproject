using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float Attack_Delay;
    public int gold_count;

    [Header("## -- Player_Hit_HUD -- ##")]
    public Image Hit_Image; // ������ UI �̹���

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

    [Header("## -- Fade_UI -- ##")]
    public TextMeshProUGUI Fade_Text; // ���̵� ȿ���� ������ Text
    public float Fade_Duration = 1.0f; // ���̵� �ð�

    void Awake()
    {
        Instance = this;
        Hit_Image.color = new Color(1, 0, 0, 0); // ó���� ���� ����
    }
    private void Start()
    {
        Color textColor = Fade_Text.color;
        Fade_Text.color = textColor;

        WaitAndFadeIn();

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
        Debug.Log("��� Relic ������ 1�� �ʱ�ȭ��!");
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

    IEnumerator WaitAndFadeIn()
    {
        yield return new WaitForSeconds(2f); // 2�� ���
        FadeOut();
    }
    public void FadeIn()
    {
        StartCoroutine(FadeText(Fade_Text, Fade_Duration, 0, 1)); // ���� �� ����
    }

    public void FadeOut()
    {
        StartCoroutine(FadeText(Fade_Text, Fade_Duration, 0, 1)); // ���� �� ����
    }
    IEnumerator FadeText(TextMeshProUGUI Text, float Duration, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color textColor = Text.color; // ���� �ؽ�Ʈ ���� ����

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / Duration);
            Text.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        // ���� ���� ����
        Text.color = new Color(textColor.r, textColor.g, textColor.b, endAlpha);
    }
}



public interface IEssential_funtion
{
    //Text_pro, target�̸�, ������, ���� ����
    void TakeDamage(GameObject take_object, ref float Health, float damage, bool live, string type);
    //Text_pro, target�̸�, ������
    void Take_Hit_Text_Damage(GameObject damage_text_pro, GameObject target_objectstring, string text_position_target, float damage);
    //��ƼŬ ������, ��ƼŬ���
    void Hit_Palticle(GameObject hitEffectPrefab, GameObject take_object);
}