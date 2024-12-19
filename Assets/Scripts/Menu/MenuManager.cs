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
        private bool shaking;

        private int page = 0;
        Button backButtonC;
        Button frontButtonC;
        Button creditsBtn;
        TextMeshProUGUI creditsText;

        private void Awake()
        {
            GameObject.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(GameManager.LoadIntro);
            GameObject.Find("TutorialBtn").GetComponent<Button>().onClick.AddListener(GameManager.LoadTutorial);
            exitButton = GameObject.Find("Exit").GetComponent<Button>();
            exitButton.onClick.AddListener(GameManager.Quit);

            backButtonC = GameObject.Find("Back").GetComponent<Button>();
            frontButtonC = GameObject.Find("Forward").GetComponent<Button>();
            creditsBtn = GameObject.Find("CreditsBtn").GetComponent<Button>();
            creditsText = GameObject.Find("CreditsText").GetComponent<TextMeshProUGUI>();

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

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)) { 
                creditsMenu.SetActive(false);
            }    
        }
        public void OpenCredits() {
            creditsMenu.SetActive(true);
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
            currentCredits.SetActive(false);
            page++;
            currentCredits = credits[page];
            currentCredits.SetActive(true);
            creditsText.text = "Credits " + (page + 1).ToString() + "/3";
            UpdateButtons();
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
            currentCredits.SetActive(false);
            page--;
            currentCredits = credits[page];
            currentCredits.SetActive(true);
            creditsText.text = "Credits " + (page + 1).ToString() + "/3";
            UpdateButtons();
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