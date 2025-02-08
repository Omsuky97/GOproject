using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Game_UI_System : MonoBehaviour
{
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


    private void Awake()
    {
        Relic_Gacha_UI.SetActive(false);
    }
    //[Header("## -- GUI_Anim -- ##")]


    //    void Update()
    //    {
    //        if (Input.GetButtonDown("Game_Menu_Stop"))
    //        {
    //            Game_Menu();
    //        }
    //    }
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
    public void Select_Relic()
    {

    }
}
