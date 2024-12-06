using System;
using UnityEngine;
using static Wave.WaveData;

namespace Wave
{
    [Serializable]
    [CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
    public class Wave : ScriptableObject
    {
        public float StartDelay;
        public bool SpawnBall;
        [TextArea]
        public string Description;
        public WaveData.JesterData[] jesters;
        public SegmentData[] segments;
    }
}