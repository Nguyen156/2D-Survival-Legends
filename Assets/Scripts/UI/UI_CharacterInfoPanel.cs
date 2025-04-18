using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterInfoPanel : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject priceContainer;
    [SerializeField] private Transform statsParent;

    [field:SerializeField] public Button Button { get; private set; }

    public void Setup(CharacterDataSO characterData, bool unlocked)
    {
        nameText.text = characterData.Name;
        priceText.text = characterData.Price.ToString();

        priceContainer.SetActive(!unlocked);

        StatContainerManager.GenerateStatContainers(characterData.NonNeutralStats, statsParent);
    }
}
