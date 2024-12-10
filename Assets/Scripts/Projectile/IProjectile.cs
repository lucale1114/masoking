using UnityEngine;
using Wave;

namespace Projectile
{
    public interface IProjectile
    {
        public IShotData GetShotData();
        public float GetDamageMod();
        public bool CanHitThings();
        public int GetNumberOfBounces();
        public void AttemptBounce(Vector2 normal);
    }

    public interface IShotData
    {
        public float GetDamage();
        public bool GetBreakable();
    }
}