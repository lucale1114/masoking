using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager Instance;

        [SerializeField] AudioSource soundFXObject;
        [SerializeField] AudioSource soundFXObjectNoPitch;

        [SerializeField] AudioSource walkFX;
        [SerializeField] AudioSource dashFX;
        [SerializeField] AudioSource Claps;

        [SerializeField] AudioClip walkClip; // Assign a walking sound clip in the inspector
        [SerializeField] AudioClip roll; // Assign a rolling sound clip in the inspector
        [SerializeField] AudioClip dashCharge; // Assign a rolling sound clip in the inspector
        [SerializeField] AudioClip claps;


       // float timer = 0;
        //float minTime = 0.05f;


        private bool isWalking = false;
        private bool isDashing = false;


        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Update()
        {

            if (soundFXObject.pitch > 1)
            {
                //While the pitch is over 1, decrease it as time passes.
                soundFXObject.pitch -= 0.1f * Time.deltaTime;
            }

        }

        public void PlaySoundFX(AudioClip audioClip, float volume)
        {
            

      

            soundFXObject.PlayOneShot(audioClip, volume);
        
        }


        public void PlayRandomSoundFX(AudioClip[] audioClip, float volume)
        {
            int rand = Random.Range(0, audioClip.Length);
            PlaySoundFX(audioClip[rand], volume);
        }

        public void PlayRandomSoundFXNoPitch(AudioClip[] audioClip, float volume)
        {
            int rand = Random.Range(0, audioClip.Length);
            

            soundFXObjectNoPitch.PlayOneShot(audioClip[rand], volume);
            
        }

        public void PlayOnLoop()
        {

            Claps.clip = claps;
            Claps.loop = true;
            Claps.volume = 0.6F;
            Claps.Play();
        }

        public void StopLoop()
        {
          
            Claps.loop = false;
        }


        public void StartWalking()
        {
            if (!isWalking && walkFX != null && walkClip != null)
            {
                isWalking = true;
                walkFX.clip = walkClip;
                walkFX.loop = true; // Optional: Set to `true` for continuous playback
                walkFX.Play();
            }
        }

        public void StopWalking()
        {
            if (isWalking && walkFX.isPlaying)
            {
                walkFX.Stop();
            }
            isWalking = false;
            walkFX.loop = false;
        }

        public void StartDash()
        {
            if (!isDashing && dashFX != null && dashCharge != null)
            {
                isDashing = true;
                dashFX.clip = dashCharge;
                dashFX.loop = true; // Optional: Set to `true` for continuous playback
                dashFX.Play();
            }
        }

        public void StopDash()
        {
            if (isDashing && dashFX.isPlaying)
            {
                dashFX.Stop();
            }
            isDashing = false;
            dashFX.loop = false;
        }

        public void PitchChange()
        {
            soundFXObject.pitch = Mathf.Min(soundFXObject.pitch += 0.05f, 2f);
        }

        public void StopWalkingFull()
        {
           walkFX.Stop();
            walkFX.loop = false;
        }

    }
}
