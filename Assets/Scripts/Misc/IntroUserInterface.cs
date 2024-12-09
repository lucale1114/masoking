using System.Collections;
using TMPro;
using UnityEngine;
using Collision = Player.Collision;

namespace Misc
{
    public class IntroUserInterface : UserInterface
    {
        private TextMeshProUGUI _moveText;
        private TextMeshProUGUI _enemyText;
        private TextMeshProUGUI _dashText;
        private TextMeshProUGUI _dashWallText;

        private bool _hasMoved;

        public bool HaveDash { get; private set; }

        protected new void Awake()
        {
            base.Awake();

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

            if (Input.GetAxisRaw("Vertical") != 0 && _hasMoved != true ||
                Input.GetAxisRaw("Horizontal") != 0 && _hasMoved != true)
            {
                StartCoroutine(SwitchTextMoveWithDelay());
            }

            if (Input.GetKeyDown(KeyCode.Space) && HaveDash != true)
            {
                StartCoroutine(SwitchTextDashWithDelay());
            }

            if (Collision.BeenHit && HaveDash)
            {
                StartCoroutine(SwitchTextEnemyWithDelay());
            }
        }

        private IEnumerator SwitchTextDashWithDelay()
        {
            HaveDash = true;
            yield return new WaitForSeconds(5f);
            _dashText.enabled = false;
            _enemyText.enabled = true;
        }

        private IEnumerator SwitchTextMoveWithDelay()
        {
            _hasMoved = true;
            yield return new WaitForSeconds(2f);
            _dashText.enabled = true;
            _moveText.enabled = false;
        }

        private IEnumerator SwitchTextEnemyWithDelay()
        {
            yield return new WaitForSeconds(2f);
            _enemyText.enabled = false;
            _dashWallText.enabled = true;
        }
    }
}