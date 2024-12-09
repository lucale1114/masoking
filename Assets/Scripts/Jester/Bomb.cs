using Objects;
using Player;
using Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public BombJesterCollision BombJesterCollision;
    
    public static GameObject player;
    [SerializeField] AudioClip[] booms;

    [SerializeField] private float explosionRadius = 1.5f;

    //Vector3 playerPos = Player.transform.position;
    //Vector3 bombPos = bomb.transform.position;
    void Start()
    {
            // Find the player using a tag if not assigned
        player = GameObject.FindGameObjectWithTag("Player");

        

        if (player == null)
        {
            UnityEngine.Debug.LogError("Player not assigned or found!");
        }

    }


    public void Update()
    {
        if (BombJesterCollision.HasDashed)
        {
            Invoke(nameof(WaitForExplosion), 3f);
            
        }
       
    }

    public void WaitForExplosion()
    {
        SoundFXManager.Instance.PlayRandomSoundFX(booms, transform, 1f);

        if (player != null)
        {
            // Calculate distance between bomb and player
            //float distance = Vector3.Distance(transform.position, player.transform.position);
            float sqrDistance = Vector3.SqrMagnitude(transform.position - player.transform.position);


            if (sqrDistance <= explosionRadius * explosionRadius)
            {
                UnityEngine.Debug.Log("Player is in range of the explosion!");
                // Apply damage
                HeatSystem playerHeat = player.GetComponent<HeatSystem>();
                if (playerHeat != null)
                {
                    int damage = 10;
                    playerHeat.ChangeHeat(damage);
                    UnityEngine.Debug.Log($"Player took {damage} damage from the explosion.");
                }
                else
                {
                    UnityEngine.Debug.Log("HeatSystem component not found on Player!");
                }
            }
            else
            {
                           
                UnityEngine.Debug.Log("Player is out of range of the explosion.");
            }
        }
        Destroy(gameObject);
    }

}
