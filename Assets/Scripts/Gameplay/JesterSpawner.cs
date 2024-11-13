using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterSpawner : MonoBehaviour
{
    const int X_LEFT = -9;
    const int X_RIGHT = 9;

    public GameObject jester;

    void Start()
    {
        StartCoroutine(WaveStart());
    }

    IEnumerator WaveStart()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(Random.Range(2, 5));
            int x;
            if (Random.Range(0,2) == 1)
            {
                x = X_LEFT;
            }
            else
            {
                x = X_RIGHT;
            }
            SpawnJester(new Vector3(x, Random.Range(5, -5), 0));
        }
    }
    void SpawnJester(Vector3 position)
    {
        GameObject newJester = Instantiate(jester, position, jester.transform.rotation);
        Destroy(newJester, 15);
    }
}
