using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static WaveData;

public class TimestampDebug : MonoBehaviour
{
    public TMP_InputField inputFieldResult;
    public Button pauseButton;
    public Button heatButton;
    public HeatSystem heat;

    private void Start()
    {
        if (gameObject.activeInHierarchy)
        {
            heat.Invincible = !(PlayerPrefs.GetInt("Invi", 0) != 0);
            SetInvincible();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Paused)
        {
            if (inputFieldResult.text != "") {
                Timestamp = float.Parse(inputFieldResult.text);
            }
        }
        else
        {
            inputFieldResult.text = Timestamp.ToString();
        }
    }

    public void SetInvincible()
    {
        if (heat.Invincible) { 
            heat.Invincible = false;
            heatButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Invi: OFF";
        }
        else
        {
            heat.Invincible = true;
            heatButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Invi: ON";
        }
        PlayerPrefs.SetInt("Invi", heat.Invincible ? 1 : 0);
    }

    public void Pause()
    {
        Paused = !Paused;
        if (Paused)
        {
            pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Paused";
        }
        else
        {
            pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Unpaused";
        }
    }



}
