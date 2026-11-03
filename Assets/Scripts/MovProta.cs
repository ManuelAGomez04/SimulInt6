using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovProta : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;
    public object Rigidbody2D;

    Rigidbody2D rigidbody;
    public Animator Animator;
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
        float velocidadX = Input.GetAxisRaw("Horizontal")*Time.deltaTime;
        float velocidadY = Input.GetAxisRaw("Vertical")*Time.deltaTime;
        Animator.SetFloat("Caminar", velocidadX);
        if (velocidadX > 0)
        {
            gameObject.transform.localScale = new Vector3(-0.2415622f, 0.2082884f, 1);
           

        }
        else if (velocidadX < 0)
        {
            gameObject.transform.localScale = new Vector3(0.2415622f, 0.2082884f, 1);

        }
      
        Vector3 posicion = transform.position;

        transform.position = new Vector3(velocidadX*speed + posicion.x, posicion.y + velocidadY*speed, posicion.z);
       
    }

}
