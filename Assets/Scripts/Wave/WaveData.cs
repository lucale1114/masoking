using System;
using UnityEngine;

namespace Wave
{
    public class WaveData : MonoBehaviour
    {
        public enum Actions
        {
            FireAimed,
            FireStorm,
            FireBurst,
            FireCurved,
            FireWavy,
            FireRow,
            Throw,
            ThrowAndRoll,
            Snipe,
        }

        public enum Sides
        {
            Left,
            Right,
            Random,
            OppositeOfLast,
            CopyLast,
        }

        public enum Color
        {
            Blue,
            Red
        }

        [Serializable]
        public class JesterCommand
        {
            public float timestamp;
            public Actions action;
            public ShotDataObject shotData;
        }

        [Serializable]
        public class JesterData
        {
            public float timestamp;
            public Color color;
            public Sides side;
            [Range(-5.0f, 5.0f)] public float y;
            public bool randomY;

            public JesterCommand[] commands;
        }

        [Serializable]
        public class SegmentData
        {
            public float timestamp;
            public Segment segment;
        }

        [Serializable]
        public class ShotDataObject
        {
            [Tooltip("Speed of the projectile. Everything uses it in floats.")]
            public float speed;

            [Tooltip("Inaccuracy of the shot. Magic number-like, higher is just more inaccurate with no specific unit.")]
            public int inaccuracy;

            [Tooltip("Scale of projectile.")]
            public float scale = 1;

            [Tooltip("How much this fills up the heat meter.")]
            public float damage = 10;

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

            [Tooltip("Time in air for throws.")] public float throwAirTime;

            [Tooltip("Curve for throws.")] public AnimationCurve animationCurve;

            [Tooltip("For burst shots. When the burst should happen in seconds. For curved, when it should start curving.")]
            public float timer;

            [Tooltip("Cosine wave settings for wave shot.")]
            public float frequency;

            [Tooltip("Cosine wave settings for wave shot.")]
            public int amp;

            [Tooltip("The speed curved shots move at. Set to negative to reverse gravity.")]
            public float gravityDir;

            [Tooltip(
                "Amount of shots, depends on the type but should be self-explanatory. Usable for aimed, burst, row, storm.")]
            public int amount;

            [Tooltip(
                "Cloned projectile speed. This is in addition to the normal speed, so setting it negative the same value will turn it 0!")]
            public float speed2;

            [Tooltip("Time inbetween shots for aimed and storm. For reticle based shots, when it should shoot.")]
            public float fireBetween;

            [Tooltip("How much area is in the row shot.")]
            public int radius;

            [Tooltip("Forces spin. Enable this to force shots not to spin on shots that are enabled by default.")]
            public bool spin;

            [Tooltip("Fires forward, based on the jester's position.")]
            public bool straight;

            [Tooltip("Number of bounces on walls")]
            public int numberOfBounces;
        }
    }
}