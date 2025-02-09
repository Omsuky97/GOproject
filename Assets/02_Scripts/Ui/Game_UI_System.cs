using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using static UnityEngine.GraphicsBuffer;
using Lean.Gui;

public class Game_UI_System : MonoBehaviour
{
    public float selectionTimeout = 30f; // �ڵ� ���� �ð� (��)

    [Header("## -- Menu_UI -- ##")]
    public GameObject menu_back_gournd;
    public GameObject pause_menu;
    private bool menu_ui_paused = false; // UI ���� ����

    [Header("## -- Game_Draw -- ##")]
    public GameObject draw_menu;
    private bool draw_paused = false; // UI ���� ����

    [Header("## -- Game_Relic -- ##")]
    public GameObject Relic_Gacha_UI;
    public GameObject Relic_View;
    public GameObject Relic_Gacha;
    public Rellic_Slot Rellic_Slot;
    public Relic_Item Relic_Item;
    public Equip_Relic_Explain Equip_Relic_Explain;
    public Relic_Manager Relic_Manager;
    public Equip rellic_equip;

    public GameObject Active_Equip_Relic_Explain;

    private bool Relic_Gacha_UI_paused = false; // UI ���� ����

    private void Awake()
    {
        Relic_Gacha_UI.SetActive(false);
        Active_Equip_Relic_Explain.SetActive(false);
    }
    public void Game_Menu()
    {
        if (menu_ui_paused) menu_ui_paused = false;
        else if (!menu_ui_paused) menu_ui_paused = true;
        menu_back_gournd.SetActive(menu_ui_paused);
        pause_menu.SetActive(menu_ui_paused); // UI Ȱ��ȭ/��Ȱ��ȭ
        Time.timeScale = menu_ui_paused ? 0 : 1;
    }
    public void Game_Option()
    {
        Debug.Log("�ϴ� �ɼ�");
    }
    public void Game_Exit()
    {
        Debug.Log("�ϴ� ����");
    }
    public void Game_Drow()
    {
        draw_paused = true;
        draw_menu.SetActive(draw_paused); // UI Ȱ��ȭ/��Ȱ��ȭ
    }
    public void Game_Drow_Exit()
    {
        draw_paused = false;
        draw_menu.SetActive(draw_paused); // UI Ȱ��ȭ/��Ȱ��ȭ
    }
    public void Open_Gacha()
    {
        if (!Relic_Gacha_UI_paused)
        {
            Relic_Gacha_UI_paused = true;
            Relic_Gacha_UI.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    public void Open_Relic_Gacha()
    {
        if (!Relic_Gacha_UI_paused)
        {
            Relic_Gacha.SetActive(true);
            Relic_View.SetActive(false);
        }
    }
    public void Open_Relic_View()
    {
        if (!Relic_Gacha_UI_paused)
        {
            Relic_Gacha.SetActive(false);
            Relic_View.SetActive(true);
        }
    }
    public void Select_Relic(Button clickedButton)
    {
        Image button_Image = clickedButton.GetComponentsInChildren<Image>()[2];
        Button_Count button_number = clickedButton.GetComponentsInChildren<Button_Count>()[0];  //3���� �ϳ��� ������ ��� �� ���� (0, 1, 2)
        for (int button_count = 0; button_count < Rellic_Slot.slot_button.Length; button_count++)
        {
            if (Rellic_Slot.non_relic_id[button_count] != 0 && (Relic_Item.data_id[button_number.button_num] == Rellic_Slot.non_relic_id[button_count]))
            {
                int level = Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_count]).Relics_Lv;
                if (level >= 1 && level < 5)
                {
                    Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_count]).Relics_Lv += 1;
                    break;
                }
            }
            else if (Rellic_Slot.non_relic_id[button_count] == 0)
            {
                Rellic_Slot.Button_Setting(button_Image, Relic_Item.data_id[button_number.button_num]);
                break;
            }
        }
        Relic_Gacha_UI.SetActive(false);
        Relic_Gacha_UI_paused = false;
        Time.timeScale = 1.0f;
    }
    public void Equip_Relic(LeanButton clickedButton)
    {
        Relic_Gacha_UI_paused = true;
        Rellic_Slot_Count button_slot_num = clickedButton.GetComponentsInChildren<Rellic_Slot_Count>()[0];
        if (Rellic_Slot.non_relic_id[button_slot_num.slot_num] == 0) return;
        Active_Equip_Relic_Explain.SetActive(true);
        Image button_Image = clickedButton.GetComponentsInChildren<Image>()[2];
        Equip_Relic_Explain.Equip_Relic_Explain_Panel(button_Image, Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_slot_num.slot_num]).Relics_Name, Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_slot_num.slot_num]).item_desc, Rellic_Slot.non_relic_id[button_slot_num.slot_num]);
        Relic_Gacha_UI.SetActive(false);
        Relic_Gacha_UI_paused = false;
    }
    public void Btn_Equip(Button clickedButton)
    {
        Transform parentTransform = clickedButton.transform.parent;
        // �θ��� �ڽĵ� �� "main_Image"�� �̸����� �˻�
        Transform mainImageTransform = parentTransform.Find("main_Image_Tile");
        Image equip_image = mainImageTransform.GetComponentsInChildren<Image>()[1];
        Equip_Relic_Explain[] equip_id_nums = clickedButton.GetComponentsInParent<Equip_Relic_Explain>();
        Equip_Relic_Explain equip_id_num = equip_id_nums[0]; // ���� ����� �θ�
        rellic_equip.Relic_Equip(equip_image, equip_id_num.relic_id_num);
        Active_Equip_Relic_Explain.SetActive(false);
        Relic_Gacha_UI_paused = false;
    }
    public void Btn_Reset(Button clickedButton)
    {
        Transform parentTransform = clickedButton.transform.parent;
        // �θ��� �ڽĵ� �� "main_Image"�� �̸����� �˻�
        Transform mainImageTransform = parentTransform.Find("main_Image_Tile");
        Image equip_image = mainImageTransform.GetComponentsInChildren<Image>()[1];
        Equip_Relic_Explain[] equip_id_nums = clickedButton.GetComponentsInParent<Equip_Relic_Explain>();
        Equip_Relic_Explain equip_id_num = equip_id_nums[0]; // ���� ����� �θ�
        rellic_equip.Relic_Equip(equip_image, equip_id_num.relic_id_num);
        Active_Equip_Relic_Explain.SetActive(false);
        Relic_Gacha_UI_paused = false;
    }
}
