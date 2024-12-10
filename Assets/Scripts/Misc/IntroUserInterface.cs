using Player;
using System.Collections;
using TMPro;
using UnityEngine;
using Collision = Player.Collision;

namespace Misc
{
    public class IntroUserInterface : UserInterface
    {
         [SerializeField] private TextMeshProUGUI _moveText;
         [SerializeField] private TextMeshProUGUI _enemyText;
         [SerializeField] private TextMeshProUGUI _dashText;
         [SerializeField] private TextMeshProUGUI _dashWallText;

        private bool _hasMoved;
        private bool _beenHit;

        public bool HaveDash { get; private set; }

        protected new void Awake()
        {
            IsIntro = true;

            base.Awake();
            _comboResultText.enabled = false;
            _comboCounter.enabled = false;
            _heatBar.enabled = false;
            _heatSystem.enabled = false;          
            _portrait.enabled = false;
            _mashSpace.enabled = false;
            

             _moveText = GameObject.Find("Move").GetComponent<TextMeshProUGUI>();
             _enemyText = GameObject.Find("Enemy").GetComponent<TextMeshProUGUI>();
             _dashText = GameObject.Find("Dash").GetComponent<TextMeshProUGUI>();
             _dashWallText = GameObject.Find("DashWall").GetComponent<TextMeshProUGUI>();

            _enemyText.enabled = false;
            _dashText.enabled = false;
            _dashWallText.enabled = false;
        }

        protected new void Update()
        {
            base.Update();

            if (Input.GetAxisRaw("Vertical") != 0 && _hasMoved != true &&
                Input.GetAxisRaw("Horizontal") != 0 && _hasMoved != true)
            {
                StartCoroutine(SwitchTextMoveWithDelay());
            }

            if (Input.GetKeyDown(KeyCode.Space) && HaveDash != true && _hasMoved == true)
            {
                StartCoroutine(SwitchTextDashWithDelay());
            }

            if (Collision.BeenHit && _beenHit != true && HaveDash == true)
            {
                StartCoroutine(SwitchTextEnemyWithDelay());
            }
        }

        private IEnumerator SwitchTextDashWithDelay()
        {
            yield return new WaitForSeconds(10f);
            HaveDash = true;
            _dashText.enabled = false;
            _enemyText.enabled = true;
        }

        private IEnumerator SwitchTextMoveWithDelay()
        {
            yield return new WaitForSeconds(10f);
            _hasMoved = true;
            _dashText.enabled = true;
            _moveText.enabled = false;
        }

        private IEnumerator SwitchTextEnemyWithDelay()
        {
            _beenHit = true;
            yield return new WaitForSeconds(10f);
            _enemyText.enabled = false;
            _dashWallText.enabled = true;
        }
    }
}