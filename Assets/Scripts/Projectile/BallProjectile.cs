using Player;
using UnityEngine;
using Wave;

namespace Projectile
{
    public class BallProjectile : AbstractThrownProjectile
    {
        [SerializeField] AudioClip roll;
        protected override void OnUpdate(float airTime)
        {
            if (CurrentTime > Data.throwAirTime)
            {
                RigidBody.MovePosition(Direction * (Time.deltaTime * Data.speed) + (Vector2) transform.position);
                SoundFXManager.Instance.PlayOnLoop();

            }
            else
            {
                var position = Vector2.Lerp(StartPosition, Target, airTime);

                position.y += curveHeight * Data.animationCurve.Evaluate(airTime);
                RigidBody.MovePosition(position);
                SoundFXManager.Instance.StopLoop();
            }

            Destroy(gameObject, 4f);
        }

        protected override void InstantiateReticle(WaveData.ShotDataObject shotData)
        {
            base.InstantiateReticle(shotData);

            Direction = (Target - transform.position).normalized;
            Reticle.transform.right = Direction;
        }
    }
}