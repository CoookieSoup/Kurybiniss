using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Vector2 platfromVelWithPlayerCache;
    private bool jumpedOffMovingPlatform;

    public SpriteRenderer sprite;
    public Animator animator;
    public bool flipX;

    public bool isWallSliding;
    public float wallSlideSpeed = 7f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpingDirection;
    private readonly float wallJumpingTime = 0.05f;
    private float wallJumpingCounter;
    private Vector2 wallJumpingPower = new Vector2(10f, 20f);

    public float maxHealth = 4f;
    public float currentHealth = 4f;
    [SerializeField] private Image healthBar;
    public float defaultInvincibilityTimer;
    public float currentInvincibilityTimer;
    public bool tookDamage;
    public bool canMove;
    public float noInputTimeAfterTakingDamage = 0.5f;

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

    // Coyote time logic start && off movement platform start
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
        if (collision.collider.gameObject.name == "MovingPlatform")
        {
            myRigidbody.velocity = new Vector2 (platfromVelWithPlayerCache.x, platfromVelWithPlayerCache.y + myRigidbody.velocity.y);
            jumpedOffMovingPlatform = true;
        }


    }
    // Coyote time logic start && off movement platform end
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

    // Health system start
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && tookDamage == false)
        {
            canMove = false;
            tookDamage = true;
            currentInvincibilityTimer = defaultInvincibilityTimer;
            currentHealth--;
            if (currentHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            healthBar.fillAmount = (currentHealth / maxHealth);
            if (collider.gameObject.transform.position.x < myRigidbody.position.x)
            {
                myRigidbody.velocity = new Vector2(speedStrength, jumpStrength);
            }
            if (collider.gameObject.transform.position.x > myRigidbody.position.x)
            {
                myRigidbody.velocity = new Vector2(-speedStrength, jumpStrength);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collider)
    {
        if (currentInvincibilityTimer < 0f && collider.gameObject.CompareTag("Enemy"))
        {
            canMove = false;
            tookDamage = true;
            currentInvincibilityTimer = defaultInvincibilityTimer;
            currentHealth--;
            if (currentHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            healthBar.fillAmount = (currentHealth / maxHealth);
            if (collider.gameObject.transform.position.x < myRigidbody.position.x)
            {
                myRigidbody.velocity = new Vector2(20f, 40f);
            }
            if (collider.gameObject.transform.position.x > myRigidbody.position.x)
            {
                myRigidbody.velocity = new Vector2(-20f, 40f);
            }
        }
    }
    // Health system end

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentInvincibilityTimer = defaultInvincibilityTimer;
        canMove = true;
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(2.2f, 0.2f), 0, groundLayer);
        horizontal = Input.GetAxisRaw("Horizontal");
        if(myRigidbody.velocity.x < 0f && !canMove && flipX)
        {
            animator.SetBool("TakeDMGBack", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);

        }
        if (myRigidbody.velocity.x < 0f && !canMove && !flipX)
        {
            animator.SetBool("TakeDMGFront", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
        if (myRigidbody.velocity.x > 0f && !canMove && flipX)
        {
            animator.SetBool("TakeDMGFront", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
        if (myRigidbody.velocity.x > 0f && !canMove && !flipX)
        {
            animator.SetBool("TakeDMGBack", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
        if (canMove)
        {
            myRigidbody.velocity = new Vector2(horizontal * myRigidbody.velocity.x, myRigidbody.velocity.y); //changed speedStrength to myRigidbody.velocity.x here
            animator.SetBool("TakeDMGBack", false);
            animator.SetBool("TakeDMGFront", false);
        }
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        if (isWallSliding) 
        {
            animator.SetBool("isWallSliding", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
            jumpedOffMovingPlatform = false;
        }
        if (!isWallSliding) 
        {
            animator.SetBool("isWallSliding", false);
        }
        if (myRigidbody.velocity.y < 0f && !isWallSliding && canMove)
        {
            animator.SetBool("isWallSliding", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        if (isGrounded && myRigidbody.velocity.y == 0f)
        {
            jumpedOffMovingPlatform = false;
        }
        if (isGrounded) 
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
            jumpedOffMovingPlatform = false;
        }
        if (currentInvincibilityTimer < defaultInvincibilityTimer - noInputTimeAfterTakingDamage)
        {
            canMove = true;
        }
        if (!isGrounded && myRigidbody.velocity.y > 0f && isWallSliding && canMove)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("isWallSliding", false);
        }
        if (!isGrounded && myRigidbody.velocity.y > 0f && canMove)
        {
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Jump") && isGrounded && canMove)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpStrength);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }
        if (Input.GetButtonUp("Jump") && myRigidbody.velocity.y > -1f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f);
        }
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && canMove)
        {
            if (myRigidbody.velocity.x > speedStrength)
            {
                myRigidbody.velocity = new Vector2(-speedStrength, myRigidbody.velocity.y);
            }
            if (jumpedOffMovingPlatform == true && myRigidbody.velocity.x <= -speedStrength)
            {
                myRigidbody.velocity = new Vector2(platfromVelWithPlayerCache.x, myRigidbody.velocity.y);
            }
            if (jumpedOffMovingPlatform == false)
            {
                myRigidbody.velocity = new Vector2(-speedStrength, myRigidbody.velocity.y);
            }
            sprite.flipX = true;
        }
        //&& Mathf.Abs(myRigidbody.velocity.x) <= speedStrength
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && canMove)
        {
            if (myRigidbody.velocity.x < -speedStrength)
            {
                myRigidbody.velocity = new Vector2 (speedStrength, myRigidbody.velocity.y);
            }
            if (jumpedOffMovingPlatform == true && myRigidbody.velocity.x >= speedStrength)
            {
                myRigidbody.velocity = new Vector2(platfromVelWithPlayerCache.x, myRigidbody.velocity.y);
            }
            if (jumpedOffMovingPlatform == false)
            {
                myRigidbody.velocity = new Vector2(speedStrength, myRigidbody.velocity.y);
            }
            sprite.flipX = false;
        }
        if (isOnPlatform)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x + platformRb.velocity.x, myRigidbody.velocity.y);
            platfromVelWithPlayerCache = new Vector2(myRigidbody.velocity.x, platformRb.velocity.y);
        }

        WallSlide();
        WallJump();
        ExtendedJump();
        if (tookDamage)
        {
            currentInvincibilityTimer -= Time.deltaTime;
        }
        if (currentInvincibilityTimer <= 0f)
        {
            tookDamage = false;
        }
    }
    
}
