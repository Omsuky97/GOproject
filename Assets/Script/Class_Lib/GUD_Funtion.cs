using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUD_Funtion : MonoBehaviour
{
    //������ �̹���, ��ӿ���(flase�� �����ؾ���), �⺻�̹���, 2��� �� ������ �̹���
    public void Game_Speed_UP(Image HUD_speed_up, bool speed_up_paused, Sprite nomal_Image, Sprite speed_two_Image)
    {
        speed_up_paused = !speed_up_paused; // ���� ���� ����
        if (speed_up_paused)
        {
            Time.timeScale = 2;
            HUD_speed_up.sprite = speed_two_Image; // UI Image ����
        }
        else if (!speed_up_paused)
        {
            Time.timeScale = 1;
            HUD_speed_up.sprite = nomal_Image; // UI Image ����
        }
    }
}
