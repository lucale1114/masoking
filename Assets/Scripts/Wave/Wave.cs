using System;
using System.Collections.Generic;
using Jester.Blue;
using Jester.Green;
using Jester.Purple;
using Jester.Red;
using UnityEngine;
using Wave.Hazards.Ram;

namespace Wave
{
    [Serializable]
    [CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
    public class Wave : ScriptableObject
    {
        public float StartDelay;
        public bool SpawnBall;
        [TextArea] public string Description;

        public List<RedJesterData> red = new();
        public List<BlueJesterData> blue = new();
        public List<GreenJesterData> green = new();
        public List<PurpleJesterData> purple = new();
        public List<BatteringRamData> ram = new();
        public bool updated;
    }
}