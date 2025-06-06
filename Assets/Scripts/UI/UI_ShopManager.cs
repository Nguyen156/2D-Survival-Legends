using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopManager : MonoBehaviour
{
    [Header(" Player Stats Elements ")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private RectTransform playerStatsClosePanel;
    private Vector2 playerStatsOpenedPos;
    private Vector2 playerStatsClosedPos;
    
    [Header(" Inventory Elements ")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryClosePanel;
    private Vector2 inventoryOpenedPos;
    private Vector2 inventoryClosedPos;

    [Header(" Item Info Elements ")]
    [SerializeField] private RectTransform itemInfoSlidePanel;
    private Vector2 itemInfoOpenedPos;
    private Vector2 itemInfoClosedPos;

    private IEnumerator Start()
    {
        yield return null;

        SetupPlayerStatsPanel();
        SetupInventoryPanel();
        SetupItemInfoPanel();
    }

    #region Player Stats Panel

    private void SetupPlayerStatsPanel()
    {
        float width = Screen.width / (4 * playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.Width(x: width);

        playerStatsOpenedPos = playerStatsPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenedPos + Vector2.left * width;

        playerStatsPanel.anchoredPosition = playerStatsClosedPos;

        HidePlayerStats();
    }

    public void ShowPlayerStats()
    {
        playerStatsPanel.gameObject.SetActive(true);
        playerStatsClosePanel.gameObject.SetActive(true);
        playerStatsClosePanel.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsOpenedPos, .5f).setEase(LeanTweenType.easeInCubic);

        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.alpha(playerStatsClosePanel, .8f, .5f).setRecursive(false);
    }

    public void HidePlayerStats()
    {
        //;

        playerStatsClosePanel.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsClosedPos, .5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => playerStatsPanel.gameObject.SetActive(false));

        LeanTween.cancel(playerStatsClosePanel);
        LeanTween.alpha(playerStatsClosePanel, 0, .5f)
            .setRecursive(false)
            .setOnComplete(() => playerStatsClosePanel.gameObject.SetActive(false));
    }

    #endregion

    #region Inventory Panel
    private void SetupInventoryPanel()
    {
        float width = Screen.width / (4 * inventoryPanel.lossyScale.x);
        inventoryPanel.offsetMin = inventoryPanel.offsetMin.Width(x: -width);

        inventoryOpenedPos = inventoryPanel.anchoredPosition;
        inventoryClosedPos = inventoryOpenedPos + Vector2.right * width;

        inventoryPanel.anchoredPosition = inventoryClosedPos;

        HideInventory(false);
    }

    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryClosePanel.gameObject.SetActive(true);

        inventoryClosePanel.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryOpenedPos, .5f).setEase(LeanTweenType.easeInCubic);

        LeanTween.cancel(inventoryClosePanel);
        LeanTween.alpha(inventoryClosePanel, .8f, .5f).setRecursive(false);
    }
    public void HideInventory(bool hideItemInfo = true)
    {
        inventoryClosePanel.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryClosedPos, .5f)
            .setEase(LeanTweenType.easeOutCubic)
            .setOnComplete(() => inventoryPanel.gameObject.SetActive(false));

        LeanTween.cancel(inventoryClosePanel);
        LeanTween.alpha(inventoryClosePanel, 0, .5f)
            .setRecursive(false)
            .setOnComplete(() => inventoryClosePanel.gameObject.SetActive(false));

        if(hideItemInfo)
            HideItemInfo();
    }
    #endregion

    public void SetupItemInfoPanel()
    {
        float height = Screen.height / (2 * itemInfoSlidePanel.lossyScale.y);
        itemInfoSlidePanel.offsetMax = itemInfoSlidePanel.offsetMax.Width(y: height);

        itemInfoOpenedPos = itemInfoSlidePanel.anchoredPosition;
        itemInfoClosedPos = itemInfoOpenedPos + Vector2.down * height;

        itemInfoSlidePanel.anchoredPosition = itemInfoClosedPos;
    }

    public void ShowItemInfo()
    {
        LeanTween.cancel(itemInfoSlidePanel);
        LeanTween.move(itemInfoSlidePanel, itemInfoOpenedPos, .5f)
            .setEase(LeanTweenType.easeOutCubic);
    }

    public void HideItemInfo()
    {
        LeanTween.cancel(itemInfoSlidePanel);
        LeanTween.move(itemInfoSlidePanel, itemInfoClosedPos, .5f)
            .setEase(LeanTweenType.easeInCubic);
    }
}
