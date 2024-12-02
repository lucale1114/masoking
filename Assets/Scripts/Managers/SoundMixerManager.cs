using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer audioMixer;

        public void SetMasterVolume(float level)
        {
            //audioMixer.SetFloat("MasterVolume",level);
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
        }

        public void SetSoundFXVolume(float level)
        {
            audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);

        }

        public void SetMusicVolume(float level)
        {
            // audioMixer.SetFloat("MusicVolume", level);
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);


        }
    }
}
