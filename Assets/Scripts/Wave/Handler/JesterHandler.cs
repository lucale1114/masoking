using System.Collections.Generic;
using Jester;
using UnityEngine;
using static Wave.WaveData;
using Random = UnityEngine.Random;

namespace Wave.Handler
{
    public class JesterHandler : MonoBehaviour
    {
        private const float XLeft = -8.5f;
        private const float XRight = 8.5f;

        [SerializeField] private GameObject jesterPrefab;

        private StandingJesterData[] _currentWaveJesters;
        private readonly List<GameObject> _currentJesters = new();

        private Sides _lastUsed;

        private void Start()
        {
            JesterBehaviour.AnyJesterDestroyed += jester => _currentJesters.Remove(jester);
        }

        public void SetCurrentWaveJesters(StandingJesterData[] currentWaveJesters)
        {
            _currentWaveJesters = currentWaveJesters;
        }

        public void StartWave()
        {
            _currentJesters.Clear();

            foreach (var item in _currentWaveJesters)
            {
                _currentJesters.Add(SpawnJester(item));
            }
        }

        public bool IsFinished()
        {
            return _currentJesters.Count == 0;
        }

        public void SpawnMashJester()
        {
            var jesterData = new StandingJesterData
            {
                timestamp = WaveHandler.Timestamp + 0.5f,
                randomY = true,
                side = Sides.Random
            };

            var shotData = new ShotDataObject
            {
                fireBetween = Random.Range(0.5f, 1.5f),
                speed = Random.Range(7, 15),
                amount = 99,
                damage = 6
            };

            jesterData.commands = new[]
            {
                new JesterCommand()
                {
                    action = Actions.FireAimed,
                    shotData = shotData
                }
            };
            _currentJesters.Add(SpawnJester(jesterData));
        }

        private GameObject SpawnJester(StandingJesterData waveObject)
        {
            float x = 0;
            var wave = waveObject.side;
            if (wave == Sides.OppositeOfLast)
            {
                wave = _lastUsed == Sides.Right ? Sides.Left : Sides.Right;
            }
            else if (wave == Sides.CopyLast)
            {
                wave = _lastUsed;
            }

            if (wave == Sides.Left)
            {
                x = XLeft;
                _lastUsed = Sides.Left;
            }
            else if (wave == Sides.Right)
            {
                x = XRight;
                _lastUsed = Sides.Right;
            }
            else if (wave == Sides.Random)
            {
                if (Random.Range(0, 2) == 1)
                {
                    x = XLeft;
                    _lastUsed = Sides.Left;
                }
                else
                {
                    x = XRight;
                    _lastUsed = Sides.Right;
                }
            }

            var y = waveObject.randomY ? Random.Range(-5.0f, 5.0f) : waveObject.y;
            var newJester = Instantiate(jesterPrefab, new Vector3(x, y), jesterPrefab.transform.rotation);
            var jesterBehaviour = newJester.GetComponent<JesterBehaviour>();
            jesterBehaviour.enterTimestamp = waveObject.timestamp;
            jesterBehaviour.jesterCommands = waveObject.commands;

            newJester.GetComponent<JesterAnimator>().SetColor(waveObject.color);
            return newJester;
        }
    }
}