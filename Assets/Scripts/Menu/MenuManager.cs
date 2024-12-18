using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        Button exitButton;
        GameObject[] credits;
        GameObject creditsMenu;
        GameObject currentCredits;
        private bool shaking;

        private void Awake()
        {
            GameObject.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(GameManager.LoadIntro);
            GameObject.Find("TutorialBtn").GetComponent<Button>().onClick.AddListener(GameManager.LoadTutorial);
            exitButton = GameObject.Find("Exit").GetComponent<Button>();
            exitButton.onClick.AddListener(GameManager.Quit);

            credits[0] = GameObject.Find("Credits1");
            creditsMenu = GameObject.Find("Credits");
            currentCredits = credits[0];
            credits[1] = GameObject.Find("Credits2");
            credits[2] = GameObject.Find("Credits3");
            credits[1].SetActive(false);
            credits[2].SetActive(false);
            creditsMenu.SetActive(true);
        }

        public void NextButton()
        {

        }

        public void BackButton()
        {
            //try()
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