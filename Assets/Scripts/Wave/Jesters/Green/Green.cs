using System;
using UnityEngine;

namespace Wave.Jesters.Green
{
    public enum GreenJesterActions
    {
        Move,
        Idle
    }

    [Serializable]
    public class GreenJesterCommand
    {
        public GreenJesterActions action;
        public Vector2 destination;
        public float time;
    }

    [Serializable]
    public class GreenJesterData
    {
        public Vector2 startPosition;
        public bool loop;
        public GreenJesterCommand[] commands;
    }
}