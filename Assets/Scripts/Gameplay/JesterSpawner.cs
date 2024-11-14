using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveData;

public class JesterSpawner : MonoBehaviour
{
    const float X_LEFT = -9.5f;
    const float X_RIGHT = 9.5f;
    private float currentTick;

    public GameObject jester;
    public WaveSpawning[] wave1;

    public WaveSpawning[] currentWave;
    // Spawns jesters either on the left side or right side and uses a random Y axis.
    void Start()
    {
        currentWave = wave1;

    }

    public void TimestampTick()
    {
        foreach (WaveSpawning wave in currentWave)
        {
            if (Mathf.Approximately(wave.timestamp, Timestamp))
            {
                SpawnJester(wave.side, wave.y, wave.commands);
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

    void SpawnJester(Sides side, float y, JesterCommand[] commands)
    {
        float x;
        if (side == Sides.Left)
        {
            x = X_LEFT;
        }
        else if (side == Sides.Right)
        {
            x = X_RIGHT;
        }
        else
        {
            if (Random.Range(0, 2) == 1)
            {
                x = X_LEFT;
            }
            else
            {
                x = X_RIGHT;
            }
        }

        GameObject newJester = Instantiate(jester, new Vector3(x, y), jester.transform.rotation);
        newJester.GetComponent<JesterBehaviour>().jesterCommands = commands;
    }
}
