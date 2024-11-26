using UnityEngine;

namespace Projectile
{
    public interface IProjectile
    {
        public WaveData.ShotDataObject GetShotData();
        public float GetDamageMod();
        public void SetActive();
        public bool CanHitThings();
        public int GetNumberOfBounces();
        public void Bounce(Vector2 normal);
    }
}