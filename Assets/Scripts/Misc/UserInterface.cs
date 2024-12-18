using System.Collections;
using DG.Tweening;
using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wave.Handler;

namespace Misc
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] public Sprite[] kingPortraits;
        [SerializeField] public Sprite[] comboTexts;
        [SerializeField] public int[] comboArray;
        [SerializeField] public AudioClip[] kingAudioClips;
        [SerializeField] public AudioClip[] ComboClips;



        public TextMeshProUGUI _comboCounter;
        public TextMeshProUGUI _scoreCounter;

        private GameObject _pauseMenu;
        private GameObject _lostMenu;
        public GameObject _wonMenu;
        private GameObject _soundMenu;
        public Image _comboResultText;
        private WaveHandler _waveHandler;

        private GameObject winMode;
        private GameObject loseMode;
        private Transform pointerHand;
        private Image madKing;

        public Image _heatBar;
        public HeatSystem _heatSystem;
        public Score _scoreSystem;

        public Image _portrait;
        public Image _hands;
        public TextMeshProUGUI _mashSpace;
        public bool _isInMax;
        public bool _cancel;

        protected bool IsIntro;
        private Vector3 handsPosDown;
        private Vector3 handsPosUp;
        private bool inTrouble;

        protected void Awake()
        {
            _comboResultText = GameObject.Find("ComboResult").GetComponent<Image>();
            pointerHand = GameObject.Find("HandCursor").transform;
            madKing = GameObject.Find("MadKing").GetComponent<Image>();
            _comboCounter = GameObject.Find("ComboText").GetComponent<TextMeshProUGUI>();
            _comboCounter.enabled = false;

            _scoreCounter = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

            _waveHandler = FindObjectOfType<WaveHandler>();
            _mashSpace = GameObject.Find("MashSpace").GetComponent<TextMeshProUGUI>();
            _mashSpace.gameObject.SetActive(false);

            _portrait = GameObject.Find("Portrait").GetComponent<Image>();
            _hands = GameObject.Find("Hands").GetComponent<Image>();

            winMode = GameObject.Find("WinState");
            winMode.gameObject.SetActive(false);
            loseMode = GameObject.Find("LoseState");
            _lostMenu = GameObject.Find("LostMenu");
            _lostMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _lostMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            loseMode.gameObject.SetActive(false);

            _pauseMenu = GameObject.Find("PauseMenu");
            _pauseMenu.transform.Find("Elements/Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _pauseMenu.transform.Find("Elements/Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _pauseMenu.transform.Find("Elements/Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
            _pauseMenu.transform.Find("Elements/Panel/SoundBtn").GetComponent<Button>().onClick.AddListener(Sound);
            pointerHand.transform.position = new Vector3(pointerHand.transform.position.x, _pauseMenu.transform.Find("Elements/Panel/RestartBtn").transform.position.y, 0);

            _pauseMenu.SetActive(false);

            _soundMenu = GameObject.Find("SoundMenu");
            _soundMenu.SetActive(false);



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

        private void Sound()
        {
            Time.timeScale = _soundMenu.activeSelf ? 1 : 0;
            _soundMenu.SetActive(!_soundMenu.activeSelf);
            _pauseMenu.SetActive(false);
        }

        public void ButtonSelected1()
        {
            pointerHand.transform.position = new Vector3(pointerHand.transform.position.x, _pauseMenu.transform.Find("Elements/Panel/RestartBtn").transform.position.y, 0);
            madKing.sprite = kingPortraits[1];
            madKing.transform.DOKill();

        }
        public void ButtonSelected2()
        {
            pointerHand.transform.position = new Vector3(pointerHand.transform.position.x, _pauseMenu.transform.Find("Elements/Panel/MenuBtn").transform.position.y, 0);
            madKing.sprite = kingPortraits[1];
            madKing.transform.DOKill();
        }
        public void ButtonSelected3()
        {
            pointerHand.transform.position = new Vector3(pointerHand.transform.position.x, _pauseMenu.transform.Find("Elements/Panel/SoundBtn").transform.position.y, 0);
            madKing.sprite = kingPortraits[1];
            madKing.transform.DOKill();
        }
        public void ButtonSelected4()
        {
            pointerHand.transform.position = new Vector3(pointerHand.transform.position.x, _pauseMenu.transform.Find("Elements/Panel/QuitBtn").transform.position.y, 0);
            madKing.sprite = kingPortraits[0];
            madKing.transform.DOShakePosition(50, 5, 20, 90).SetUpdate(true);
        }

        private void Start()
        {
            _heatBar = GameObject.Find("HeatBar").GetComponent<Image>();
            _heatSystem = FindObjectOfType<HeatSystem>();
            _scoreSystem = FindObjectOfType<Score>();
            _heatSystem.HeatChanged += heat =>
            {
                _heatBar.DOFillAmount(heat, 0.5f).SetEase(Ease.OutSine);
                _portrait.GetComponent<Animator>().SetFloat("HeatPercent", _heatSystem.GetCurrentHeatNormalized() * 100);
                if (_heatSystem.GetCurrentHeatNormalized() < 0.2f) {
                    _hands.GetComponent<Animator>().SetInteger("State", 1);
                    Hands(true, 0, true);
                    inTrouble = true;
                }
                else if (inTrouble)
                {
                    inTrouble = false;
                    Hands(false, 0, true);
                }
            };

            if (!IsIntro)
            {
                 SoundFXManager.Instance.StopWalkingFull();
                _waveHandler.FinishedLevel += () =>
                {
                    JesterFeverHandler.JesterFever = true;
                    _mashSpace.gameObject.SetActive(true);
                    StartCoroutine(EndGame());
                };

                _heatSystem.HeatDepleted += () =>
                {
                    GameObject.Find("SoundMusicManager").GetComponent<AudioSource>().volume = 0;
                    GameObject.Find("SoundFXManager").GetComponent<AudioSource>().volume = 0;
                    loseMode.SetActive(true);
                    Time.timeScale = 0;
                };

                _heatSystem.ComboEnded += comboMultiplier => { StartCoroutine(ComboFinish(comboMultiplier)); };
                _heatSystem.MaxHeat += () => { StartCoroutine(MaxHeatGained()); };

                _heatSystem.ComboMultiplierChanged += comboMultiplier =>
                {
                    _cancel = true;
                    if (comboMultiplier == 0)
                    {
                        return;
                    }

                    _comboCounter.color = new Color32(255, 255, 255, 255);
                    _comboCounter.enabled = true;
                    _comboCounter.text = $"{comboMultiplier:0} Hit Combo!";
                    if (comboMultiplier > 5)
                    {
                        StartCoroutine(StartingCombo());
                    }
                };
                _scoreSystem.ScoreChanged += UpdateScoreCounter;
            }

            handsPosUp = _hands.transform.position;
            handsPosDown = _hands.transform.position - new Vector3(0, 230, 0);
            _hands.transform.position = handsPosDown;
        }

        IEnumerator EndGame()
        {
            yield return new WaitForSeconds(10);
            for (int i = 0; i < 3; i++) {
                Camera.main.DOOrthoSize(4.5f, 0.1f);
                yield return new WaitForSeconds(0.1f);
                Camera.main.DOOrthoSize(5f, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }
            GameObject.Find("SoundMusicManager").GetComponent<AudioSource>().DOFade(0, 3);
            GameObject.Find("SoundFXManager").GetComponent<AudioSource>().DOFade(0, 3);

            yield return new WaitForSeconds(0.2f);

            JesterFeverHandler.JesterFever = false;
            winMode.SetActive(true);
            Invoke(nameof(EndScreen), 11);
        }

        private void EndScreen()
        {
            Time.timeScale = 0;
            _wonMenu.SetActive(true);
        }

        private void ChangeKingPortrait(int index, bool punch, bool shake)
        {
            if (punch)
            {

                _portrait.transform.DOPunchScale(transform.localScale, 0.5f, 8, 0.5f);
                SoundFXManager.Instance.PlayRandomSoundFXNoPitch(kingAudioClips, 1f);
            }

            if (shake)
            {

                _portrait.transform.DOShakePosition(1, 5);
                SoundFXManager.Instance.PlayRandomSoundFXNoPitch(kingAudioClips, 1f);
            }
        }

        private IEnumerator ComboFinish(float combo)
        {
            _cancel = false;
            HandleAnimations(combo);
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSeconds(0.1f);
                _comboCounter.color = new Color32(255, 255, 255, 50);
                yield return new WaitForSeconds(0.1f);
                _comboCounter.color = new Color32(255, 255, 255, 255);
                if (_cancel)
                {
                    yield break;
                }
            }

            _comboCounter.enabled = false;
        }

        private void UpdateScoreCounter(float score)
        {
            _scoreCounter.text = score.ToString("000000000");
            _scoreCounter.transform.DORewind();
            _scoreCounter.transform.DOPunchScale(transform.localScale, 0.1f, 2, 0.1f);
        }

        private void HandleAnimations(float combo)
        {
            if (combo < comboArray[0])
            {
                return;
            }

            for (var i = 0; i < comboArray.Length; i++)
            {
                if (combo >= comboArray[i])
                {
                    _comboResultText.sprite = comboTexts[i];
                }
                else
                {
                    break;
                }
            }


            _comboResultText.GetComponent<Animator>().Play("ComboTextAnimation");
            SoundFXManager.Instance.PlayRandomSoundFXNoPitch(ComboClips, 1f);
        }

        private IEnumerator StartingCombo()
        {
            if (!_isInMax)
            {
                ChangeKingPortrait(3, false, true);
                float comboGotten = 0;
                while (_heatSystem.GetCombo() >= 5)
                {
                    comboGotten = _heatSystem.GetCombo();
                    yield return new WaitForSeconds(0.01f);
                }
                Hands(true, Mathf.Clamp(comboGotten, 5, 30), false);
                yield return new WaitForSeconds(0.5f);
                if (!_isInMax)
                {
                    ChangeKingPortrait(0, false, false);
                }
                yield return new WaitForSeconds(1.5f);
                Hands(false, 1, false);
            }
        }

        private void Hands(bool enter, float speed, bool angry)
        {
            Animator animator = _hands.GetComponent<Animator>();
            if (inTrouble) {
                return;
            }
            if (!enter)
            {
               _hands.transform.DOMove(handsPosDown, 1);
                return;
            }

            _hands.transform.DOMove(handsPosUp, 1);
            if (angry)
            {
             animator.speed = 1;
             return;
            }
            animator.SetInteger("State", 0);
            animator.speed = speed / 20;
        }

        private IEnumerator MaxHeatGained()
        {
            _heatSystem.CanMaxHeat = false;
            if (JesterFeverHandler.JesterFever)
            {
                ChangeKingPortrait(2, true, false);
                yield break;
            }
            ChangeKingPortrait(1, true, false);
            if (ColorUtility.TryParseHtmlString("#FF0000", out Color col) && !JesterFeverHandler.JesterFever)
                _portrait.DOColor(col, 1);
                _hands.DOColor(col, 1);
            _isInMax = true;
            yield return new WaitForSeconds(5);
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color cole))
                _portrait.DOColor(cole, 1);
                _hands.DOColor(cole, 1);
            _isInMax = false;
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

        protected void Update()
        {
            if (!winMode.activeSelf && !loseMode.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape)    )
                {
                    Time.timeScale = _pauseMenu.activeSelf ? 1 : 0;
                    _pauseMenu.SetActive(!_pauseMenu.activeSelf);
                    _soundMenu.SetActive(false);
                    PauseAllSources();
                }
            }
        }

        private void PauseAllSources()
        {
            var allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in allAudioSources)
            {
                if (audioSource.isActiveAndEnabled)
                {
                    if (audioSource.isPlaying) audioSource.Pause();
                    else audioSource.UnPause();
                }
            }
        }
    }
}