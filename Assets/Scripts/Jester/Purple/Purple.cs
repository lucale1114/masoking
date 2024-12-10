using System;
using UnityEngine;

namespace Jester.Purple
{
    public enum PurpleJesterActions
    {
        Move,
        Idle
    }

    [Serializable]
    public class PurpleJesterCommand
    {
        public PurpleJesterActions action;
        public Vector2 destination;
        public float time;
    }

    [Serializable]
    public class PurpleJesterData
    {
        public Vector2 startPosition;
        public bool loop;
        public PurpleJesterCommand[] commands;
    }
}