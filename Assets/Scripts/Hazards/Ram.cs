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
        public float delay;
        public float damage;
        public WallSides side;
        [Range(-4.0f, 4.0f)] public float y;
        [Range(-5.0f, 5.0f)] public float x;
        public bool randomY;
    } 
}

