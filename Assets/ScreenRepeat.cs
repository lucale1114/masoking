using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRepeat : MonoBehaviour
{
    Vector2 startPos;
    float repeatWith;
    void Start()
    {
        startPos = transform.position;
        repeatWith = GetComponent<BoxCollider2D>().size.x / 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startPos.x - repeatWith)
        {
            transform.position =  startPos;
        }
    }
}
