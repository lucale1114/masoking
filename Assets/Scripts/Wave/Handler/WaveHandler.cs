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

        private JesterHandler _jesterHandler;
        private JugglingBallHandler _jugglingBallHandler;

        private int _waveNumber;
        private bool _pausedByWave;

        private void Start()
        {
            #if UNITY_EDITOR
            if (debugForceWave > 0)
            {
                _waveNumber = debugForceWave - 1;
            }
            #endif

            _jesterHandler = GetComponent<JesterHandler>();
            _jugglingBallHandler = GetComponent<JugglingBallHandler>();

            StartCoroutine(nameof(TimestampRoutine), 0.1f);
            LaunchNextWave();
        }

        private void LaunchNextWave()
        {
            Timestamp = 0;
            var currentWave = _waveNumber < waveList.waves.Length ? waveList.waves[_waveNumber] : null;

            if (currentWave)
            {
                if (currentWave.SpawnBall)
                {
                    _jugglingBallHandler.AttemptSpawnBall();
                }

                _jesterHandler.SetCurrentWaveJesters(currentWave.jesters);
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
            _jesterHandler.StartWave();
            _pausedByWave = false;
        }

        private IEnumerator TimestampRoutine()
        {
            while (true)
            {
                if (!(_pausedByWave || Paused))
                {
                    Timestamp += 0.1f;
                    Timestamp = Mathf.Round(Timestamp * 10.0f)/10.0f;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Update()
        {
            if (!(_pausedByWave || Paused))
            {
                if (_jesterHandler.IsFinished())
                {
                    _waveNumber++;
                    LaunchNextWave();
                }
            }
        }
    }
}