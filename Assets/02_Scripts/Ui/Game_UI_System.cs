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
    [Header("## -- Menu_UI -- ##")]
    public GameObject menu_back_gournd;
    public GameObject pause_menu;
    private bool menu_ui_paused = false; // UI 상태 추적

    [Header("## -- Game_Draw -- ##")]
    public GameObject draw_menu;
    private bool draw_paused = false; // UI 상태 추적

    [Header("## -- Game_Relic -- ##")]
    public GameObject Relic_Gacha_UI;
    public GameObject Relic_View;
    public GameObject Relic_Gacha;
    public Rellic_Slot Rellic_Slot;
    public Relic_Item Relic_Item;
    public Equip_Relic_Explain Equip_Relic_Explain;
    public Relic_Manager Relic_Manager;

    private void Awake()
    {
        Relic_Gacha_UI.SetActive(false);
    }
    public void Game_Menu()
    {
        if (menu_ui_paused) menu_ui_paused = false;
        else if (!menu_ui_paused) menu_ui_paused = true;
        menu_back_gournd.SetActive(menu_ui_paused);
        pause_menu.SetActive(menu_ui_paused); // UI 활성화/비활성화
        Time.timeScale = menu_ui_paused ? 0 : 1;
    }
    public void Game_Option()
    {
        Debug.Log("일단 옵션");
    }
    public void Game_Exit()
    {
        Debug.Log("일단 나가");
    }
    public void Game_Drow()
    {
        draw_paused = true;
        draw_menu.SetActive(draw_paused); // UI 활성화/비활성화
    }
    public void Game_Drow_Exit()
    {
        draw_paused = false;
        draw_menu.SetActive(draw_paused); // UI 활성화/비활성화
    }
    public void Open_Gacha()
    {
        Relic_Gacha_UI.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void Open_Relic_Gacha()
    {
        Relic_Gacha.SetActive(true);
        Relic_View.SetActive(false);
    }
    public void Open_Relic_View()
    {
        Relic_Gacha.SetActive(false);
        Relic_View.SetActive(true);
    }
    public void Select_Relic(Button clickedButton)
    {
        Image button_Image = clickedButton.GetComponentsInChildren<Image>()[2];
        Button_Count button_number = clickedButton.GetComponentsInChildren<Button_Count>()[0];
        Rellic_Slot.Button_Setting(button_Image, Relic_Item.data_id[button_number.button_num]);
        Relic_Gacha_UI.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Equip_Relic(LeanButton clickedButton)
    {
        Rellic_Slot_Count button_slot_num = clickedButton.GetComponentsInChildren<Rellic_Slot_Count>()[0];
        Image button_Image = clickedButton.GetComponentsInChildren<Image>()[2];
        Equip_Relic_Explain.Equip_Relic_Explain_Panel(button_Image, Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_slot_num.slot_num]).Relics_Name, Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_slot_num.slot_num]).item_desc);
        Relic_Gacha_UI.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
