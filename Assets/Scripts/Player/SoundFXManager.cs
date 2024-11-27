using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] AudioSource soundFXObject;

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

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        /* AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

         audioSource.clip = audioClip;

         audioSource.volume = volume;

         audioSource.Play();

         float clipLength = audioSource.clip.length;

         Destroy(audioSource.gameObject, clipLength);*/
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
}
