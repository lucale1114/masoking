using System;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        Button exitButton;
        GameObject[] credits = new GameObject[3];
        GameObject creditsMenu;
        GameObject currentCredits;
        GameObject soundMenu;
        GameObject fadeOut;

        private bool shaking;
        private int page = 0;
        Button backButtonC;
        Button frontButtonC;
        Button creditsBtn;
        TextMeshProUGUI creditsText;
        private Button _playBtn;

        private void Awake()
        {
            _playBtn = GameObject.Find("PlayBtn").GetComponent<Button>();
            _playBtn.onClick.AddListener(StartGame);

            GameObject.Find("TutorialBtn").GetComponent<Button>().onClick.AddListener(GameManager.LoadTutorial);
            GameObject.Find("OptionsBtn").GetComponent<Button>().onClick.AddListener(OpenSounds);
            exitButton = GameObject.Find("Exit").GetComponent<Button>();
            exitButton.onClick.AddListener(GameManager.Quit);
            fadeOut = GameObject.Find("FadeOut");
            fadeOut.SetActive(false);
            backButtonC = GameObject.Find("Back").GetComponent<Button>();
            frontButtonC = GameObject.Find("Forward").GetComponent<Button>();
            creditsBtn = GameObject.Find("CreditsBtn").GetComponent<Button>();
            creditsText = GameObject.Find("CreditsText").GetComponent<TextMeshProUGUI>();
            soundMenu = GameObject.Find("SoundMenu");
            soundMenu.SetActive(false);
            credits[0] = GameObject.Find("Credits1");
            creditsMenu = GameObject.Find("Credits");
            currentCredits = credits[0];
            credits[1] = GameObject.Find("Credits2");
            credits[2] = GameObject.Find("Credits3");
            credits[1].SetActive(false);
            credits[2].SetActive(false);
            creditsMenu.SetActive(false);
            backButtonC.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            backButtonC.GetComponent<Button>().enabled = false;
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_playBtn.gameObject);
        }

        private void OpenSounds()
        {
            soundMenu.SetActive(true);
        }

        private void StartGame()
        {
            fadeOut.SetActive(true);
            GameObject.Find("SoundMusicManager").GetComponent<AudioSource>().DOFade(0, 1.5f);
            Invoke("RealStart", 2);
        }

        private void RealStart()
        {
            GameManager.LoadOpening();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton2)) {
                creditsMenu.SetActive(false);
                soundMenu.SetActive(false);
                EventSystem.current.SetSelectedGameObject(_playBtn.gameObject);
            }

            if (creditsMenu.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    BackButton();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    NextButton();
                }
            }
        }
        public void OpenCredits() {
            creditsMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(creditsMenu);
        }

        public void UpdateButtons()
        {
            if (currentCredits == credits[2])
            {
                frontButtonC.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
                frontButtonC.GetComponent<Button>().enabled = false;
            }
            else
            {
                frontButtonC.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                frontButtonC.GetComponent<Button>().enabled = true;
            }
            if (currentCredits == credits[0])
            {
                backButtonC.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
                backButtonC.GetComponent<Button>().enabled = false;
            }
            else
            {
                backButtonC.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                backButtonC.GetComponent<Button>().enabled = true;
            }
        }

        public void NextButton()
        {
            if (page < 2)
            {
                currentCredits.SetActive(false);
                page++;
                currentCredits = credits[page];
                currentCredits.SetActive(true);
                creditsText.text = "Credits " + (page + 1).ToString() + "/3";
                UpdateButtons();
            }
        }

        public void ButtonSelected(Image button)
        {
            Color32 color = button.color;
            button.color = new Color32(255, 229, 0, color.a);
        }

        public void ButtonDeselected(Image button)
        {
            Color32 color = button.color;
            button.color = new Color32(255, 255, 255, color.a);

        }
        public void BackButton()
        {
            if (page > 0)
            {
                currentCredits.SetActive(false);
                page--;
                currentCredits = credits[page];
                currentCredits.SetActive(true);
                creditsText.text = "Credits " + (page + 1).ToString() + "/3";
                UpdateButtons();
            }
        }

        public void Entered()
        {
            exitButton.transform.DOShakePosition(50, 5, 20, 90);
        }

        public void Exited()
        {
            exitButton.transform.DOKill();
        }
    }
}