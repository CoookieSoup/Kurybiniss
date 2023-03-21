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
    public bool isGrounded;

    public bool isOnPlatform;
    public Rigidbody2D platformRb;

    public SpriteRenderer sprite;
    public Animator animator;
    public bool flipX;

    private bool isWallSliding;
    public float wallSlideSpeed = 7f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(10f, 20f);




    // Wall slide logic start

    private bool IsTouchingWall()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(4.1f, 4f), 0, groundLayer);
    }

    private void WallSlide ()
    {
        if (IsTouchingWall() && isGrounded == false && horizontal != 0f)
        {
            isWallSliding = true;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Mathf.Clamp(myRigidbody.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    // Wall slide logic end

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            if (flipX == true)
            {
                wallJumpingDirection = -1f;
            }
            if (flipX == false)
            {
                wallJumpingDirection = 1f;
            }
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            myRigidbody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if(flipX == false && wallJumpingDirection == -1)
            {
                flipX = true;
            }
            if (flipX == true && wallJumpingDirection == 1)
            {
                flipX = false;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

    }
     private void StopWallJumping()
    {
        isWallJumping = false;
    }






    // Wall jump logic start

    // Wall jump logic end





    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(2.2f, 0.2f), 0, groundLayer);
        horizontal = Input.GetAxisRaw("Horizontal");
        myRigidbody.velocity = new Vector2(horizontal * speedStrength, myRigidbody.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        if (myRigidbody.velocity.y < 0f)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        if (isGrounded) 
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
        if (!isGrounded && myRigidbody.velocity.y > 0f)
        {
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpStrength);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
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


        WallSlide();
        WallJump();

    }
    
}
