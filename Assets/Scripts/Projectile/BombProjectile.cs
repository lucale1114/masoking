using Jester;
using Jester.Red;
using UnityEngine;

namespace Projectile
{
    public class BombProjectile : AbstractThrownProjectile
    {
        private bool _isActivated;

        protected override void OnUpdate(float airTime)
        {
            if (CurrentTime > Data.throwAirTime)
            {
                if (!_isActivated)
                {
                    _isActivated = true;
                    StopSpin();
                    GetComponent<Bomb>().Activate();
                }
            }
            else
            {
                var position = Vector2.Lerp(StartPosition, Target, airTime);

                position.y += curveHeight * Data.animationCurve.Evaluate(airTime);
                RigidBody.MovePosition(position);
            }
        }

        protected override void InstantiateReticle(RedShotDataObject shotData)
        {
            base.InstantiateReticle(shotData);

            Direction = (Target - transform.position).normalized;
            Reticle.transform.right = Direction;
        }
    }
}