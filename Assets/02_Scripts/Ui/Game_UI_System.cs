using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using static UnityEngine.GraphicsBuffer;
using Lean.Gui;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.IO;

public class Game_UI_System : MonoBehaviour
{
    public float selectionTimeout = 30f; // 자동 선택 시간 (초)

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
    public Equip rellic_equip;
    public GameObject Active_Equip_Relic_Explain;
    private bool Relic_Gacha_UI_paused = false; // UI 상태 추적

    [Header("## -- Game_Fade_UI -- ##")]
    public TextMeshProUGUI Day_Text;
    public float fadeDuration = 1f; // 페이드 시간

    [Header("## -- Game_Sound_UI -- ##")]
    public GameObject Option_UI;

    [Header("## -- Game_Sound -- ##")]
    public Slider Game_Master_Sound_Slider;
    public Slider Game_SFX_Sound_Slider;
    public Slider Game_BGM_Sound_Slider;
    public TextMeshProUGUI Master_Text;
    public TextMeshProUGUI SFX_Text;
    public TextMeshProUGUI BGM_Text;

    private void Awake()
    {
        Relic_Gacha_UI.SetActive(false);
        Active_Equip_Relic_Explain.SetActive(false);
        Relic_Gacha.SetActive(false);
    }
    private void Start()
    {
        Game_Master_Sound_Slider.value = PlayerPrefs.GetFloat("Master", 5f);
        Game_SFX_Sound_Slider.value = PlayerPrefs.GetFloat("SFX", 5f);
        Game_BGM_Sound_Slider.value = PlayerPrefs.GetFloat("BGM", 5f);

        UpdatMasterText(Game_Master_Sound_Slider.value);
        UpdateSFXText(Game_SFX_Sound_Slider.value);
        UpdateBGMText(Game_BGM_Sound_Slider.value);

        Game_Master_Sound_Slider.onValueChanged.RemoveAllListeners();
        Game_SFX_Sound_Slider.onValueChanged.RemoveAllListeners();
        Game_BGM_Sound_Slider.onValueChanged.RemoveAllListeners();

        // 오디오 매니저에 슬라이더 값 전달
        Game_Master_Sound_Slider.onValueChanged.AddListener(Audio_Manager.instance.Set_Master_Volume);
        Game_SFX_Sound_Slider.onValueChanged.AddListener(Audio_Manager.instance.Set_SFX_Volume);
        Game_BGM_Sound_Slider.onValueChanged.AddListener(Audio_Manager.instance.Set_BGM_Volume);

        Game_Master_Sound_Slider.onValueChanged.AddListener(UpdatMasterText);
        Game_SFX_Sound_Slider.onValueChanged.AddListener(UpdateSFXText);
        Game_BGM_Sound_Slider.onValueChanged.AddListener(UpdateBGMText);

        StartCoroutine(FadeOutText(Day_Text));
    }
    void UpdatMasterText(float value)
    {
        int percentage = Mathf.RoundToInt(value * 20); // 0~1 값을 0~100으로 변환
        Master_Text.text = percentage + "%"; // 퍼센트로 표시
    }
    void UpdateSFXText(float value)
    {
        int percentage = Mathf.RoundToInt(value * 20); // 0~1 값을 0~100으로 변환
        SFX_Text.text = percentage + "%"; // 퍼센트로 표시
    }
    void UpdateBGMText(float value)
    {
        int percentage = Mathf.RoundToInt(value * 20); // 0~1 값을 0~100으로 변환
        BGM_Text.text = percentage + "%"; // 퍼센트로 표시
    }
    public void Game_Menu()
    {
        if (menu_ui_paused) menu_ui_paused = false;
        else if (!menu_ui_paused) menu_ui_paused = true;
        menu_back_gournd.SetActive(menu_ui_paused);
        pause_menu.SetActive(menu_ui_paused); // UI 활성화/비활성화
        Time.timeScale = menu_ui_paused ? 0 : 1;
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

    public void Game_Sound_Option()
    {
        Time.timeScale = 0.0f;
    }
    public void Game_Sound_Option_Exit()
    {
        Time.timeScale = 1.0f;
    }
    public void Game_Exit()
    {
        Time.timeScale = 0.0f;
    }
    public void Game_Exit_Button_Exit()
    {
        Time.timeScale = 1.0f;
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
    //Btn_Relic_Gacha
    public void Open_Gacha()
    {
        if (!Relic_Gacha_UI_paused)
        {
            if (GameManager.Instance.gold_count >= 100)
            {
                Relic_Gacha_UI_paused = true;
                Relic_Gacha_UI.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
    }
    //Btn_Open_Relic_Gacha
    public void Open_Relic_Gacha()
    {
        if (!Relic_Gacha_UI_paused)
        {
            Relic_Gacha.SetActive(true);
            Relic_View.SetActive(false);
        }
    }
    //Btn_Open_Relic_View
    public void Open_Relic_View()
    {
        if (!Relic_Gacha_UI_paused)
        {
            Relic_Gacha.SetActive(false);
            Relic_View.SetActive(true);
        }
    }
    //가챠 후 선택
    public void Select_Relic(Button clickedButton)
    {
        Image button_Image = clickedButton.GetComponentsInChildren<Image>()[1];
        Button_Count button_number = clickedButton.GetComponentsInChildren<Button_Count>()[0];  //3개중 하나를 눌렀을 경우 값 전달 (0, 1, 2)
        for (int button_count = 0; button_count < Rellic_Slot.slot_button.Length; button_count++)
        {
            if (Rellic_Slot.non_relic_id[button_count] != 0 && (Relic_Item.data_id[button_number.button_num] == Rellic_Slot.non_relic_id[button_count]))
            {
                int level = Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_count]).Relics_Lv;
                if (level >= 1 && level < 5)
                {
                    Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_count]).Relics_Lv += 1;
                    rellic_equip.Relic_Equip_Update(Rellic_Slot.non_relic_id[button_count]);
                    break;
                }
            }
            else if (Rellic_Slot.non_relic_id[button_count] == 0)
            {
                Rellic_Slot.Button_Setting(button_Image, Relic_Item.data_id[button_number.button_num]);
                break;
            }
        }
        GameManager.Instance.gold_count -= 200;
        Relic_Gacha_UI.SetActive(false);
        Relic_Gacha_UI_paused = false;
        Time.timeScale = 1.0f;
    }
    // 유물에서 장착할 렐릭 창 표시
    public void Equip_Relic(LeanButton clickedButton)
    {
        Rellic_Slot_Count button_slot_num = clickedButton.GetComponentsInChildren<Rellic_Slot_Count>()[0];
        if (Rellic_Slot.non_relic_id[button_slot_num.slot_num] == 0) return;
        Relic_Gacha_UI_paused = true;
        Active_Equip_Relic_Explain.SetActive(true);
        Image button_Image = clickedButton.GetComponentsInChildren<Image>()[2];
        Equip_Relic_Explain.Equip_Relic_Explain_Panel(button_Image, Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_slot_num.slot_num]).Relics_Name, Relic_Manager.GetRelicById(Rellic_Slot.non_relic_id[button_slot_num.slot_num]).item_desc, Rellic_Slot.non_relic_id[button_slot_num.slot_num]);
        Relic_Gacha_UI.SetActive(false);
        Relic_Gacha_UI_paused = false;
    }
    // 유물 장착
    public void Btn_Equip(Button clickedButton)
    {
        Transform parentTransform = clickedButton.transform.parent;
        //부모의 자식들 중 "main_Image"를 이름으로 검색
        Transform mainImageTransform = parentTransform.Find("main_Image_Tile");
        Image equip_image = mainImageTransform.GetComponentsInChildren<Image>()[0];
        Equip_Relic_Explain[] equip_id_nums = clickedButton.GetComponentsInParent<Equip_Relic_Explain>();
        Equip_Relic_Explain equip_id_num = equip_id_nums[0]; // 가장 가까운 부모
        rellic_equip.Relic_Equip(equip_image, equip_id_num.relic_id_num);
        Active_Equip_Relic_Explain.SetActive(false);
        Relic_Gacha_UI_paused = false;
    }
    public void Btn_Equip_Exit(Button clickedButton)
    {
        int btn_num = clickedButton.GetComponent<Relic_Slot_Num>().Button_Bum;
        int btn_relic_num = clickedButton.GetComponentInParent<Relic_Exit_Button>().relic_Num[btn_num];
        rellic_equip.Relic_Equip_Exit(btn_relic_num);
        //buttonImages[1].sprite = null;
    }
    public void ChangeScene( )
    {
        string exePath = Application.dataPath;
        exePath = Path.Combine(Application.dataPath, "../", Application.productName + ".exe");                                         
        exePath = Path.Combine(Application.dataPath, "../", Application.productName);
        if (File.Exists(exePath))
        {
            // 현재 실행 중인 게임의 새 프로세스를 시작
            Process.Start(exePath);

            // 현재 게임 종료
            Application.Quit();
        }
    }
}
