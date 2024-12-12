using UnityEngine;

namespace Player
{
    public class DashTrail : MonoBehaviour
    {
        private ParticleSystemRenderer _trailRenderer;
        [SerializeField] AudioClip[] dash;

        private void Start()
        {
            _trailRenderer = GetComponentInChildren<ParticleSystemRenderer>();
            
            _trailRenderer.enabled = false;

            GetComponentInParent<Movement>().IsDashing += (isDashing, velocity) =>
            {
                _trailRenderer.enabled = isDashing;
                DetermineDirection(isDashing, velocity);
            };
            GetComponentInParent<Movement>().Bounced += (isDashing, velocity) =>
            {
                DetermineDirection(isDashing, velocity);
            };
        }

        private void DetermineDirection(bool isDashing, Vector3 velocity)
        {
            if (velocity.x > 0)
            {
                _trailRenderer.flip = new Vector3(1, 0, 0);
            }
            else
            {
                _trailRenderer.flip = new Vector3(0, 0, 0);
            }
        }
    }
}