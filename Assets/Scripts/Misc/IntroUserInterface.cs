using Jester;
using Managers;
using Player;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class IntroUserInterface : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI moveTextMesh;
        [SerializeField]
        private TextMeshProUGUI DashTextMesh;
        [SerializeField]
        private TextMeshProUGUI dashwallTextMesh;
        private GameObject _pauseMenu;
        private bool hasMoved;
        private bool hasDashed;

        private void Awake()
        {
         
            _pauseMenu = GameObject.Find("PauseMenu");
            _pauseMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _pauseMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _pauseMenu.transform.Find("Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
            _pauseMenu.SetActive(false);
            DashTextMesh.enabled = false;
            dashwallTextMesh.enabled = false;
            hasMoved = false;
            hasDashed = false;


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

            if (Input.GetAxisRaw("Vertical") != 0 && hasMoved != true || Input.GetAxisRaw("Horizontal") != 0 && hasMoved != true )
            {
              
                StartCoroutine(SwitchTextMoveWithDelay());

            }

            if (Input.GetKeyDown(KeyCode.Space) && hasDashed != true)
            {
               
                StartCoroutine(SwitchTextDashWithDelay());
            }
            
        }

        private IEnumerator SwitchTextDashWithDelay()
        {
            DashTextMesh.enabled = true; // Show DashTextMesh
            yield return new WaitForSeconds(2f); // Wait for seconds
            DashTextMesh.enabled = false; // Hide DashTextMesh
            dashwallTextMesh.enabled = true; // Show dashwallTextMesh
            hasDashed = true;

        }

        private IEnumerator SwitchTextMoveWithDelay()
        {
            
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            DashTextMesh.enabled = true;
            moveTextMesh.enabled = false;
            hasMoved = true;

        }
    }
}
