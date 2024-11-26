using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource soundFXObject2;
    [SerializeField] AudioSource soundFXObjectnew;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        soundFXObjectnew = GetComponent<AudioSource>();

        if (soundFXObjectnew == null)
        {
            soundFXObjectnew = GetComponent<AudioSource>();
        }
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObjectnew, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFX(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        if (spawnTransform is null)
        {
            throw new System.ArgumentNullException(nameof(spawnTransform));
        }

        int rand = Random.Range(0, audioClip.Length);

       

        soundFXObjectnew.PlayOneShot(audioClip[rand], volume);

    }
}
