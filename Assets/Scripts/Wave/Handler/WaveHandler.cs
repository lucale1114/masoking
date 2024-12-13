using System;
using System.Collections;
using UnityEngine;

namespace Wave.Handler
{
    public class WaveHandler : MonoBehaviour
    {
        public event Action FinishedLevel;

        public static bool Paused;
        public static float Timestamp;

        [SerializeField] private WaveList waveList;
        [SerializeField] private int debugForceWave;

        private StandingJesterHandler _standingJesterHandler;
        private MovingJesterHandler _movingJesterHandler;
        private JugglingBallHandler _jugglingBallHandler;
        private RamHandler _ramHandler;

        private int _waveNumber;
        private bool _pausedByWave;

        private void Awake()
        {
            Timestamp = 0;
        }

        private void Start()
        {
            #if UNITY_EDITOR
            if (debugForceWave > 0)
            {
                _waveNumber = debugForceWave - 1;
            }
            #endif

            _standingJesterHandler = GetComponent<StandingJesterHandler>();
            _movingJesterHandler = GetComponent<MovingJesterHandler>();
            _ramHandler = GetComponent<RamHandler>();
            _jugglingBallHandler = GetComponent<JugglingBallHandler>();

            StartCoroutine(nameof(TimestampRoutine), 0.1f);
            LaunchNextWave();
        }

        private void LaunchNextWave()
        {
            var currentWave = _waveNumber < waveList.waves.Length ? waveList.waves[_waveNumber] : null;
            if (currentWave)
            {
                Timestamp = 0;

                if (currentWave.SpawnBall)
                {
                    _jugglingBallHandler.AttemptSpawnBall();
                }

                _standingJesterHandler.SetCurrentWaveJesters(currentWave.red, currentWave.blue);
                _movingJesterHandler.SetCurrentWaveJesters(currentWave.green, currentWave.purple);
                _ramHandler.SetRams(currentWave.ram);

                StartCoroutine(PauseRoutine(currentWave.StartDelay));
            }
            else
            {
                FinishedLevel?.Invoke();
            }
        }

        private IEnumerator PauseRoutine(float delay)
        {
            _pausedByWave = true;
            yield return new WaitForSeconds(delay);
            _standingJesterHandler.StartWave();
            _movingJesterHandler.StartWave();
            _ramHandler.StartWave();
            yield return null; // wait one frame to allow waves to initialize properly
            _pausedByWave = false;
        }

        private IEnumerator TimestampRoutine()
        {
            while (true)
            {
                if (!(_pausedByWave || Paused))
                {
                    Timestamp += 0.1f;
                    Timestamp = Mathf.Round(Timestamp * 10.0f) / 10.0f;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Update()
        {
            if (!(_pausedByWave || Paused))
            {
                if (_standingJesterHandler.IsFinished() && _movingJesterHandler.IsFinished())
                {
                    _waveNumber++;
                    LaunchNextWave();
                }
            }
        }
    }
}