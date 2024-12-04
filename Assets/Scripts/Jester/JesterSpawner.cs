using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave;
using static Wave.WaveData;


#if UNITY_EDITOR
#endif

namespace Jester
{
    public class JesterSpawner : MonoBehaviour
    {
        const float X_LEFT = -8.5f;
        const float X_RIGHT = 8.5f;
        private float currentTick;

        private float waveEndTime = 5;
        private int waveNumber = 0;
        private bool waveEnded = false;
        private bool spawnDebounce;
        private bool finished;
        public GameObject jester;
        public WaveList waves;
        public Wave.Wave currentWave;
        public int debugForceWave;

        private List<JesterData> integratedEvents = new List<JesterData>();

        public event Action FinishedLevel;

        public float SetDebugTimestamp;
        // Spawns jesters either on the left side or right side and uses a random Y axis.
        void Start()
        {
            #if UNITY_EDITOR
            if (debugForceWave > 0)
            {
                waveNumber = debugForceWave - 1;
            }
            #endif
            LaunchNewWave();
        }

        private void SpawnJugglingBall()
        {
            GameObject ball = Instantiate(Resources.Load($"CircusBall") as GameObject);
        }

        private void LaunchNewWave()
        {
            ResetTime();
            waveEnded = false;
            waveEndTime = 5;
            currentWave = waves.waves[waveNumber];
            #if UNITY_EDITOR
                Timestamp = SetDebugTimestamp;
#endif
            if (!GameObject.Find("CircusBall") && waves.waves[waveNumber].SpawnBall)
            {
                SpawnJugglingBall();
            }
            CalculateWaveTime();
            StartCoroutine(PauseTimestamp(currentWave.StartDelay));
        }

        private void CalculateWaveTime()
        {
            float highest = 0;
            foreach (JesterData wave in currentWave.jesters)
            {
                if (wave.timestamp > highest)
                {
                    highest = wave.timestamp;
                }
            }
            waveEndTime = highest;
        }

        IEnumerator PauseTimestamp(float delay)
        {
            PausedByWave = true;
            yield return new WaitForSeconds(delay);
            PausedByWave = false;
        }

        public void TimestampTick()
        {
            if (finished)
            {
                return;
            }
            foreach (JesterData wave in currentWave.jesters)
            {
                if (Mathf.Approximately(wave.timestamp, Timestamp))
                {
                    SpawnJester(wave);
                }
            }
            foreach (JesterData data in integratedEvents)
            {
                if (Mathf.Approximately(data.timestamp, Timestamp))
                {
                    SpawnJester(data);
                }
            }
            foreach (SegmentData segment in currentWave.segments)
            {
                if (Mathf.Approximately(segment.timestamp, Timestamp))
                {
                    IntegrateSegment(segment);
                }
            }
            if (Timestamp == waveEndTime)
            {
                waveEnded = true;
            }
            if (waveEnded)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && Timestamp > waveEndTime + 2)
                {
                    waveNumber++;
                    if (waveNumber == waves.waves.Length)
                    {
                        FinishedLevel?.Invoke();
                        finished = true;
                    }
                    else
                    {
                        LaunchNewWave();
                    }
                }
            }
        }

        void Update()
        {
            if (currentTick != Timestamp)
            {
                currentTick = Timestamp;
                TimestampTick();
                spawnDebounce = true;
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (JesterFever && spawnDebounce)
                {
                    spawnDebounce = false;
                    JesterData jesterData = new JesterData();
                    jesterData.timestamp = currentTick + 0.5f;
                    jesterData.randomY = true;
                    jesterData.side = Sides.Random;

                    ShotDataObject shotData = new ShotDataObject();
                    shotData.fireBetween = UnityEngine.Random.Range(0.5f, 1.5f);
                    shotData.speed = UnityEngine.Random.Range(7, 15);
                    shotData.amount = 99;
                    shotData.damage = 6;

                    jesterData.commands = new[] {
                        new JesterCommand()
                        {
                            action = Actions.FireAimed,
                            shotData = shotData
                        }
                    };
                    SpawnJester(jesterData);
                }
            }
        }

        void IntegrateSegment(SegmentData segment)
        {
            foreach (JesterData data in segment.segment.events)
            {
                JesterData separated = JsonUtility.FromJson<JesterData>(JsonUtility.ToJson(data));
                separated.timestamp = segment.timestamp + separated.timestamp;
                print(separated.timestamp);
                integratedEvents.Add(separated);
            }
        }

        void SpawnJester(JesterData waveObject)
        {
            float x = 0;
            Sides wave = waveObject.side;
            if (wave == Sides.OppositeOfLast)
            {
                if (LastUsed == Sides.Right)
                {
                    wave = Sides.Left;
                }
                else
                {
                    wave = Sides.Right;
                }
            }
            else if (wave == Sides.CopyLast)
            {
                wave = LastUsed;
            }

            if (wave == Sides.Left)
            {
                x = X_LEFT;
                LastUsed = Sides.Left;
            }
            else if (wave == Sides.Right)
            {
                x = X_RIGHT;
                LastUsed = Sides.Right;
            }
            else if (wave == Sides.Random)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    x = X_LEFT;
                    LastUsed = Sides.Left;
                }
                else
                {
                    x = X_RIGHT;
                    LastUsed = Sides.Right;
                }
            }
            float y;
            if (waveObject.randomY)
            {
                y = UnityEngine.Random.Range(-5.0f, 5.0f);
            }
            else
            {
                y = waveObject.y;
            }
            GameObject newJester = Instantiate(jester, new Vector3(x, y), jester.transform.rotation);
            newJester.GetComponent<JesterBehaviour>().enterTimestamp = Timestamp;
            newJester.GetComponent<JesterBehaviour>().jesterCommands = waveObject.commands;
            newJester.GetComponent<JesterAnimator>().SetColor(waveObject.color);
        }
    }
}
