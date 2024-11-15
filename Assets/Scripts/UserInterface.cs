using Managers;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject _pauseMenu;
    private GameObject _lostMenu;
    private GameObject _wonMenu;

    private Image _heatBar;
    private HeatSystem _heatSystem;


    private void Awake()
    {
        _pauseMenu = GameObject.Find("PauseMenu");
        _pauseMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
        _pauseMenu.transform.Find("Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
        _pauseMenu.SetActive(false);

        _lostMenu = GameObject.Find("LostMenu");
        _lostMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
        _lostMenu.SetActive(false);

        _wonMenu = GameObject.Find("WonMenu");
        _wonMenu.transform.Find("Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
        _wonMenu.SetActive(false);
    }

    private static void Restart()
    {
        Time.timeScale = 1;
        MasokingSceneManager.Restart();
    }

    private static void Quit()
    {
        MasokingSceneManager.Quit();
    }

    private void Start()
    {
        _heatBar = GameObject.Find("HeatBar").GetComponent<Image>();
        _heatSystem = FindObjectOfType<HeatSystem>();

        _heatSystem.HeatChanged += heat => _heatBar.fillAmount = heat;
        _heatSystem.HeatMaxedOut += () =>
        {
            Time.timeScale = 1;
            _wonMenu.SetActive(true);
        };
        _heatSystem.HeatDepleted += () =>
        {
            Time.timeScale = 1;
            _lostMenu.SetActive(true);
        };
    }

    private void Update()
    {
        if (!_lostMenu.activeSelf && !_wonMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = _pauseMenu.activeSelf ? 1 : 0;
                _pauseMenu.SetActive(!_pauseMenu.activeSelf);
            }
        }
    }
}