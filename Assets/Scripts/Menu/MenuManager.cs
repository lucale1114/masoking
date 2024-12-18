using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        Button exitButton;
        private bool shaking;

        private void Awake()
        {
            GameObject.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(GameManager.LoadIntro);
            GameObject.Find("TutorialBtn").GetComponent<Button>().onClick.AddListener(GameManager.LoadTutorial);
            exitButton = GameObject.Find("Exit").GetComponent<Button>();
            exitButton.onClick.AddListener(GameManager.Quit);
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