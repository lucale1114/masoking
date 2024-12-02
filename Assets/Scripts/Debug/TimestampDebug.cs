using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Wave.WaveData;

public class TimestampDebug : MonoBehaviour
{
    public TMP_InputField inputFieldResult;
    public Button pauseButton;
    public Button heatButton;
    public HeatSystem heat;
    public bool mousePositionCall;

    private void Start()
    {
        if (gameObject.activeInHierarchy)
        {
            heat.invincible = !(PlayerPrefs.GetInt("Invi", 0) != 0);
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
        if (mousePositionCall && Input.GetMouseButtonDown(0))
        {
            print(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void SetInvincible()
    {
        if (heat.invincible) {
            heat.invincible = false;
            heatButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Invi: OFF";
        }
        else
        {
            heat.invincible = true;
            heatButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Invi: ON";
        }
        PlayerPrefs.SetInt("Invi", heat.invincible ? 1 : 0);
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
