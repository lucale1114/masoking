﻿using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    [CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
    public class Wave : ScriptableObject
    {
        public float StartDelay;
        public bool SpawnBall;
        public WaveData.JesterData[] jesters;
    }
}