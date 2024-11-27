using UnityEngine;

namespace Projectile
{
    public interface IProjectile
    {
        public WaveData.ShotDataObject GetShotData();
        public float GetDamageMod();
        public bool CanHitThings();
        public int GetNumberOfBounces();
        public void AttemptBounce(Vector2 normal);
    }
}