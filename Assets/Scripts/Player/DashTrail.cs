using UnityEngine;

namespace Player
{
    public class DashTrail : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        [SerializeField] AudioClip[] dash;

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