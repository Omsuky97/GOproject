using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equip_Relic_Explain : MonoBehaviour
{
    public Image relic_equip_Image;
    public TextMeshProUGUI relic_equip_name;
    public TextMeshProUGUI relic_equip_desc;

    public void Equip_Relic_Explain_Panel(Image relic_image, string relic_name, string relic_desc)
    {
        relic_equip_Image.sprite = relic_image.sprite;
        relic_equip_name.text = relic_name;
        relic_equip_desc.text = relic_desc;
}
}
