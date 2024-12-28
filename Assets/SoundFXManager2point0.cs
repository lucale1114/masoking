using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SoundFXManager20 : MonoBehaviour
    {
        public static SoundFXManager20 Instance;

        [SerializeField] private AudioSource soundFXObject;
        [SerializeField] private AudioSource[] loopFXSources;  // Array to hold multiple looping audio sources
        private Dictionary<SoundType, object> soundFXDict;

        private float timer = 0;
        private const float MinTime = 0.05f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            InitializeSounds();
        }

        private void InitializeSounds()
        {
            soundFXDict = new Dictionary<SoundType, object>
            {
                //Single AudioClips
                { SoundType.Walk, Resources.Load<AudioClip>("Sounds/King/KingWalk") },
                { SoundType.DashCharge, Resources.Load<AudioClip>("Sounds/King/DashCharge") },
                { SoundType.Claps, Resources.Load<AudioClip>("Sounds/King/Claps") },

                //AudioClips
                { SoundType.Smash, Resources.LoadAll<AudioClip>("Sounds/Projectile/Smash") }, 
                { SoundType.Smack, Resources.LoadAll<AudioClip>("Sounds/Projectile/Smack") },   
                { SoundType.Bonk, Resources.LoadAll<AudioClip>("Sounds/Projectile/Bonk") },
                { SoundType.Explosion, Resources.LoadAll<AudioClip>("Sounds/Projectile/Explosion") },   
                { SoundType.Slash, Resources.LoadAll<AudioClip>("Sounds/Projectile/Slash") },   
                { SoundType.Splat, Resources.LoadAll<AudioClip>("Sounds/Projectile/Splat") },   
                { SoundType.WallBonk, Resources.LoadAll<AudioClip>("Sounds/King/WallBoing") },
                { SoundType.Dash, Resources.LoadAll<AudioClip>("Sounds/King/Dash") },
                { SoundType.KingGrunts, Resources.LoadAll<AudioClip>("Sounds/King/KingGrunts/Grunts") },   
                { SoundType.KingScore, Resources.LoadAll<AudioClip>("Sounds/King/KingGrunts/KingScore") },   
                { SoundType.Discust, Resources.LoadAll<AudioClip>("Sounds/Jesters/Discust") }, 
                { SoundType.Grunts, Resources.LoadAll<AudioClip>("Sounds/Jester/Grunts") },   
                { SoundType.Throw, Resources.LoadAll<AudioClip>("Sounds/Projectile/Throw") },   
                { SoundType.Huffing, Resources.LoadAll<AudioClip>("Sounds/King/Huffing") },   
            };
        }

        private void Update()
        {
            timer += Time.deltaTime;
        }

        public void PlaySoundFX(SoundType type, float volume = 1f)
        {
            if (!soundFXDict.ContainsKey(type)) return;
            if (timer < MinTime) return;

            timer = 0;

            if (soundFXDict[type] is AudioClip singleClip)
            {
                soundFXObject.PlayOneShot(singleClip, volume);
            }
            else if (soundFXDict[type] is AudioClip[] clipArray)
            {
                int rand = Random.Range(0, clipArray.Length);
                soundFXObject.PlayOneShot(clipArray[rand], volume);
            }
        }

        private void SetupLoopingAudioSources()
        {
            loopFXSources = new AudioSource[3]; // Create 3 AudioSource components, for example

            // Add AudioSource components to the SoundManager GameObject
            for (int i = 0; i < loopFXSources.Length; i++)
            {
                loopFXSources[i] = gameObject.AddComponent<AudioSource>();  // Add an AudioSource to this object
                loopFXSources[i].loop = true; // Set them to loop
            }
        }

        public void StartLoop(SoundType type, float volume = 1f)
        {
            if (!soundFXDict.ContainsKey(type)) return;

            if (soundFXDict[type] is AudioClip singleClip)
            {
                foreach (var audioSource in loopFXSources)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = singleClip;
                        audioSource.loop = true;
                        audioSource.volume = volume;
                        audioSource.Play();
                        break;  // Exit once the clip starts playing to prevent overwriting
                    }
                }
            }
        }

        public void StopLoop(SoundType type)
        {
            if (!soundFXDict.ContainsKey(type)) return;

            if (soundFXDict[type] is AudioClip singleClip)
            {
                foreach (var audioSource in loopFXSources)
                {
                    if (audioSource.clip == singleClip)
                    {
                        audioSource.Stop();
                        audioSource.loop = false;
                    }
                }
            }
        }
    }

    public enum SoundType
    {
        Walk,
        Dash,
        DashCharge,
        Claps,
        Smash, // Array of smash sounds
        Smack,
        Bonk,
        Boing,
        Discust,
        Grunts,
        KingGrunts,
        KingScore,
        WallBonk,
        Explosion,
        Slash,
        Splat,
        Throw,
        Huffing,

    }
}
