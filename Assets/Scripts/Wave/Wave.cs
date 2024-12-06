using System;
using UnityEngine;
using UnityEngine.Serialization;
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
        public StandingJesterData[] standingJesters;
    }
}