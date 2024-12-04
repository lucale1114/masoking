using UnityEngine;

namespace Player
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager Instance;

        [SerializeField] AudioSource soundFXObject;
        [SerializeField] AudioSource walkFX;

        [SerializeField] private AudioClip walkClip; // Assign a walking sound clip in the inspector
        [SerializeField] private AudioClip roll; // Assign a walking sound clip in the inspector


        private bool isWalking = false;


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

        }

        public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
        {

            if (spawnTransform is null)
            {
                throw new System.ArgumentNullException(nameof(spawnTransform));
            }

            soundFXObject.PlayOneShot(audioClip, volume);
        }

        public void PlayRandomSoundFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
        {
            if (spawnTransform is null)
            {
                throw new System.ArgumentNullException(nameof(spawnTransform));
            }

            int rand = Random.Range(0, audioClip.Length);



            soundFXObject.PlayOneShot(audioClip[rand], volume);

        }

        public void PlayOnLoop()
        {
            soundFXObject.clip = roll;
            walkFX.loop = true; // Optional: Set to `true` for continuous playback
            walkFX.Play();
        }

        public void StopLoop()
        {
            walkFX.loop = false;
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

        public void PitchChange()
        {
            soundFXObject.pitch += 0.05f;
        }

    }
}
