using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WaveList", menuName = "WaveList")]
public class WaveList : ScriptableObject
{
    public Wave[] waves;
}
