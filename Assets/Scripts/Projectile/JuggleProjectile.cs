using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class JuggleProjectile : MonoBehaviour
{
    const float DEBOUNCE_TIME = 0.5f;
    private bool debounce = true;
    Transform player;
    HeatSystem heatSystem;
    AudioSource sound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        heatSystem = player.GetComponent<HeatSystem>();
        sound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform == player && debounce)
        {
            if (transform.position.y - 1 > player.transform.position.y)
            {
                debounce = false;
                heatSystem.Hit();
                sound.Play();
                Invoke("DebounceFunction", DEBOUNCE_TIME);
            }
        }
    }

    void DebounceFunction()
    {
        debounce = true;
    }
}
