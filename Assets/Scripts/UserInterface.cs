using System;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Image _heatBar;
    private GameObject _popup;
    private HeatSystem _heatSystem;


    private void Awake()
    {
        GameObject.Find("RestartBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            MasokingSceneManager.Restart();
        });
        GameObject.Find("QuitBtn").GetComponent<Button>().onClick.AddListener(MasokingSceneManager.Quit);

        _popup = GameObject.Find("Popup");
        _popup.SetActive(false);
    }

    private void Start()
    {
        _heatBar = GameObject.Find("HeatBar").GetComponent<Image>();
        _heatSystem = FindObjectOfType<HeatSystem>();

        _heatSystem.HeatChanged += heat => _heatBar.fillAmount = heat;
        _heatSystem.HeatMaxedOut += () => { };
        _heatSystem.HeatDepleted += () => { };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = _popup.activeSelf ? 1 : 0;
            _popup.SetActive(!_popup.activeSelf);
        }
    }
}