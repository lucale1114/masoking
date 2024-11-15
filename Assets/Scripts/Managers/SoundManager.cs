using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;

        private AudioSource _audioSource;

        private void Awake()
        {
            if (_instance == null && _instance != this)
            {
                _instance = this;
                _audioSource = GetComponent<AudioSource>();
                DontDestroyOnLoad(_instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}