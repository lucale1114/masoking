using System.Collections.Generic;
using Jester.Blue;
using Jester.Red;
using UnityEngine;
using Wave.Jesters.Red;
using Random = UnityEngine.Random;

namespace Wave.Handler
{
    public class StandingJesterHandler : MonoBehaviour
    {
        private const float XLeft = -8.5f;
        private const float XRight = 8.5f;

        [SerializeField] private GameObject redJesterPrefab;
        [SerializeField] private GameObject blueJesterPrefab;

        private List<RedJesterData> _currentWaveRedJesters;
        private List<BlueJesterData> _currentWaveBlueJesters;
        private readonly List<GameObject> _currentJesters = new();

        private Sides _lastUsed;

        private void Start()
        {
            RedJesterBehaviour.AnyJesterDestroyed += jester =>
            {
                if (_currentJesters.IndexOf(jester) != -1)
                {
                    _currentJesters.Remove(jester);
                }
            };
            BlueJesterBehaviour.AnyJesterDestroyed += jester =>
            {
                if (_currentJesters.IndexOf(jester) != -1)
                {
                    _currentJesters.Remove(jester);
                }
            };
        }

        public void SetCurrentWaveJesters(List<RedJesterData> redJesters, List<BlueJesterData> blueJesters)
        {
            _currentWaveRedJesters = redJesters;
            _currentWaveBlueJesters = blueJesters;
        }

        public void StartWave()
        {
            _currentJesters.Clear();
            foreach (var item in _currentWaveRedJesters)
            {
                _currentJesters.Add(SpawnJester(item));
            }

            foreach (var item in _currentWaveBlueJesters)
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
            var jesterData = new BlueJesterData
            {
                timestamp = WaveHandler.Timestamp + 0.5f,
                randomY = true,
                side = Sides.Random
            };

            var shotData = new BlueShotDataObject
            {
                fireBetween = Random.Range(0.5f, 1.5f),
                speed = Random.Range(7, 15),
                amount = 99,
                damage = 6
            };

            jesterData.commands = new[]
            {
                new BlueJesterCommand()
                {
                    action = BlueJesterActions.FireAimed,
                    shotData = shotData
                }
            };
            _currentJesters.Add(SpawnJester(jesterData));
        }

        private GameObject SpawnJester(RedJesterData jesterData)
        {
            var x = GetWaveAndX(jesterData.side);
            var y = jesterData.randomY ? Random.Range(-5.0f, 5.0f) : jesterData.y;

            var newJester = Instantiate(redJesterPrefab, new Vector3(x, y), redJesterPrefab.transform.rotation);
            var jesterBehaviour = newJester.GetComponent<RedJesterBehaviour>();
            jesterBehaviour.enterTimestamp = jesterData.timestamp;
            jesterBehaviour.jesterCommands = jesterData.commands;

            return newJester;
        }

        private GameObject SpawnJester(BlueJesterData jesterData)
        {
            var x = GetWaveAndX(jesterData.side);
            var y = jesterData.randomY ? Random.Range(-5.0f, 5.0f) : jesterData.y;

            var newJester = Instantiate(blueJesterPrefab, new Vector3(x, y), blueJesterPrefab.transform.rotation);
            var jesterBehaviour = newJester.GetComponent<BlueJesterBehaviour>();
            jesterBehaviour.enterTimestamp = jesterData.timestamp;
            jesterBehaviour.jesterCommands = jesterData.commands;

            return newJester;
        }

        private float GetWaveAndX(Sides side)
        {
            float x = 0;
            var wave = side;
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

            return x;
        }
    }
}