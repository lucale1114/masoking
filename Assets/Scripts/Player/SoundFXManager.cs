using UnityEngine;

namespace Player
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager Instance;

        [SerializeField] AudioSource soundFXObject;
        [SerializeField] AudioSource walkFX;
        [SerializeField] AudioSource dashFX;

        [SerializeField] AudioClip walkClip; // Assign a walking sound clip in the inspector
        [SerializeField] AudioClip roll; // Assign a rolling sound clip in the inspector
        [SerializeField] AudioClip dashCharge; // Assign a rolling sound clip in the inspector


        float timer = 0;
        float minTime = 0.05f;


        private bool isWalking = false;
        private bool isDashing = false;


        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            soundFXObject = GetComponent<AudioSource>();

            if (soundFXObject == null)
            {
                soundFXObject = GetComponent<AudioSource>();
            }



        }

        void Update()
        {

            if (soundFXObject.pitch > 1)
            {
                //While the pitch is over 1, decrease it as time passes.
                soundFXObject.pitch -= 0.1f * Time.deltaTime;
            }

            timer += Time.deltaTime;

        }

        public void PlaySoundFX(AudioClip audioClip, float volume)
        {
            if (timer < minTime)
            {
                return;
            }

            timer = 0;

            soundFXObject.PlayOneShot(audioClip, volume);
        
        }


        public void PlayRandomSoundFX(AudioClip[] audioClip, float volume)
        {
            int rand = Random.Range(0, audioClip.Length);
            PlaySoundFX(audioClip[rand], volume);
        }

        public void PlayOnLoop()
        {
            soundFXObject.clip = roll;
        }

        public void StopLoop()
        {
            
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

    }
}
