using System.Collections;
using DG.Tweening;
using Jester;
using Managers;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Wave.WaveData;

namespace Misc
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] kingPortraits;

        private TextMeshProUGUI _comboCounter;

        private GameObject _pauseMenu;
        private GameObject _lostMenu;
        private GameObject _wonMenu;
        private GameObject _soundMenu;
        private JesterSpawner _jesterSpawner;

        private Image _heatBar;
        private HeatSystem _heatSystem;
        private Image _portrait;
        private TextMeshProUGUI _mashSpace;
        private bool isInMax;
        private bool cancel;

        private void Awake()
        {
            _comboCounter = GameObject.Find("ComboText").GetComponent<TextMeshProUGUI>();
            _comboCounter.enabled = false;
            _jesterSpawner = GameObject.Find("Game").GetComponent<JesterSpawner>();
            _mashSpace = GameObject.Find("MashSpace").GetComponent<TextMeshProUGUI>();
            _pauseMenu = GameObject.Find("PauseMenu");
            _soundMenu = GameObject.Find("SoundMenu");
            _portrait = GameObject.Find("Portrait").GetComponent<Image>();
            //_maxHeat.gameObject.SetActive(false);
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

            _heatSystem.HeatChanged += heat => _heatBar.DOFillAmount(heat, 0.5f).SetEase(Ease.OutSine);
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
            _heatSystem.ComboMultiplierChanged += comboMultiplier =>
            {
                cancel = true;
                _comboCounter.color = new Color32(255, 255, 255, 255);
                _comboCounter.enabled = true;
                _comboCounter.text = $"{comboMultiplier:0} Hit Combo!";
                if (comboMultiplier > 5)
                {
                    StartCoroutine(StartingCombo());
                }
            };
            _heatSystem.ComboEnded += () =>
            {
                StartCoroutine(ComboFinish());
            };
            _heatSystem.MaxHeat += () =>
            {
                StartCoroutine(MaxHeatGained());
            };
        }

        private void EndGame()
        {
            JesterFever = false;
            Time.timeScale = 0;
            _wonMenu.SetActive(true);
        }

        private void ChangeKingPortrait(int index, bool punch, bool shake)
        {
            _portrait.sprite = kingPortraits[index];
            if (punch)
            {
                _portrait.transform.DOPunchScale(transform.localScale, 0.5f, 8, 0.5f);
            }
            if (shake)
            {
                _portrait.transform.DOShakePosition(1, 5, 10, 90);
            }
        }
        IEnumerator ComboFinish()
        {
            cancel = false;
            for (int i = 0; i < 6; i++) {
                yield return new WaitForSeconds(0.1f);
                print("combo");
                _comboCounter.color = new Color32(255, 255, 255, 50);
                yield return new WaitForSeconds(0.1f);
                _comboCounter.color = new Color32(255, 255, 255, 255);
                if (cancel)
                {
                    yield return null;
                }
            }
            print("Finish");
            _comboCounter.enabled = false;
        }

        IEnumerator StartingCombo()
        {
            if (!isInMax)
            {
                ChangeKingPortrait(3, false, true);
                while (_heatSystem.GetCombo() >= 5)
                {
                    yield return new WaitForSeconds(0.01f);
                }
                yield return new WaitForSeconds(0.5f);
                if (!isInMax)
                {
                    ChangeKingPortrait(0, false, false);
                }
            }
        }

        IEnumerator MaxHeatGained()
        {
            _heatSystem.CanMaxHeat = false;
            if (JesterFever)
            {
                ChangeKingPortrait(2, true, false);
                yield break;
            }
            else
            {
                ChangeKingPortrait(1, true, false);
            }
            isInMax = true;
            yield return new WaitForSeconds(5);
            isInMax = false;
            if (_heatSystem.GetCombo() >= 5)
            {
                StartCoroutine(StartingCombo());
            } 
            else
            {
                ChangeKingPortrait(0, false, false);
            }
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