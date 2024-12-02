using System;
using UnityEngine;

namespace Wave
{
    [Serializable]
    [CreateAssetMenu(fileName = "WaveList", menuName = "WaveList")]
    public class WaveList : ScriptableObject
    {
        public Wave[] waves;
    }
}