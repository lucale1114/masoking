using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "WaveList", menuName = "WaveList")]
    public class WaveList : ScriptableObject
    {
        public Wave[] waves;
    }
}