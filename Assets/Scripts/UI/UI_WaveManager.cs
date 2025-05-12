using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WaveManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveCompletedText;
    private bool hasPlaySound;

    public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdatTimerText(string timerString) => timerText.text = timerString;

    public void UpdateWaveCompltedText(string text) => waveCompletedText.text = text;

    public void ChangeTimerTextColor(int timer)
    {
        timerText.color = Color.white;

        if (timer <= 6)
        {
            timerText.color = Color.red;

            if(!hasPlaySound)
                StartCoroutine(IEPlaySound());
        }
       
    }

    private IEnumerator IEPlaySound()
    {
        hasPlaySound = true;
        AudioManager.instance.PlaySFX(5);

        yield return new WaitForSeconds(1);

        hasPlaySound = false;
    }
}
