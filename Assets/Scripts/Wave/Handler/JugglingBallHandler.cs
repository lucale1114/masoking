using UnityEngine;

namespace Wave.Handler
{
    public class JugglingBallHandler : MonoBehaviour
    {
        private GameObject _jugglingBall;

        public void AttemptSpawnBall()
        {
            if (!_jugglingBall)
            {
                _jugglingBall = Instantiate(Resources.Load($"CircusBall") as GameObject);
            }
        }
    }
}