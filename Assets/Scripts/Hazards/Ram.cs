using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wave.Hazards.Ram
{
    [Serializable]
    public class BatteringRamData
    {
        public float timestamp;
        public WallSides side;
        public float delay;
        public float damage;
        public float size = 1;
        public bool launched;
        public float launchSpeedMultiplier = 1;
        [Range(-4.0f, 4.0f)] public float y;
        [Range(-5.0f, 5.0f)] public float x;
    } 
}

