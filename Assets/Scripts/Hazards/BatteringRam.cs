using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using UnityEngine;
using Wave.Handler;
using static Wave.WaveData;

public class BatteringRam : MonoBehaviour
{
    
    private Transform _indicator;
    private Vector3 _originalPos;

    public BatteringRamData data;

    private bool canHit = true;
    public int dirX = 1;
    public int dirY = 1;

    void Start()
    {
        _indicator = transform.parent.GetChild(1);
        _originalPos = transform.position;

        Invoke("SpawnedRam", 2);
    }

    // Update is called once per frame
    void SpawnedRam()
    {
        canHit = true;
        transform.DOMove(_indicator.GetChild(0).position, 0.35f).SetEase(Ease.InSine).OnComplete(() => {
            canHit = false;
            StartCoroutine("Retract");
        });
        Destroy(_indicator.gameObject);
    }

    IEnumerator Retract()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 8; i++)
        {
            transform.DOMove(transform.position + new Vector3(0.5f * dirX, 0.5f * dirY, 0), 0.15f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(transform.parent.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision);
        if (canHit)
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                canHit = false;
                GameObject player = collision.collider.gameObject;
                Movement movme = player.GetComponent<Movement>();
                player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                
                player.GetComponent<HeatSystem>().ChangeHeat(100);
            }
        }
    }
}
