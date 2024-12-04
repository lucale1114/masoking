using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave;

[Serializable]
[CreateAssetMenu(fileName = "Segment", menuName = "Segment")]
public class Segment : ScriptableObject
{
    [TextArea]
    public string description;
    public WaveData.JesterData[] events;
}
