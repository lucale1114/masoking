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
        public Transform target1;
        public Transform target2;

        private float speed = 3;
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
            _scoreCounter.enabled = false;
            _hands.enabled = false;
            

            rbMove = boardMove.GetComponent<Rigidbody2D>();
            rbDash = boardDash.GetComponent<Rigidbody2D>();

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
            rbMove.transform.position = Vector3.MoveTowards(rbMove.transform.position, target1.position, speed * Time.deltaTime);
        }

        private void MoveBoardDash()
        {
            rbDash.transform.position = Vector3.MoveTowards(rbDash.transform.position, target2.position, speed * Time.deltaTime);
        }

    }
}