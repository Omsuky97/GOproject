using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;
    public AudioMixer audioMixer;

    [Header("#BGM")]
    public AudioSource BGM_Source;
    public AudioClip BGM_Clip;

    [Header("#SFX")]
    public AudioSource SFX_Attack_Source;
    public AudioClip SFX_Attack_Clip;

    public AudioSource SFX_Player_Hit_Source;
    public AudioClip SFX_Player_Hit_Clip;


    public AudioSource SFX_Monster_Hit_Source;
    public AudioClip SFX_Monster_Hit_Clip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #region Master
    public void Set_Master_Volume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Master", volume); // 저장
        PlayerPrefs.Save();
    }
    public void Set_SFX_Volume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume); // 저장
        PlayerPrefs.Save();
    }
    public void Set_BGM_Volume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGM", volume); // 저장
        PlayerPrefs.Save();
    }
    #endregion
    public void GetAttack_Sound()
    {
        SFX_Attack_Source.PlayOneShot(SFX_Attack_Clip);
    }
    public void Get_Player_Hit_Sound()
    {
        SFX_Player_Hit_Source.PlayOneShot(SFX_Player_Hit_Clip);
    }
    public void Get_Monster_Hit_Sound()
    {
        SFX_Monster_Hit_Source.PlayOneShot(SFX_Monster_Hit_Clip);
    }
    public void Get_BGM_Sound()
    {
        BGM_Source.PlayOneShot(BGM_Clip);
    }
    private void Load_Volume()
    {
        float masterVol = PlayerPrefs.GetFloat("Master", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFX", 1f);
        float bgmVol = PlayerPrefs.GetFloat("BGM", 1f);

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVol) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVol) * 20);
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(bgmVol) * 20);
    }
}
