using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header(" Audio Source ")]
    [SerializeField] private AudioSource[] sfx;

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
    }

    private void OnDestroy()
    {
        SettingsManager.OnSFXStateChanged -= SFXStateChangedCallback;
        SettingsManager.OnMusicStateChanged -= MusicStateChangedCallback;
    }

    public void PlaySFX(int sfxToPlay)
    {
        if (sfxToPlay >= sfx.Length)
            return;

        if(!IsSFXOn)
            return;

        sfx[sfxToPlay].Play();
    }

    private void SFXStateChangedCallback(bool sfxState)
    {
        IsSFXOn = sfxState;
    }

    private void MusicStateChangedCallback(bool musicState)
    {
        IsMusicOn = musicState;
    }
}
