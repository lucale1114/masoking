using DG.Tweening;
using Player;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Misc
{
    public class IntroUserInterface : UserInterface
    {
        public GameObject boardMove;
        public GameObject boardDash;
        public Transform target1;
        public Transform target2;

        private readonly float speed = 3;
        private Rigidbody2D rbMove;
        private Rigidbody2D rbDash;
        private GameObject _player;

        

        protected new void Awake()
        {
            IsIntro = true;

            base.Awake();
            _comboResultText.enabled = false;
            _comboCounter.enabled = false;
           
           
            _mashSpace.enabled = false;
            _scoreCounter.enabled = false;

            _wonMenu.Equals(false);
            

            _hands.enabled = false;

            _player = GameObject.Find("Player");

            _player.GetComponent<Player.Movement>().enabled = false;

            rbMove = boardMove.GetComponent<Rigidbody2D>();
            rbDash = boardDash.GetComponent<Rigidbody2D>();

            StartCoroutine(SwitchBoard());
            StartCoroutine(WaitSec());

        }




        protected new void Update()
        {
            base.Update();
        }

      
        private IEnumerator SwitchBoard()
        {
            yield return new WaitForSeconds(7f);
            MoveBoardMoveRigth();
            yield return new WaitForSeconds(7f);
            MoveBoardMoveLeft();
            yield return new WaitForSeconds(2f);
            MoveBoardDash();
            
        }

        private IEnumerator WaitSec()
        {
            yield return new WaitForSeconds(7f);
            _player.GetComponent<Player.Movement>().enabled = true;

        }

        private void MoveBoardMoveRigth()
        {
            rbMove.transform.DOMove(target2.position, 2f); 
        }

        private void MoveBoardMoveLeft()
        {
            rbMove.transform.DOMove(target1.position, 2f);
        }


        private void MoveBoardDash()
        {
            rbDash.transform.DOMove(target2.position, 2f);
        }

    }
}