using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUD_Funtion : MonoBehaviour
{
    //변경할 이미지, 배속여부(flase로 시작해야함), 기본이미지, 2배속 시 변경할 이미지
    public void Game_Speed_UP(Image HUD_speed_up, bool speed_up_paused, Sprite nomal_Image, Sprite speed_two_Image)
    {
        speed_up_paused = !speed_up_paused; // 현재 상태 변경
        if (speed_up_paused)
        {
            Time.timeScale = 2;
            HUD_speed_up.sprite = speed_two_Image; // UI Image 변경
        }
        else if (!speed_up_paused)
        {
            Time.timeScale = 1;
            HUD_speed_up.sprite = nomal_Image; // UI Image 변경
        }
    }
}
