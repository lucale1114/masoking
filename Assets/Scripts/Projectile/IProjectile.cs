namespace Projectile
{
    public interface IProjectile
    {
        public WaveData.ShotDataObject GetShotData();
        public float GetDamageMod();
    }
}