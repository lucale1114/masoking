using Objects;
using Player;
using Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public BombJesterCollision BombJesterCollision;
    
    public static GameObject Player;
    public static GameObject bomb;
    [SerializeField] AudioClip[] booms;

    [SerializeField] private float explosionRadius = 2f;

    //Vector3 playerPos = Player.transform.position;
    //Vector3 bombPos = bomb.transform.position;

    bool beenHit;
    bool notHit;


    void Start()
    {
            // Find the player using a tag if not assigned
            Player = GameObject.FindGameObjectWithTag("Player");

        beenHit = false;
        notHit = false; 

        if (Player == null)
        {
            UnityEngine.Debug.LogError("Player not assigned or found!");
        }

    }


    public void LateUpdate()
    {
        BombStart();
    }

    public void BombStart()
    {
        if (BombJesterCollision.HasDashed)
        {
            StartCoroutine(WaitForExplosion());
            Destroy(this.gameObject, 3);

        }

    }

    public IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(2.8f);
        //Vector2.Distance(bombPos, playerPos);

        if (Player != null)
        {
            // Calculate distance between bomb and player
            float distance = Vector3.Distance(transform.position, Player.transform.position);

            if (distance <= explosionRadius && beenHit != true || notHit != true)
            {
                beenHit = true;
                

                UnityEngine.Debug.Log("Player is in range of the explosion!");

                // Apply damage
                HeatSystem playerHeat = Player.GetComponent<HeatSystem>();
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
                notHit=true;
                SoundFXManager.Instance.PlayRandomSoundFX(booms, transform, 1f);
                UnityEngine.Debug.Log("Player is out of range of the explosion.");
            }
        }

        /*if (playerPos.x < 1 || playerPos.y < 1 && beenHit != true)
        {
            beenHit = true;
            UnityEngine.Debug.Log("Hit");
            var damage = 10;
            HeatSystem playerHeat = Player.GetComponent<HeatSystem>();

            UnityEngine.Debug.Log("HitNOW");


            if (playerHeat != null)
            {
                UnityEngine.Debug.Log("Heat");
                playerHeat.ChangeHeat(damage);
             
            }
            else
            {
                UnityEngine.Debug.LogWarning("HeatSystem not found on Player!");
            }

        }*/


    }

}
