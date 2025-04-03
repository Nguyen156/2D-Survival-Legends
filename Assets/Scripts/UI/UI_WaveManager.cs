using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WaveManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;

    public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdatTimerText(string timerString) => timerText.text = timerString;
}
