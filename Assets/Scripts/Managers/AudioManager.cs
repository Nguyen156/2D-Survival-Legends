using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header(" Audio Source ")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    private int bgmIndex;

    public bool IsSFXOn {  get; private set; }
    public bool IsMusicOn {  get; private set; }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SettingsManager.OnSFXStateChanged += SFXStateChangedCallback;
        SettingsManager.OnMusicStateChanged += MusicStateChangedCallback;

        if (bgm.Length <= 0)
            return;

        PlayMusicIfNeeded();
    }

    private void OnDestroy()
    {
        SettingsManager.OnSFXStateChanged -= SFXStateChangedCallback;
        SettingsManager.OnMusicStateChanged -= MusicStateChangedCallback;
    }

    public void PlayMusicIfNeeded()
    {
        if (!IsMusicOn)
        {
            bgm[bgmIndex].Stop();
            return;
        }

        if (bgm[bgmIndex].isPlaying == false)
            PlayRandomBGM();
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int bgmToPlay)
    {
        if (bgm.Length <= 0)
            return;

        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }

        bgmIndex = bgmToPlay;
        bgm[bgmToPlay].Play();
    }

    public void PlaySFX(int sfxToPlay, bool randomPitch = true)
    {
        if (sfxToPlay >= sfx.Length)
            return;

        if(!IsSFXOn)
            return;

        if (randomPitch)
            sfx[sfxToPlay].pitch = Random.Range(.9f, 1.1f);

        sfx[sfxToPlay].Play();
    }

    private void SFXStateChangedCallback(bool sfxState)
    {
        IsSFXOn = sfxState;
    }

    private void MusicStateChangedCallback(bool musicState)
    {
        IsMusicOn = musicState;

        PlayMusicIfNeeded();

    }
}
