using Gameplay;
using UnityEngine;
using static WaveData;


#if UNITY_EDITOR
#endif

namespace Jester
{
    public class JesterSpawner : MonoBehaviour
    {
        const float X_LEFT = -9.5f;
        const float X_RIGHT = 9.5f;
        private float currentTick;

        public GameObject jester;
        public WaveList waves;

        public Gameplay.Wave currentWave;
        public float SetDebugTimestamp;
        // Spawns jesters either on the left side or right side and uses a random Y axis.
        void Start()
        {
            currentWave = waves.waves[0];
            print(currentWave.name);
            #if UNITY_EDITOR
                Timestamp = SetDebugTimestamp;
            #endif
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
        }

        void Update()
        {
            if (currentTick != Timestamp)
            {
                currentTick = Timestamp;
                TimestampTick();
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
                if (Random.Range(0, 2) == 1)
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
                y = Random.Range(-5, 5);
            }
            else
            {
                y = waveObject.y;
            }
            GameObject newJester = Instantiate(jester, new Vector3(x, y), jester.transform.rotation);
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