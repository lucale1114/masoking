using UnityEngine;

namespace Projectile
{
    public class CurvedProjectile : AbstractThrownProjectile
    {
        protected override void OnUpdate(float airTime)
        {
            if (CurrentTime > Data.throwAirTime)
            {
                Destroy(gameObject);
            }

            var position = Vector2.Lerp(StartPosition, Target, airTime);

            position.y += curveHeight * Data.animationCurve.Evaluate(airTime);
            RigidBody.MovePosition(position);
        }
    }
}