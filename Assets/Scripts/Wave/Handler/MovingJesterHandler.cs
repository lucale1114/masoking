using System.Collections.Generic;
using Jester.Green;
using UnityEngine;
using static Wave.WaveData;

namespace Wave.Handler
{
    public class MovingJesterHandler : MonoBehaviour
    {
        [SerializeField] private GameObject jesterPrefab;

        private MovingJesterData[] _currentWaveJesters;
        private readonly List<GameObject> _currentJesters = new();

        public void SetCurrentWaveJesters(MovingJesterData[] currentWaveJesters)
        {
            _currentWaveJesters = currentWaveJesters;
        }

        public void StartWave()
        {
            _currentJesters.Clear();

            foreach (var item in _currentWaveJesters)
            {
                if (item.commands.Length > 0) {
                    _currentJesters.Add(SpawnJester(item));
                }
            }
        }

        public bool IsFinished()
        {
            return _currentJesters.Count == 0;
        }

        private GameObject SpawnJester(MovingJesterData data)
        {
            var newJester = Instantiate(jesterPrefab, data.startPosition, jesterPrefab.transform.rotation);
            newJester.GetComponent<Movement>().SetData(data);
            return newJester;
        }
    }
}