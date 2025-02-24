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


    public AudioSource[] SFX_Monster_Hit_Source;
    public AudioClip SFX_Monster_Hit_Clip;


    public enum SFX { atk, cocked, hit, hit2 }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #region Master
    public void Set_Master_Volume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
    public void Set_SFX_Volume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
    public void Set_BGM_Volume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
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
    public void Get_Monster_Hit_Sound(int count)
    {
        SFX_Monster_Hit_Source[count].PlayOneShot(SFX_Monster_Hit_Clip);
    }
}
