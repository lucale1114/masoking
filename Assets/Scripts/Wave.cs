using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveData;

[Serializable]
[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public JesterData[] jesters;
}

