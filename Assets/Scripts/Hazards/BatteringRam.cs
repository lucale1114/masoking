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

    private bool began;
    private bool canHit = true;
    public int dirX;
    public int dirY;
    public bool launchMode;

    Collider2D ramHead;

    private void Awake()
    {
        _indicator = transform.parent.GetChild(1);
        _indicator.gameObject.SetActive(false);
    }
    void Start()
    {
        _originalPos = transform.position;
        transform.localScale *= data.size;
        ramHead = GetComponents<Collider2D>()[0];
        if (data.launched) {
            transform.position += new Vector3(2f * dirX * data.size, 2f * dirY * data.size, 0);
        }
        //_indicator.localScale *= data.size;
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
        if (launchMode)
        {
            transform.DOMove(transform.position + new Vector3(30 * -dirX, 30 * -dirY), 8f / data.launchSpeedMultiplier).SetEase(Ease.OutQuint);
        }
        else
        {
            transform.DOMove(_indicator.GetChild(0).position + new Vector3(1f * dirX * data.size, 1f * dirY * data.size, 0), 0.35f).SetEase(Ease.InSine).OnComplete(() =>
            {
                canHit = false;
                StartCoroutine("Retract");
            });
        }
           
        Destroy(_indicator.gameObject);
    }

    private void TimestampTick()
    {
        if (Mathf.Approximately(WaveHandler.Timestamp, data.timestamp) && !began)
        {
            BeginRam();
            began = true;
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
            transform.DOMove(transform.position + new Vector3(0.5f * dirX * data.size, 0.5f * dirY * data.size, 0), 0.15f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(transform.parent.gameObject);
    }

    private void AllowHit()
    {
        canHit = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canHit && ramHead.IsTouching(collision.collider))
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                canHit = false;
                GameObject player = collision.collider.gameObject;
                Movement movement = player.GetComponent<Movement>();
                movement.Knocked(0.5f, transform.up);
                //player.GetComponent<Rigidbody2D>().velocity = transform.forward * 500;
                player.GetComponent<HeatSystem>().ChangeHeat(data.damage);
                if (launchMode) { 
                    Invoke("AllowHit", 0.3f);
                }
            }
        }
    }
}
