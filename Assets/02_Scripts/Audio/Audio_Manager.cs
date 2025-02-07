using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    [Header("#BGM")]
    public AudioClip bgm_clip;
    public float bgm_volume;
    AudioSource bgm_player;

    [Header("#SFX")]
    public AudioClip[] sfc_clips;
    public float sfx_volume;
    public int channels;
    AudioSource[] sfx_players;
    int chanel_Index;

    public enum SFX { atk, cocked, hit, hit2 }

    private void Awake()
    {
        instance = this;
        Init();
    }
    void Init()
    {
        GameObject bgm_object = new GameObject("BGM_Player");
        bgm_object.transform.parent = transform;
        bgm_player = bgm_object.AddComponent<AudioSource>();
        bgm_player.playOnAwake = false;
        bgm_player.loop = true;
        bgm_player.volume = bgm_volume;
        bgm_player.clip = bgm_clip;

        GameObject sfxobject = new GameObject("SFX_Player");
        sfxobject.transform.parent = transform;
        sfx_players = new AudioSource[channels];

        for(int index = 0; index < sfx_players.Length; index++)
        {
            sfx_players[index] = sfxobject.AddComponent<AudioSource>();
            sfx_players[index].playOnAwake = false;
            sfx_players[index].volume = sfx_volume;
        }
    }
    public void PlaySfx(SFX sfx)
    {
        for(int index = 0; index < sfx_players.Length; index++)
        {
            int loopIndex = (index + chanel_Index) % sfx_players.Length;
            if (sfx_players[loopIndex].isPlaying) continue;

            chanel_Index = loopIndex;
            sfx_players[loopIndex].clip = sfc_clips[(int)sfx];
            sfx_players[loopIndex].Play();
            break;
        }
    }
}
