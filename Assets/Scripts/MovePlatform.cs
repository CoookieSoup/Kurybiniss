using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Platform")
        {
            speed = speed * -1;
        }
    }

    void FixedUpdate()
    {

        rb.velocity = Vector2.right * speed * Time.deltaTime;
    }
}