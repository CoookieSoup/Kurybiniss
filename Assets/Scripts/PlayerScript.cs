using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float jumpStrength;
    public float speedStrength;
    private float horizontal;

    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;

    public bool isOnPlatform;
    public Rigidbody2D platformRb;
    public bool isLastContactMovingPlatform;
    private Time timer;
    MovingPlatformController movingPlatformController;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

    }
    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Platform")
        {
            isLastContactMovingPlatform = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //grounded check, tie skaiciai su f yra hardcoded, tu scenoje ant isGrounded object uzdek CapsuleColider, settink i horizontal ir kokie skaiciukai tokius duek kad jump hitbox
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(4.38f, 0.17f), CapsuleDirection2D.Horizontal, 0, groundLayer);
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
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            myRigidbody.velocity = new Vector2(speedStrength, myRigidbody.velocity.y);
        }

        if (isOnPlatform)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x + platformRb.velocity.x, myRigidbody.velocity.y);
            isLastContactMovingPlatform = true;
        }
        if (isLastContactMovingPlatform)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x + movingPlatformController.rb.velocity.x, myRigidbody.velocity.y);
        }

    }
    
}
