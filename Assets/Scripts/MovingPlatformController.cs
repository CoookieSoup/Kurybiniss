using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public Transform posA, posB;
    public int Speed;
    Vector3 targetPos;

    PlayerScript movementController;
    public Rigidbody2D rb;
    Vector3 moveDirection;

    public Rigidbody2D playerRb;

    private void Awake()
    {
        movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        targetPos = posB.position;
        DirectionCalculate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < .1f)
        {
            targetPos = posB.position;
            DirectionCalculate();
        }

        if (Vector2.Distance(transform.position, posB.position) < .1f)
        {
            targetPos = posA.position;
            DirectionCalculate();
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = moveDirection * Speed;
    }

    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            movementController.isOnPlatform = true;
            movementController.platformRb = rb;
        }
    }
    //note to self play with this for momentum, if deleted close to good
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movementController.isOnPlatform = false;
            movementController.platformRb = rb;
        }
    }
    
}
