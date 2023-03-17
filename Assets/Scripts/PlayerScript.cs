using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float jumpStrength;
    public float speedStrength;
    private float horizontal;
    public SpriteRenderer sprite;

    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;

    public bool isOnPlatform;
    public Rigidbody2D platformRb;

    public bool flipX;

    MovingPlatformController movingPlatformController;
    Rigidbody2D movingPlaftormRb;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        movingPlatformController = GetComponent<MovingPlatformController>();
        movingPlaftormRb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        //grounded check, tie skaiciai su f yra hardcoded, tu scenoje ant isGrounded object uzdek CapsuleColider, settink i horizontal ir kokie skaiciukai tokius duek kad jump hitbox
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(4f, 0.2f), 0, groundLayer);
        horizontal = Input.GetAxisRaw("Horizontal");
        myRigidbody.velocity = new Vector2(horizontal * speedStrength, myRigidbody.velocity.y);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpStrength);
        }
        if (Input.GetButtonUp("Jump") && myRigidbody.velocity.y > -1f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f);
        }
        if (Input.GetKey(KeyCode.A))
        {
                myRigidbody.velocity = new Vector2(-speedStrength, myRigidbody.velocity.y);
            sprite.flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            myRigidbody.velocity = new Vector2(speedStrength, myRigidbody.velocity.y);
            sprite.flipX = false;
        }



        if (isOnPlatform)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x + platformRb.velocity.x, myRigidbody.velocity.y);
        }
        if (isGrounded)
        {
            movingPlatformController.jumpedOffMovingPlatform = false;
        }
        if (movingPlatformController.jumpedOffMovingPlatform == true)
        {
            myRigidbody.AddForce(transform.right * movingPlaftormRb.velocity.x);
        }


    }
    
}
