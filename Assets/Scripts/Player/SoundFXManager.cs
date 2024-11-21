using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
