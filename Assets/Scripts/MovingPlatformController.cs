using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public Transform posA, posB;
    public int Speed;
    Vector3 targetPos;

    PlayerScript playerScript;
    public Rigidbody2D rb;
    Vector3 moveDirection;

    public Rigidbody2D playerRb;

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
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
        if (Vector2.Distance(transform.position, posA.position) < 3f)
        {
            targetPos = posB.position;
            DirectionCalculate();
        }

        if (Vector2.Distance(transform.position, posB.position) < 3f)
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
            playerScript.isOnPlatform = true;
            playerScript.platformRb = rb;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerScript.isOnPlatform = false;
            playerScript.platformRb = rb;
        }
    }

    
}
