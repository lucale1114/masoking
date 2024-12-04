using Jester;
using Managers;
using Player;
using Projectile;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField]
        private TextMeshProUGUI EnemyTextMesh;
        private GameObject _pauseMenu;
        private GameObject _soundMenu;
        private bool hasMoved;
        private bool hasDashed;
        public Movement movement;

        public bool HaveDash => hasDashed;

        private void Awake()
        {
         
            _pauseMenu = GameObject.Find("PauseMenu");
            _soundMenu = GameObject.Find("SoundMenu");
            _pauseMenu.transform.Find("Panel/RestartBtn").GetComponent<Button>().onClick.AddListener(Restart);
            _pauseMenu.transform.Find("Panel/MenuBtn").GetComponent<Button>().onClick.AddListener(Menu);
            _pauseMenu.transform.Find("Panel/QuitBtn").GetComponent<Button>().onClick.AddListener(Quit);
            _pauseMenu.transform.Find("Panel/SoundBtn").GetComponent<Button>().onClick.AddListener(Sound);
            _pauseMenu.SetActive(false);
            _soundMenu.SetActive(false);
            moveTextMesh.enabled = true;
            DashTextMesh.enabled = false;
            dashwallTextMesh.enabled = false;
            EnemyTextMesh.enabled = false;
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

        public void Sound()
        {
            Time.timeScale = _soundMenu.activeSelf ? 1 : 0;
            _soundMenu.SetActive(!_soundMenu.activeSelf);
            _pauseMenu.SetActive(false);

        }


        public void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = _pauseMenu.activeSelf ? 1 : 0;
                _pauseMenu.SetActive(!_pauseMenu.activeSelf);
                _soundMenu.SetActive(false);
            }
            

            if (Input.GetAxisRaw("Vertical") != 0 && hasMoved != true || Input.GetAxisRaw("Horizontal") != 0 && hasMoved != true )
            {
              
                StartCoroutine(SwitchTextMoveWithDelay());

            }

            if (Input.GetKeyDown(KeyCode.Space) && hasDashed != true)
            {
          
                StartCoroutine(SwitchTextDashWithDelay());
            }

            if (Player.Collision.BeenHit == true && hasDashed != false)
            {
                StartCoroutine(SwitchTextEnemyWithDelay());
            }

        

        }

      

        private IEnumerator SwitchTextDashWithDelay()
        {
            hasDashed = true;
            yield return new WaitForSeconds(5f); // Wait for seconds
            DashTextMesh.enabled = false; // Show DashTextMesh
            EnemyTextMesh.enabled = true; // Show dashwallTextMesh

        }

        private IEnumerator SwitchTextMoveWithDelay()
        {
            
            hasMoved = true;
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            DashTextMesh.enabled = true;
            moveTextMesh.enabled = false;

        }

        private IEnumerator SwitchTextEnemyWithDelay()
        {

            yield return new WaitForSeconds(2f);
            EnemyTextMesh.enabled = false;
            dashwallTextMesh.enabled = true;
           

        }
   

       
    }
}
