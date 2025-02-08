using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Lean.Gui;

public class Rellic_Slot : MonoBehaviour
{
    public LeanButton[] slot_button;
    public Image[] relic_slot_Image;
    public int[] non_relic_id;

    public int non_slot_count;

    public void Button_Setting(Image non_slot_image, int relic_id)
    {
        relic_slot_Image[non_slot_count].sprite = non_slot_image.sprite;
        non_relic_id[non_slot_count] = relic_id;
        non_slot_count += 1;
    }
}
