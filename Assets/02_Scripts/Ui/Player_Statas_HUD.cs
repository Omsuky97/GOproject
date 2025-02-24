using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Statas_HUD : MonoBehaviour
{
    public enum InfoType { None, Health, BossHealth, Day, Gold }
    public InfoType type;

    TextMeshProUGUI cold_count;
    TextMeshProUGUI day_Text;
    Slider paleyr_hp_Slider;
    Slider boss_hp_Slider;

    private void Start()
    {
        switch (type)
        {
            case InfoType.Health:
                paleyr_hp_Slider = GetComponent<Slider>();
                break;
            case InfoType.BossHealth:
                boss_hp_Slider = GetComponent<Slider>();
                break;
            case InfoType.Day:
                day_Text = GetComponent<TextMeshProUGUI>();
                break;
            case InfoType.Gold:
                cold_count = GetComponent<TextMeshProUGUI>();
                break;
        }
    }
    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                float hp = GameManager.Instance.player_hp;
                float max_hp = GameManager.Instance.player_max_hp;
                paleyr_hp_Slider.value = hp / max_hp;
                break;
            case InfoType.BossHealth:
                float boss_hp = Enumy_Monster.Instance.Monster_Hp;
                float boss_max_hp = Enumy_Monster.Instance.Monster_MaxHp;
                boss_hp_Slider.value = boss_hp / boss_max_hp;
                break;
            case InfoType.Day:
                day_Text.text = $"D-day {GameManager.Instance.count_day}";
                break;
            case InfoType.Gold:
                cold_count.text = $"{GameManager.Instance.gold_count}";
                break;

        }
    }


}
