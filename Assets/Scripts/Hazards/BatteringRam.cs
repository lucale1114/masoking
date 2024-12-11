using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using UnityEngine;
using Wave.Handler;
using Wave.Hazards.Ram;

public class BatteringRam : MonoBehaviour
{
    
    private Transform _indicator;
    private Vector3 _originalPos;
    private float _currentTick;

    public BatteringRamData data;

    private bool canHit = true;
    public int dirX;
    public int dirY;

    private void Awake()
    {
        _indicator = transform.parent.GetChild(1);
        _indicator.gameObject.SetActive(false);
    }
    void Start()
    {
        print(data.side);
        print(dirY);
        _originalPos = transform.position;
    }

    void BeginRam()
    {
        _indicator.gameObject.SetActive(true);
        Invoke("SpawnedRam", data.delay);
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

    private void TimestampTick()
    {
        if (Mathf.Approximately(WaveHandler.Timestamp, data.timestamp))
        {
            BeginRam();
        }
    }


    private void Update()
    {
        if (!Mathf.Approximately(_currentTick, WaveHandler.Timestamp))
        {
            _currentTick = WaveHandler.Timestamp;
            TimestampTick();
        }
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
        if (canHit)
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                canHit = false;
                GameObject player = collision.collider.gameObject;
                Movement movement = player.GetComponent<Movement>();
                movement.Knocked(0.5f, transform.forward);
                //player.GetComponent<Rigidbody2D>().velocity = transform.forward * 500;
                player.GetComponent<HeatSystem>().ChangeHeat(data.damage);
            }
        }
    }
}
