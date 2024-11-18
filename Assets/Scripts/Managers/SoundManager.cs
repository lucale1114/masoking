using Misc;
using UnityEngine;

namespace Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        private AudioSource _audioSource;
        private AudioClip _hit;

        private new void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _hit = Resources.Load<AudioClip>($"Effects/Hit_placeholder");
        }

        public static void PlayHit(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(Instance._hit, position);
        }
    }
}