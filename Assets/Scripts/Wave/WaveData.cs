using System;
using UnityEngine;

public class WaveData : MonoBehaviour
{
    public static float Timestamp;
    public static bool Paused;
    public static Sides LastUsed;

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
        FireSniper,
        Throw
    }

    public enum Sides
    {
        Left,
        Right,
        Random,
        OppositeOfLast,
        CopyLast,
    }

    [Serializable]
    public class JesterCommand
    {
        public float timestamp;
        public Actions action;
        public ShotDataObject shotData;
    }

    [Serializable]
    public class AdvancedJester
    {
    }

    [Serializable]
    public class AdvancedProjectile
    {
        [Tooltip(
            "Where on the field this projectile is aimed. Changing anything here overrides the default of shooting towards the player.")]
        public float x;

        [Tooltip(
            "Where on the field this projectile is aimed. Changing anything here overrides the default of shooting towards the player.")]
        public float y;

        [Tooltip("Picks a random position on the field to shoot at horizontally. Range: -5 to 5.")]
        public bool randomX;

        [Tooltip("Picks a random position on the field to shoot at vertically. Range: -4 to 4.")]
        public bool randomY;

        [Tooltip("Time in air for throws")] public float throwAirTime;
        [Tooltip("Curve for throws")] public AnimationCurve animationCurve;
    }

    [Serializable]
    public class JesterData
    {
        public float timestamp;
        public Sides side;
        [Range(-5.0f, 5.0f)] public float y;
        public bool randomY;

        public JesterCommand[] commands;
    }

    [Serializable]
    public class ShotDataObject
    {
        [Header("All Projectiles")] [Tooltip("Speed of the projectile. Everything uses it in floats.")]
        public float speed;

        [Tooltip("Inaccuracy of the shot. Magic number-like, higher is just more inaccurate with no specific unit.")]
        public int inaccuracy;

        [Tooltip("Size of projectile... 1 will 2x the size, and -1 will set this to 0 so don't do that!")]
        public float size = 1;

        [Tooltip(
            "How much this fills up the heat meter. This value is actually 5, which means -5 will actually make this 0, so don't do that!")]
        public float damage = 5;

        [Tooltip("More configuration. Space is valuable!")]
        public AdvancedProjectile advancedSettings;


        [Header("Curved Shots \n Burst Shots")]
        [Tooltip("For burst shots. When the burst should happen in seconds. For curved, when it should start curving.")]
        public float timer;

        [Header("Wave Shots")] [Tooltip("Cosine wave settings for wave shot.")]
        public float frequency;

        [Tooltip("Cosine wave settings for wave shot.")]
        public int amp;

        [Header("Curved Shots")] [Tooltip("The speed curved shots move at. Set to negative to reverse gravity.")]
        public float gravityDir;

        [Header("Aimed Shots \n Burst Shots \n Row Shots \n Storm Shots")]
        [Tooltip(
            "Amount of shots, depends on the type but should be self-explanatory. Usable for aimed, burst, row, storm.")]
        public int amount;

        [Header("Burst Shots")]
        [Tooltip(
            "Cloned projectile speed. This is in addition to the normal speed, so setting it negative the same value will turn it 0!")]
        public float speed2;

        [Header("Storm Shots \n Aimed Shots \n Sniper Shots")]
        [Tooltip("Time inbetween shots for aimed and storm. For snipe, when it should shoot.")]
        public float fireBetween;

        [Header("Row Shots")] [Tooltip("How much area is in the row shot.")]
        public int radius;

        [Space(15)]
        [Header("All Projectiles")]
        [Tooltip("Forces spin. Enable this to force shots not to spin on shots that are enabled by default.")]
        public bool spin;

        [Tooltip("Fires forward, based on the jester's position.")]
        public bool straight;

        [Tooltip("Bounces off the walls once.")]
        public bool rebound;
    }

    void Start()
    {
        InvokeRepeating("IncrementTime", 0, 0.1f);
    }

    void IncrementTime()
    {
        if (Paused)
        {
            return;
        }

        Timestamp += 0.1f;
        Timestamp = Mathf.Round(Timestamp * 10.0f) * 0.1f;
    }

    public static void DebugPausetime(bool state)
    {
        Paused = state;
    }

    public static void ResetTime()
    {
        Timestamp = 0;
    }
}