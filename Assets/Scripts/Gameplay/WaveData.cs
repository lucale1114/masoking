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
        FireBurst,
        FireCurved,
        FireWavy,
        FireRow,
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
        [Tooltip("Speed of the projectile. Everything uses this.")]
        public float speed;
        [Tooltip("For burst shots. When the burst should happen in seconds. For curved, when it should start curving.")]
        public float timer;
        [Tooltip("Cosine wave settings for wave shot.")]
        public float frequency;
        [Tooltip("Cosine wave settings for wave shot.")]
        public int amp;
        [Tooltip("The speed curved shots move at. Set to negative to reverse gravity.")]
        public float gravityDir;
        [Tooltip("Amount of shots, depends on the type but should be self-explanatory. Usable for aimed, burst, row, storm.")]
        public int amount;
        [Tooltip("Time inbetween shots for aimed and storm.")]
        public float fireBetween;
        [Tooltip("Inaccuracy of the shot. Aimed and storm. Magic number-like, higher is just more inaccurate with no specific unit.")]
        public int inaccuracy;
        [Tooltip("How much area is in the row shot.")]
        public int radius;
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
