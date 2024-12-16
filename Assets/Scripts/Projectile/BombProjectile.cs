using Jester;
using Jester.Red;
using Player;
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
                    var bomb = GetComponent<Bomb>();
                    bomb.SetProperties(Data.radius, Data.damage);
                    bomb.Activate();
                }
            }
            else
            {
                var position = Vector2.Lerp(StartPosition, Target, airTime);

                position.y += curveHeight * Data.animationCurve.Evaluate(airTime);
                RigidBody.MovePosition(position);
            }
        }

        public override bool CanHitThings()
        {
            return false;
        }
    }
}