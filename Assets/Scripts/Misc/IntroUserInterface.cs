using System.Collections;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Misc
{
    public class IntroUserInterface : UserInterface
    {
        public GameObject boardMove;
        public GameObject boardDash;
        public GameObject jesterTeacher;
        private float acceleration = 0.5f;

        public Transform target1;
        public Transform target2;
        public Transform target3;   

        private Rigidbody2D rbMove;
        private Rigidbody2D rbDash;
        private Rigidbody2D rbTeacher;
        private GameObject _player;
        bool VFx = false;
        bool haveHit = false;
        [SerializeField] AudioClip sad;
        [SerializeField] AudioClip glad;

        [SerializeField] protected GameObject hitFloorVfxPrefab;



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


            _player.GetComponent<Movement>().enabled = false;
            _player.GetComponent<PlayerAnimator>().PlayRelax();


            rbMove = boardMove.GetComponent<Rigidbody2D>();
            rbDash = boardDash.GetComponent<Rigidbody2D>();
            rbTeacher = jesterTeacher.GetComponent<Rigidbody2D>();


            StartCoroutine(SwitchBoard());
            
           

        }




        protected new void Update()
        {
            base.Update();
            if (_heatSystem.beenHit == true && haveHit == false)
            {
                _player.GetComponent<Movement>().enabled = true;
                SoundFXManager.Instance.PlaySoundFX(glad, 1f);
                haveHit = true;
            }

            StartCoroutine(MoveJester());
        }


        private IEnumerator SwitchBoard()
        {
            yield return new WaitForSeconds(7f);
            SoundFXManager.Instance.PlaySoundFX(sad,1f);
            yield return new WaitForSeconds(10f);
            MoveBoardMoveRigth();
            yield return new WaitForSeconds(10f);
            MoveBoardMoveLeft();
            yield return new WaitForSeconds(5f);
            MoveBoardDash();

        }

        private IEnumerator MoveJester()
        {
            yield return new WaitForSeconds(10);
            JesterFall();
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

        private void JesterFall()
        {
          
            // Convert target3.position to Vector2
            Vector2 targetPosition = target3.position; // Implicitly converts Vector3 to Vector2 by ignoring the Z axis

            // Direction to target
            Vector2 direction = (targetPosition - rbTeacher.position).normalized;
            float gravityEffect = 20f; // Adjust gravity strength

            // Apply velocity with acceleration and gravity
            rbTeacher.velocity += acceleration * Time.deltaTime * direction;
            rbTeacher.velocity += gravityEffect * Time.deltaTime * Vector2.down  ;

            // Clamp speed to prevent it from getting too fast
            float maxSpeed = 10f;
            if (rbTeacher.velocity.magnitude > maxSpeed)
            {
                rbTeacher.velocity = rbTeacher.velocity.normalized * maxSpeed;
            }

            // Stop moving if close to the target
            if (Vector2.Distance(rbTeacher.position, targetPosition) < 0.1f)
            {
                rbTeacher.velocity = Vector2.zero;
                rbTeacher.MovePosition(targetPosition); // Snap the object to the target position
                if (VFx == false)
                {
                    Instantiate(hitFloorVfxPrefab, rbTeacher.transform.position, Quaternion.identity);
                    VFx = true;

                }
            }

        }

    }
}