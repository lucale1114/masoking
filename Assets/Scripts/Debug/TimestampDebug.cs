using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static WaveData;

public class TimestampDebug : MonoBehaviour
{
    public TextMeshProUGUI inputFieldResult;
    public Button pauseButton;

    bool paused;

    private void Update()
    {
        if (Paused)
        {
            Timestamp = Int32.Parse(inputFieldResult.text);
        }
        else
        {
            inputFieldResult.text = Timestamp.ToString();
        }
    }

    public void Pause()
    {
        Paused = !Paused;
        if (paused)
        {
            pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Paused";
        }
        else
        {
            pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Unpaused";
        }
    }



}
