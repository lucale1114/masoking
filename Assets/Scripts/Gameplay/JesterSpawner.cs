using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterSpawner : MonoBehaviour
{
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
            SpawnJester(new Vector3(-7, Random.Range(5, -5), 0));
        }
    }
    void SpawnJester(Vector3 position)
    {
        GameObject newJester = Instantiate(jester, position, jester.transform.rotation);
        Destroy(newJester, 15);
    }
}
