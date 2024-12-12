using System;
using Projectile;
using UnityEngine;
using Wave;
using Wave.Jesters;

namespace Jester.Red
{
    public enum RedJesterActions
    {
        Throw,
        ThrowAndRoll,
        ThrowAndExplode
    }

    [Serializable]
    public class RedJesterCommand : IHasTimestamp
    {
        [Range(0.8f, 10f)]
        public float timestamp;
        public RedJesterActions action;
        public RedShotDataObject shotData;

        public float GetTimestamp()
        {
            return timestamp;
        }
    }

    [Serializable]
    public class RedJesterData
    {
        public Sides side;
        [Range(-5.0f, 5.0f)] public float y;
        public bool randomY;

        public RedJesterCommand[] commands;
    }

    [Serializable]
    public class RedShotDataObject : IShotData
    {
        [Tooltip("Speed of the projectile. Everything uses it in floats.")]
        public float speed;

        [Tooltip("Scale of projectile.")] public float scale = 1;

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

        [Tooltip(
            "Amount of shots, depends on the type but should be self-explanatory. Usable for aimed, burst, row, storm.")]
        public int amount;

        [Tooltip("Time in between shots for aimed and storm. For reticle based shots, when it should shoot.")]
        public float fireBetween;

        [Tooltip("Number of bounces on walls")]
        public int numberOfBounces;

        [Tooltip("Breaks on dash, otherwise bounces of the player.")]
        public bool breakable;

        [Tooltip("Radius for explosion")]
        public float radius;

        public float GetDamage()
        {
            return damage;
        }

        public bool GetBreakable()
        {
            return breakable;
        }
    }
}