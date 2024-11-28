using System.Collections;
using Jester;
using Managers;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static WaveData;

namespace Misc
{
    public class UserInterface : MonoBehaviour
    {
        private TextMeshProUGUI _comboCounter;

        private GameObject _pauseMenu;
        private GameObject _lostMenu;
        private GameObject _wonMenu;
        private GameObject _soundMenu;
        private JesterSpawner _jesterSpawner;

        private Image _heatBar;
        private HeatSystem _heatSystem;
        private Image _maxHeat;
        private TextMeshProUGUI _mashSpace;

        private void Awake()
        {
            _comboCounter = GameObject.Find("ComboText").GetComponent<TextMeshProUGUI>();
            _jesterSpawner = GameObject.Find("Game").GetComponent<JesterSpawner>();
            _mashSpace = GameObject.Find("MashSpace").GetComponent<TextMeshProUGUI>();
            _pauseMenu = GameObject.Find("PauseMenu");
            _soundMenu = GameObject.Find("SoundMenu");
            _maxHeat = GameObject.Find("MaxHeat").GetComponent<Image>();
            _maxHeat.gameObject.SetActive(false);
            _mashSpace.gameObject.SetActive(false);
            _pauseMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _pauseMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _pauseMenu.transform.Find("Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
            _pauseMenu.transform.Find("Panel/SoundBtn").GetComponent<Button>().onClick.AddListener(Sound);
            _pauseMenu.SetActive(false);
            _soundMenu.SetActive(false);

            _lostMenu = GameObject.Find("LostMenu");
            _lostMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _lostMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _lostMenu.SetActive(false);

            _wonMenu = GameObject.Find("WonMenu");
            _wonMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _wonMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _wonMenu.SetActive(false);
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

        public void Sound()
        {
            Time.timeScale = _soundMenu.activeSelf ? 1 : 0;
            _soundMenu.SetActive(!_soundMenu.activeSelf);
            _pauseMenu.SetActive(false);
        }

        private void Start()
        {
            _heatBar = GameObject.Find("HeatBar").GetComponent<Image>();
            _heatSystem = FindObjectOfType<HeatSystem>();

            _heatSystem.HeatChanged += heat => _heatBar.fillAmount = heat;
            _jesterSpawner.FinishedLevel += () =>
            {
                JesterFever = true;
                _mashSpace.gameObject.SetActive(true);
                Invoke("EndGame", 10);
            };
            _heatSystem.HeatDepleted += () =>
            {
                Time.timeScale = 0;
                _lostMenu.SetActive(true);
            };
            _heatSystem.MaxHeat += () =>
            {
                StartCoroutine(MaxHeatGained());
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
                    _comboCounter.text = $"{comboMultiplier:0} Hit Combo!";
                }
            };
        }

        private void EndGame()
        {
            JesterFever = false;
            Time.timeScale = 0;
            _wonMenu.SetActive(true);
        }

        IEnumerator MaxHeatGained()
        {
            _heatSystem.CanMaxHeat = false;
            _maxHeat.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            _maxHeat.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            _heatSystem.CanMaxHeat = true;
        }

        private void Update()
        {
            if (!_lostMenu.activeSelf && !_wonMenu.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = _pauseMenu.activeSelf ? 1 : 0;
                    _pauseMenu.SetActive(!_pauseMenu.activeSelf);
                    _soundMenu.SetActive(false);
                }
            }
        }
    }
}