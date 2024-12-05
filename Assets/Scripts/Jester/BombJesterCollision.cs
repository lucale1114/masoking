using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Objects
{
    public class BombJesterCollision : MonoBehaviour
    {
        Rigidbody2D rb;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}