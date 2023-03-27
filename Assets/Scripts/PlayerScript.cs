using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float jumpStrength;
    public float speedStrength;
    private float horizontal;
    private float jumpTime = 0.1f;
    private float jumpCounter;
    private bool hasJumped = false;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isGrounded;

    public bool isOnPlatform;
    public Rigidbody2D platformRb;

    public SpriteRenderer sprite;
    public Animator animator;
    public bool flipX;

    public bool isWallSliding;
    public float wallSlideSpeed = 7f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.05f;
    private float wallJumpingCounter;
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

    // Wall jump logic start
    private void WallJump()
    {
        if (isWallSliding)
        {
            if (flipX == true)
            {
                wallJumpingDirection = -1f;
            }
            if (flipX == false)
            {
                wallJumpingDirection = 1f;
            }
            wallJumpingCounter = wallJumpingTime;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && myRigidbody.velocity.y <= 0f)
        {
            myRigidbody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x * 2, wallJumpingPower.y * 2);
            wallJumpingCounter = 0f;
            if(flipX == false && wallJumpingDirection == -1)
            {
                flipX = true;
            }
            if (flipX == true && wallJumpingDirection == 1)
            {
                flipX = false;
            }
        }

    }

    // Wall jump logic end

    // Coyote time logic start
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && myRigidbody.velocity.y <= 0f)
        {
            jumpCounter = jumpTime;
        }
        if (collision.collider.CompareTag("Ground") && myRigidbody.velocity.y > 0f)
        {
            wallJumpingCounter = 0f;
        }
        
    }
    private void ExtendedJump()
    {
        jumpCounter -= Time.deltaTime;
        if (isGrounded && myRigidbody.velocity.y == 0f)
        {
            hasJumped = false;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            hasJumped = true;
        }
        if (Input.GetButtonDown("Jump") && isGrounded == false && jumpCounter > 0f && hasJumped == false)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpStrength);
            jumpCounter = 0f;
            hasJumped = true;
        }
    }

    // Coyote time logic end



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
        if (isWallSliding) 
        {
            animator.SetBool("isWallSliding", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
        if (!isWallSliding) 
        {
            animator.SetBool("isWallSliding", false);
        }
        if (myRigidbody.velocity.y < 0f && !isWallSliding)
        {
            animator.SetBool("isWallSliding", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        if (isGrounded) 
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
        if (!isGrounded && myRigidbody.velocity.y > 0f && isWallSliding)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("isWallSliding", false);
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
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            myRigidbody.velocity = new Vector2(-speedStrength, myRigidbody.velocity.y);
            sprite.flipX = true;
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
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
        ExtendedJump();
    }
    
}
