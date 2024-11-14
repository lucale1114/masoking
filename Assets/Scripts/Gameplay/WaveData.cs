using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData : MonoBehaviour
{
    public static float Timestamp;
    public enum Actions
    {
        Enter,
        Leave,
        FireAimed,
        FireStorm,
        FireCurved,
        FireWavy,
        FireBurst,
    }
    public enum Sides
    {
        Left,
        Right,
        Random,
    }

    [System.Serializable]
    public class JesterCommand
    {
        public float timestamp;
        public Actions action;
        public ShotDataObject shotData;
    }

    [System.Serializable]
    public class WaveSpawning
    {
        public float timestamp;
        public Sides side;
        [Range(-5.0f, 5.0f)]
        public float y;
        public JesterCommand[] commands;
    }

    [System.Serializable]
    public class ShotDataObject
    {
        public float speed;
        public float burstTimer;
        public float frequency;
        public float amp;
        public float gravityTimer;
        public float gravityDir;
        public int amount;
        public float fireBetween;
    }

    void Start()
    {
        InvokeRepeating("IncrementTime", 0, 0.1f);
    }

    void IncrementTime()
    {
        Timestamp += 0.1f;
    }

    void ResetTime()
    {
        Timestamp = 0;
    }
}
