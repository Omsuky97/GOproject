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

    [Header("## -- Game_Fade_UI -- ##")]
    public TextMeshProUGUI Day_Text;
    public float fadeDuration = 1f; // ���̵� �ð�
    public float Fade_TIme;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        Hit_Image.color = new Color(1, 0, 0, 0); // ó���� ���� ����
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
            Day_Text.text = $"{count_day}��";
            StartCoroutine(FadeSequence(Day_Text));
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
    IEnumerator FadeInText(TextMeshProUGUI text)
    {
        if (text == null) yield break; // �ؽ�Ʈ�� null�̸� �ߴ�

        text.gameObject.SetActive(true); // ���̵� �� ���� ������Ʈ Ȱ��ȭ
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        text.alpha = 1; // ���������� ������ ���̰� ����
    }

    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        if (text == null) yield break; // �ؽ�Ʈ�� null�̸� �ߴ�

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null;
        }

        text.alpha = 0;
        text.gameObject.SetActive(false); // �ʿ��ϸ� �ؽ�Ʈ ��Ȱ��ȭ
    }

    // �� �� 2�� ��� �� �ƿ� ���� �ڷ�ƾ �߰�
    IEnumerator FadeSequence(TextMeshProUGUI text)
    {
        yield return StartCoroutine(FadeInText(text)); // ���� ���̵� ��
        yield return new WaitForSeconds(Fade_TIme); // 2�� ����
        yield return StartCoroutine(FadeOutText(text)); // �� �� ���̵� �ƿ�
    }
}



public interface IEssential_funtion
{
    //Text_pro, target�̸�, ������, ���� ����
    void TakeDamage(GameObject take_object, ref float Health, float damage, bool live, string type, GameObject Did_Effect);
    //Text_pro, target�̸�, ������
    void Take_Hit_Text_Damage(GameObject damage_text_pro, GameObject target_objectstring, string text_position_target, float damage);
    //��ƼŬ ������, ��ƼŬ���
    void Hit_Palticle(GameObject hitEffectPrefab, GameObject take_object);
}