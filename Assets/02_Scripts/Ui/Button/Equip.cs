using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Lean.Gui;
using TMPro;

public class Equip : MonoBehaviour
{
    public List<Relic_Data> Relic = new List<Relic_Data>();
    public int slot_count;
    public int[] relic_id_num;
    public Image[] equip_Image;

    public void Relic_Equip(Image relic_image, int relic_id)
    {
        relic_id_num[slot_count] = relic_id;
        equip_Image[slot_count].sprite = relic_image.sprite;
        for(int i = 0; i < Relic.Count; i++)
        {
            if(Relic[i].Relics_id == relic_id_num[slot_count])
            {
                GameManager.Instance.bullet_damage += Relic[i].Bullet_Pow_UP[Relic[i].Relics_Lv];
            }
        }
        slot_count += 1;
    }
}
