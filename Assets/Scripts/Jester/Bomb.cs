using Objects;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    Rigidbody2D rb;
    bool beenHit = false;
    bool fullHit = false;   
    public BombJesterCollision BombJesterCollision;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BombJesterCollision.HasDashed)
        {
            StartCoroutine(WaitForExplosion());
            Destroy(gameObject, 3f);

        }
    }

    public IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(2.5f);
        fullHit = true;
       
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && fullHit == true && beenHit != true )
        {

            UnityEngine.Debug.Log("Hit");
            var damage = 10;
            HeatSystem playerHeat = collision.gameObject.GetComponent<HeatSystem>();

            if (playerHeat != null)
            {
                UnityEngine.Debug.Log("Heat");
                playerHeat.ChangeHeat(damage);
                beenHit = true;
            }
            else
            {
                UnityEngine.Debug.LogWarning("HeatSystem not found on Player!");
            }
        }
    }



}
