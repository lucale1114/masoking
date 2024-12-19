using System.Collections.Generic;
using Jester.Blue;
using Jester.Green;
using Jester.Purple;
using Jester.Red;
using UnityEngine;
using Movement = Jester.Green.Movement;

namespace Wave.Handler
{
    public class MovingJesterHandler : MonoBehaviour
    {
        [SerializeField] private GameObject greenJesterPrefab;
        [SerializeField] private GameObject purpleJesterPrefab;

        private List<GreenJesterData> _currentWaveGreenJesters;
        private List<PurpleJesterData> _currentWavePurpleJesters;
        private readonly List<GameObject> _currentJesters = new();

        private void Start()
        {
            Movement.AnyJesterDestroyed += OnAnyJesterDestroyed;
            Jester.Purple.Movement.AnyJesterDestroyed += OnAnyJesterDestroyed;
        }

        private void OnAnyJesterDestroyed(GameObject jester)
        {
            if (_currentJesters.IndexOf(jester) != -1)
            {
                _currentJesters.Remove(jester);
            }
        }

        public void SetCurrentWaveJesters(List<GreenJesterData> greenJesters, List<PurpleJesterData> purpleJesters)
        {
            _currentWaveGreenJesters = greenJesters;
            _currentWavePurpleJesters = purpleJesters;
        }

        public void StartWave()
        {
            _currentJesters.Clear();

            foreach (var item in _currentWaveGreenJesters)
            {
                if (item.commands.Length > 0)
                {
                    _currentJesters.Add(SpawnJester(item));
                }
            }

            foreach (var item in _currentWavePurpleJesters)
            {
                if (item.commands.Length > 0)
                {
                    _currentJesters.Add(SpawnJester(item));
                }
            }
        }

        public bool IsFinished()
        {
            return _currentJesters.Count == 0;
        }

        private GameObject SpawnJester(GreenJesterData data)
        {
            var newJester = Instantiate(greenJesterPrefab, data.startPosition, greenJesterPrefab.transform.rotation);
            newJester.GetComponent<Movement>().SetData(data);
            return newJester;
        }

        private GameObject SpawnJester(PurpleJesterData data)
        {
            var newJester = Instantiate(purpleJesterPrefab, data.startPosition, purpleJesterPrefab.transform.rotation);
            newJester.GetComponent<Jester.Purple.Movement>().SetData(data);
            return newJester;
        }
    }
}