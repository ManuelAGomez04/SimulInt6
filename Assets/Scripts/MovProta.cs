using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovProta : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;
    public object Rigidbody2D;

    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = direction * speed;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    { 
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

}
