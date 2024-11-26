using Misc;
using UnityEngine;

namespace Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        private AudioSource _audioSource;

        private new void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
         
        }
    }
}