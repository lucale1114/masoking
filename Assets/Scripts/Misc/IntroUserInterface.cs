using Player;
using System.Collections;
using TMPro;
using UnityEngine;
using Collision = Player.Collision;

namespace Misc
{
    public class IntroUserInterface : UserInterface
    {
        public GameObject boardMove;
        public GameObject boardDash;
        private float speed = 5;
        private Rigidbody2D rbMove;
        private Rigidbody2D rbDash;

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
            rbMove = boardMove.GetComponent<Rigidbody2D>();
            
        }

        protected new void Update()
        {
            base.Update();
            StartCoroutine(SwitchBoard());
          
         
        }

        private IEnumerator SwitchBoard()
        {
            yield return new WaitForSeconds(10f);
            MoveBoardMove();
            yield return new WaitForSeconds(2f);
            MoveBoardDash();
            
        }

        private void MoveBoardMove()
        {
            rbMove.velocity = Vector2.right * -3f;
        }

        private void MoveBoardDash()
        {
            rbMove.velocity = Vector2.right * 3f;
        }

    }
}