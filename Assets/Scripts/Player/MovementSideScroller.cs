using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Wave.Handler;

namespace Player
{
    public class SideScrollerMovement : MonoBehaviour
    {

        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float acceleration = 75f;
        [SerializeField] private float deceleration = 25f;
        [SerializeField] private float turnDeceleration = 75f;
   
        [SerializeField] private AudioClip walk;


        public Vector2 currentVelocity;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        public float knocked = 0;

        private float currentTimestamp = 0f;

        private PlayerAnimator playerAnimator;

        void Start()
        {
            playerAnimator = GetComponent<PlayerAnimator>();
            rb = GetComponent<Rigidbody2D>();

        }

        void Update()
        {
           
            float axisY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(0, axisY).normalized;
          

            currentVelocity.x = -4f;

            if (!Mathf.Approximately(currentTimestamp, WaveHandler.Timestamp))
            {
                
                knocked = Mathf.Max(0, knocked - 0.1f);
                currentTimestamp = WaveHandler.Timestamp;
            }

            

                if (Mathf.Approximately(currentVelocity.y, 0))
                {
                    playerAnimator.PlayIdle();
                    SoundFXManager.Instance.StopWalking();
                }
                else
                {
                    playerAnimator.PlayMoving(currentVelocity);
                    SoundFXManager.Instance.StartWalking();
                }

            
        }



        void FixedUpdate()
        {

            var targetSpeedY = moveInput.y != 0 ? maxSpeed : 0;

            if (Mathf.Approximately(moveInput.y, 0))
            {
                currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, 0, deceleration * Time.fixedDeltaTime);
            }
            else if (moveInput.y * currentVelocity.y < 0)
            {
                currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, 0, turnDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, targetSpeedY * moveInput.y,
                    acceleration * Time.fixedDeltaTime);
            }


            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);

            

            rb.velocity = currentVelocity; //* _dashMultiplier;
        }

      
    }
}