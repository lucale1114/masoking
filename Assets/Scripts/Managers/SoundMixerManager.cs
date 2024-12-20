using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace Managers
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private GameObject defaultSelectedGameObject;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedGameObject);
        }

        public void SetMasterVolume(float level)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
        }

        public void SetSoundFXVolume(float level)
        {
            audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
        }

        public void SetMusicVolume(float level)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
        }
    }
}
