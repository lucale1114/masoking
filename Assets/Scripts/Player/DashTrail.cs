using UnityEngine;

namespace Player
{
    public class DashTrail : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;

        private void Start()
        {
            _trailRenderer = GetComponentInChildren<TrailRenderer>();
            _trailRenderer.enabled = false;

            GetComponentInParent<Movement>().IsDashing += isDashing =>
            {
                _trailRenderer.enabled = isDashing;
            };
        }
    }
}