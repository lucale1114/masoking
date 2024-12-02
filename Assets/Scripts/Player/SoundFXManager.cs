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

    public void PitchChange()
    {
        soundFXObject.pitch += 0.05f;
    }

}
