using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.Audio;

public class AudioCrossFade : MonoBehaviour
{
    //
    HeatSystem heat;

    private AudioSource normal;
    private AudioSource abnormal;

    public AudioMixerGroup mixer;
    public AudioClip normalClip;
    public AudioClip abnormalClip;

    public float overrideValue;

    // Start is called before the first frame update
    void Awake()
    {
        normal      = gameObject.AddComponent<AudioSource>();
        abnormal    = gameObject.AddComponent<AudioSource>();

        normal.clip = normalClip;
        abnormal.clip = abnormalClip;

        normal.outputAudioMixerGroup = mixer;
        abnormal.outputAudioMixerGroup = mixer;

        normal.loop = true;
        abnormal.loop = true;

    }

    private void Start()
    {
        normal.Play();
        abnormal.Play();
    }

    private float GetHeat01()
    {

        //return overrideValue;

       if(heat == null){
            heat = FindObjectOfType<HeatSystem>();
       }

        return heat.GetCurrentHeatNormalized();
    }

    private void Mute()
    {
        normal.volume = 0;

    }

    // Update is called once per frame
    void Update()
{
    normal.volume = GetHeat01();
    abnormal.volume = 1 - GetHeat01();
}
}