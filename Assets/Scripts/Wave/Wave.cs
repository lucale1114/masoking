using System;
using System.Collections.Generic;
using UnityEngine;
using Wave.Jesters.Blue;
using Wave.Jesters.Green;
using Wave.Jesters.Purple;
using Wave.Jesters.Red;

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
    }
}