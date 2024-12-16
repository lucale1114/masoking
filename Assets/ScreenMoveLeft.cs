using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMoveLeft : MonoBehaviour
{
    [SerializeField] float speed = 5F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }
}
