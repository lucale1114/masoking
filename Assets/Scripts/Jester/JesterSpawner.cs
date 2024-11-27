using System;
using Gameplay;
using UnityEngine;
using static WaveData;


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
        public GameObject jester;
        public WaveList waves;
        public Gameplay.Wave currentWave;
        public int debugForceWave;

        public event Action FinishedLevel;

        public float SetDebugTimestamp;
        // Spawns jesters either on the left side or right side and uses a random Y axis.
        void Start()
        {
            /*foreach (Gameplay.Wave wave in waves.waves)
            {
                foreach (JesterData command in wave.jesters)
                {
                    float timestampspawn = command.timestamp;
                    foreach (JesterCommand com in command.commands)
                    {
                        com.timestamp = Mathf.Round((com.timestamp - timestampspawn) * 10.0f) * 0.1f;
                    }
                }
            }*/
#if UNITY_EDITOR
            if (debugForceWave > 0)
            {
                waveNumber = debugForceWave - 1;
            }
            #endif
            LaunchNewWave();
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
            CalculateWaveTime();
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

        public void TimestampTick()
        {
            foreach (JesterData wave in currentWave.jesters)
            {
                if (Mathf.Approximately(wave.timestamp, Timestamp))
                {
                    SpawnJester(wave);
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
                if (!JesterFever && spawnDebounce)
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
                    shotData.damage = 1;

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

        // Temporarily just spawns them in waves now.

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
        }
    }
}

/*#if UNITY_EDITOR
[CustomEditor(typeof(JesterSpawner))]
class WaveEditor: Editor {
    public Actions actions;
    public float y;
    public float timestamp;

    SerializedProperty wave1;

    private void OnEnable()
    {
        wave1.FindPropertyRelative("wave1");
    }


    int index = 0;
    public override void OnInspectorGUI()
    {
        var waveEdior = (JesterSpawner)target;
        if (waveEdior == null ) { return; }

        EditorGUI.PropertyField(new Rect(1,1,5,5) ,wave1);
        if (GUILayout.Button("New Jester Spawn"))
        {
            WaveSpawning newJester;
        }

        EditorGUILayout.LabelField("Y Position", EditorStyles.boldLabel);
        y = EditorGUILayout.Slider(y, -2, 2);
        EditorGUILayout.LabelField("Timestamp", EditorStyles.boldLabel);
        timestamp = EditorGUILayout.FloatField(timestamp);

        */ /*// Draw the default inspector
        DrawDefaultInspector();
        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
        var someClass = target as SomeClass;
        // Update the selected choice in the underlying object
        someClass.choice = _choices[_choiceIndex];
        // Save the changes back to the object
        EditorUtility.SetDirty(target);*/ /*
    }
}
#endif*/