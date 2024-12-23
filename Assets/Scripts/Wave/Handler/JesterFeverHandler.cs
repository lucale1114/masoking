


using UnityEngine;
using UnityEngine.InputSystem;

namespace Wave.Handler
{
    public class JesterFeverHandler : MonoBehaviour
    {
        public static bool JesterFever;

        private float _lastJesterSpawn;
        private StandingJesterHandler _standingJesterHandler;

        private InputAction _selectAction;

        private void Start()
        {
            _standingJesterHandler = GetComponent<StandingJesterHandler>();
            _selectAction = InputSystem.actions.FindAction("Select");
        }

        private void Update()
        {
            if (JesterFever)
            {
                if (_selectAction.IsPressed())
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