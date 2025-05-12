using System;
using System.Collections;
using System.Collections.Generic;
using Nguyen.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour, IWantToBeSaved
{
    [Header(" Elements ")]
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button privacyPolicyButton;
    [SerializeField] private Button askButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private GameObject creditsPanel;

    [Header(" Settings ")]
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [Header(" Data ")]
    private bool sfxState;
    private bool musicState;

    [Header(" Actions ")]
    public static Action<bool> OnSFXStateChanged;
    public static Action<bool> OnMusicStateChanged;

    private void Awake()
    {
        sfxButton.onClick.RemoveAllListeners();
        sfxButton.onClick.AddListener(SFXButtonCallback);

        musicButton.onClick.RemoveAllListeners();
        musicButton.onClick.AddListener(MusicButtonCallback);

        privacyPolicyButton.onClick.RemoveAllListeners();
        privacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonCallback);

        askButton.onClick.RemoveAllListeners();
        askButton.onClick.AddListener(AskButtonCallback);

        creditsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.AddListener(CreditsButtonCallback);

    }

    private void Start()
    {
        HideCreditsPanel();

        OnSFXStateChanged?.Invoke(sfxState);
        OnMusicStateChanged?.Invoke(musicState);
    }

    private void SFXButtonCallback()
    {
        sfxState = !sfxState;

        UpdateSFXVisuals();

        Save();

        OnSFXStateChanged?.Invoke(sfxState);
    }

    private void UpdateSFXVisuals()
    {
        sfxButton.image.color = sfxState ? onColor : offColor;
        sfxButton.GetComponentInChildren<TextMeshProUGUI>().text =
            sfxState ? "ON" : "OFF";
    }

    private void MusicButtonCallback()
    {
        musicState = !musicState;

        UpdateMusicVisuals();

        Save();

        OnMusicStateChanged?.Invoke(musicState);
    }

    private void UpdateMusicVisuals()
    {
        musicButton.image.color = musicState ? onColor : offColor;
        musicButton.GetComponentInChildren<TextMeshProUGUI>().text =
            musicState ? "ON" : "OFF";
    }

    private void PrivacyPolicyButtonCallback()
    {
        Application.OpenURL("");
    }

    private void AskButtonCallback()
    {
        string email = "acclinhtinh02@gmail.com";
        string subject = MyEscapeURL("Help");
        string body = MyEscapeURL("Hey, I need help !!!");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private string MyEscapeURL(string s)
    {
        return UnityWebRequest.EscapeURL(s).Replace("+", "%20");
    }

    private void CreditsButtonCallback()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCreditsPanel() => creditsPanel.SetActive(false);

    public void Load()
    {
        sfxState = true;
        musicState = true;

        if(SaveSystem.TryLoad(this, "SFX", out object sfxStateObject))
            sfxState = (bool)sfxStateObject;
        
        if(SaveSystem.TryLoad(this, "Music", out object musicStateObject))
            musicState = (bool)musicStateObject;

        UpdateSFXVisuals();
        UpdateMusicVisuals();
    }

    public void Save()
    {
        SaveSystem.Save(this, "SFX", sfxState);
        SaveSystem.Save(this, "Music", musicState);
    }
}
