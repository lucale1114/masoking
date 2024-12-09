using UnityEngine;

namespace Wave.Handler
{
    public class JesterFeverHandler : MonoBehaviour
    {
        public static bool JesterFever;

        private float _lastJesterSpawn;
        private StandingJesterHandler _standingJesterHandler;

        private void Start()
        {
            _standingJesterHandler = GetComponent<StandingJesterHandler>();
        }

        private void Update()
        {
            if (JesterFever)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!Mathf.Approximately(WaveHandler.Timestamp, _lastJesterSpawn))
                    {
                        _standingJesterHandler.SpawnMashJester();
                        _lastJesterSpawn = WaveHandler.Timestamp;
                    }
                }
            }
        }
    }
}