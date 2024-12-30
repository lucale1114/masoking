using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesterTeacher : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] float speed = 3;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rb.velocity = Vector2.right * speed;
    }
}
