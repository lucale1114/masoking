using Jester;
using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class UserInterface : MonoBehaviour
    {
        private TextMeshProUGUI _comboCounter;

        private GameObject _pauseMenu;
        private GameObject _lostMenu;
        private GameObject _wonMenu;
        private JesterSpawner _jesterSpawner;

        private Slider _heatBar;
        private HeatSystem _heatSystem;


        private void Awake()
        {
            _comboCounter = GameObject.Find("ComboText").GetComponent<TextMeshProUGUI>();
            _jesterSpawner = GameObject.Find("Game").GetComponent<JesterSpawner>();
            _pauseMenu = GameObject.Find("PauseMenu");
            _pauseMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _pauseMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _pauseMenu.transform.Find("Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
            _pauseMenu.SetActive(false);

            _lostMenu = GameObject.Find("LostMenu");
            _lostMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _lostMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _lostMenu.SetActive(false);

            _wonMenu = GameObject.Find("WonMenu");
            _wonMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _wonMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _wonMenu.SetActive(false);

            _heatBar = GameObject.Find("HeatBar").GetComponent<Slider>();
        }

        private static void Restart()
        {
            Time.timeScale = 1;
            GameManager.Restart();
        }

        private static void Quit()
        {
            GameManager.Quit();
        }

        private static void Menu()
        {
            Time.timeScale = 1;
            GameManager.LoadMenu();
        }

        private void Start()
        {
            _heatSystem = FindObjectOfType<HeatSystem>();

            _heatSystem.HeatChanged += heat => _heatBar.value = heat;
            _jesterSpawner.FinishedLevel += () =>
            {
                Time.timeScale = 0;
                _wonMenu.SetActive(true);
            };
            _heatSystem.HeatDepleted += () =>
            {
                Time.timeScale = 0;
                _lostMenu.SetActive(true);
            };

            _heatSystem.ComboMultiplierChanged += comboMultiplier =>
            {
                if (Mathf.Approximately(comboMultiplier, 1f))
                {
                    _comboCounter.enabled = false;
                }
                else
                {
                    _comboCounter.enabled = true;
                    _comboCounter.text = $"{comboMultiplier:0.0} Hit Combo!";
                }
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
}