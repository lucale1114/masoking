using UnityEngine;

namespace Wave.Handler
{
    public class JesterFeverHandler : MonoBehaviour
    {
        public static bool JesterFever;

        private float _lastJesterSpawn;
        private JesterHandler _jesterHandler;

        private void Start()
        {
            _jesterHandler = GetComponent<JesterHandler>();
        }

        private void Update()
        {
            if (JesterFever)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!Mathf.Approximately(WaveHandler.Timestamp, _lastJesterSpawn))
                    {
                        _jesterHandler.SpawnMashJester();
                        _lastJesterSpawn = WaveHandler.Timestamp;
                    }
                }
            }
        }
    }
}