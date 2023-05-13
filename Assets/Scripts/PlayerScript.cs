using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D myRigidbody;
    public float jumpStrength;
    public float speedStrength;
    private float horizontal;
    private float jumpTime = 0.1f;
    private float jumpCounter;
    private bool hasJumped = false;

    [HideInInspector] public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isGrounded;

    public bool isOnPlatform;
    [HideInInspector] public Rigidbody2D platformRb;
    [HideInInspector] public Vector2 platfromVelWithPlayerCache;
    public bool jumpedOffMovingPlatform;

    public SpriteRenderer sprite;
    public Animator animator;
    public bool flipX;

    public bool isWallSliding;
    public float wallSlideSpeed = 7f;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private Transform wallCheckRight;
    public LayerMask wallLayer;
    private float wallJumpingDirection;
    public float wallJumpingTime = 0.15f;
    private float wallJumpingCounter;
    private Vector2 wallJumpingPower = new Vector2(20f, 40f);
    public float lastWallslidedWallX;
    private float currentNoWallSlideOnSameWallTimer;
    public float defaultNoWallSlideOnSameWallTimer;
    public bool hasExitedWallSlideZone = false;
    public float lastWallJumpY;

    public float maxHealth = 4f;
    public float currentHealth;
    public Image healthBar;
    public float defaultInvincibilityTimer;
    private float currentInvincibilityTimer;
    public bool tookDamage;
    public bool canMove;
    public float noInputTimeAfterTakingDamage = 0.5f;

    CheckPointSystem checkPointSystem;

    public Transform cameraPos;

    public Scene currentScene;
    public int levelId;

    // Wall slide logic start

    private bool IsTouchingWallToLeft()
    {
        return Physics2D.OverlapBox(wallCheckLeft.position, new Vector2(1f, 4f), 0, groundLayer);
    }
    private bool IsTouchingWallToRight()
    {
        return Physics2D.OverlapBox(wallCheckRight.position, new Vector2(1f, 4f), 0, groundLayer);
    }

    private void WallSlide()
    {
        if (IsTouchingWallToLeft() && myRigidbody.velocity.x < 0f || IsTouchingWallToRight() && myRigidbody.velocity.x > 0f)
        {
            if (!isGrounded && horizontal != 0f )
            {
                    if (currentNoWallSlideOnSameWallTimer > 0f && transform.position.x < lastWallslidedWallX - 1f || currentNoWallSlideOnSameWallTimer > 0f && transform.position.x > lastWallslidedWallX + 1f || transform.position.x < lastWallslidedWallX - 1f || transform.position.x > lastWallslidedWallX + 1f || transform.position.x > lastWallslidedWallX - 1f && currentNoWallSlideOnSameWallTimer == defaultNoWallSlideOnSameWallTimer || transform.position.x < lastWallslidedWallX + 1f && currentNoWallSlideOnSameWallTimer == defaultNoWallSlideOnSameWallTimer) {
                        isWallSliding = true;
                        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Mathf.Clamp(myRigidbody.velocity.y, -wallSlideSpeed, float.MaxValue));
                        lastWallslidedWallX = transform.position.x;
                        currentNoWallSlideOnSameWallTimer = defaultNoWallSlideOnSameWallTimer;
                    }
            }
        }
        else
        {
            currentNoWallSlideOnSameWallTimer -= Time.deltaTime;
            isWallSliding = false;
        }
        if (lastWallJumpY - transform.position.y > 10f)
        {
            currentNoWallSlideOnSameWallTimer = -1f;
        }
        if (Physics2D.OverlapBox(wallCheckLeft.position, new Vector2(1f, 8f), 0, groundLayer) == false && Physics2D.OverlapBox(wallCheckRight.position, new Vector2(1f, 8f), 0, groundLayer) == false)
        {
            isWallSliding = false;
        }
        if (isGrounded)
        {
            animator.SetBool("isWallSliding", false);
            currentNoWallSlideOnSameWallTimer = defaultNoWallSlideOnSameWallTimer;
            lastWallslidedWallX = 100000f;
        }
    }
    // Wall slide logic end

    // Wall jump logic start
    private void WallJump()
    {
        if (isWallSliding)
        {
            hasExitedWallSlideZone = false;
            if (IsTouchingWallToLeft() && myRigidbody.velocity.x < 0f)
            {
                wallJumpingDirection = -1f;
                flipX = true;
            }
            if (IsTouchingWallToLeft() && myRigidbody.velocity.x == 0f)
            {
                wallJumpingDirection = 1f;
                flipX = false;
            }
            if (IsTouchingWallToRight() && myRigidbody.velocity.x > 0f)
            {
                wallJumpingDirection = 1f;
                flipX = false;
            }
            if (IsTouchingWallToRight() && myRigidbody.velocity.x == 0f)
            {
                wallJumpingDirection = -1f;
                flipX = true;
            }
            wallJumpingCounter = wallJumpingTime;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && myRigidbody.velocity.y <= 0f)
        {
            transform.position = new Vector2(-1f * wallJumpingDirection + transform.position.x, transform.position.y);
            isWallSliding = false;
            myRigidbody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            hasExitedWallSlideZone = true;
            lastWallJumpY = transform.position.y;
            currentNoWallSlideOnSameWallTimer = defaultNoWallSlideOnSameWallTimer;
            if (flipX == false && wallJumpingDirection == -1)
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
        if (collision.collider.gameObject.name == "MovingPlatform" && myRigidbody.velocity.y >= 0f && !isWallSliding && Mathf.Abs(myRigidbody.velocity.x) < Mathf.Abs(platfromVelWithPlayerCache.x))
        {
            myRigidbody.velocity = new Vector2(0f, jumpStrength);
            myRigidbody.AddForce(platfromVelWithPlayerCache, ForceMode2D.Impulse);
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
                PlayerPrefs.SetFloat("currentHealth", 0f);
                checkPointSystem.lastCheckpointPos = new Vector2(0f, 0f);
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
                PlayerPrefs.SetFloat("currentHealth", 0f);
                checkPointSystem.lastCheckpointPos = new Vector2(0f, 0f);
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
        checkPointSystem = GameObject.FindGameObjectWithTag("CheckpointSystem").GetComponent<CheckPointSystem>();
        currentHealth = PlayerPrefs.GetFloat("currentHealth");
        healthBar.fillAmount = (currentHealth / maxHealth);
        currentNoWallSlideOnSameWallTimer = 0f;
        cameraPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        if (checkPointSystem.lastCheckpointPos.x != 0f && checkPointSystem.lastCheckpointPos.y != 0f)
        {
            transform.position = checkPointSystem.lastCheckpointPos;
            cameraPos.position = checkPointSystem.lastCheckpointPos;
        }
        /*Scene currentScene = SceneManager.GetActiveScene();
        levelId = currentScene.buildIndex;*/
        levelId = SceneManager.GetActiveScene().buildIndex;



    }
/*    public void SetFloat(string levelId, int level)
    {
        level = SceneManager.GetActiveScene().buildIndex;
    }*/
    // Update is called once per frame
    void Update()
    {
        //PlayerPrefs.SetInt("levelId", levelId);
        Debug.Log(levelId);
        healthBar.fillAmount = (currentHealth / maxHealth);
        if (currentNoWallSlideOnSameWallTimer <= 0f)
        {
            lastWallslidedWallX = 1000000f;
        }
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(2.2f, 0.2f), 0, groundLayer);
        horizontal = Input.GetAxisRaw("Horizontal");
        if (myRigidbody.velocity.x < 0f && !canMove && flipX)
        {
            animator.SetBool("TakeDMGBack", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("TakeDMGFront", false);
        }
        if (myRigidbody.velocity.x < 0f && !canMove && !flipX)
        {
            animator.SetBool("TakeDMGFront", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("TakeDMGBack", false);
        }
        if (myRigidbody.velocity.x > 0f && !canMove && flipX)
        {
            animator.SetBool("TakeDMGFront", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("TakeDMGBack", false);
        }
        if (myRigidbody.velocity.x > 0f && !canMove && !flipX)
        {
            animator.SetBool("TakeDMGBack", true);
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("TakeDMGFront", false);
        }
        if (canMove && !jumpedOffMovingPlatform)
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
            if (myRigidbody.velocity.x > 0f)
            {
                myRigidbody.velocity = new Vector2(-speedStrength, myRigidbody.velocity.y);
            }
            if (jumpedOffMovingPlatform == false)
            {
                myRigidbody.velocity = new Vector2(-speedStrength, myRigidbody.velocity.y);
            }
            sprite.flipX = true;
            flipX = true;
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && canMove)
        {
            if (myRigidbody.velocity.x < 0f)
            {
                myRigidbody.velocity = new Vector2(speedStrength, myRigidbody.velocity.y);
            }
            if (jumpedOffMovingPlatform == false)
            {
                myRigidbody.velocity = new Vector2(speedStrength, myRigidbody.velocity.y);
            }

            sprite.flipX = false;
            flipX = false;
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
        if (isGrounded)
        {
            animator.SetBool("isWallSliding", false);
            isWallSliding = false;
        }

    }

}
