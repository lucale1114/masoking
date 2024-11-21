using Jester;
using Managers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class IntroUserInterface : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro moveTextMesh;
        private GameObject _pauseMenu;

        private void Awake()
        {
         
            _pauseMenu = GameObject.Find("PauseMenu");
            _pauseMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _pauseMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _pauseMenu.transform.Find("Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
            _pauseMenu.SetActive(false);

        
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
        
        
        }

        private void Update()
        {
          
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = _pauseMenu.activeSelf ? 1 : 0;
                    _pauseMenu.SetActive(!_pauseMenu.activeSelf);
                }

              
            
        }
    }
}
