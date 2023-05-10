using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundEnemyScript : MonoBehaviour
{
    public RaycastHit2D hitUpper;
    public Transform playerPos;
    public Rigidbody2D GroundEnemyRb;
    private Transform GroundEnemyLOSCheck;
    [SerializeField] private float GroundEnemySpeedStrength = 10f;
    [SerializeField] private float EnemyDetectRange;
    public float jumpStrength = 10f;
    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    public Transform groundCheck;
    public PlayerScript playerScript;
    public Collider2D enemyCollider2D;
    public Collider2D playerCollider2D;
    public Vector2 lastSeenPos;
    public float currentPatrolTime = 0f;
    public float patrolTurnAroundTime;
    public int patrolDirection = 1;
    public bool hasSeenPlayer = false;
    public Vector2 patrolOrigin;
    private bool hasReturnedToPatrolOrigin;
    public SpriteRenderer sprite;
    public Animator animator;
    public bool hasBeenHit;
    public float currentHitTime = 0f;
    public bool canMove = true;
    public int knockBackDirection;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GroundEnemyRb = GetComponent<Rigidbody2D>();
        GroundEnemyLOSCheck = GetComponent<Transform>();
        enemyCollider2D = GetComponent<Collider2D>();
        playerCollider2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        patrolOrigin = GroundEnemyLOSCheck.position;
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(enemyCollider2D, playerCollider2D, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenHit)
        {
            GroundEnemyRb.velocity = new Vector2(knockBackDirection * GroundEnemySpeedStrength, jumpStrength);
            currentHitTime = 1f;
            hasBeenHit = false;
            canMove = false;
        }
        currentHitTime -= Time.deltaTime;
        if (currentHitTime <= 0f && isGrounded)
        {
            canMove = true;
        }
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(2.2f, 0.2f), 0, groundLayer);
        hitUpper = Physics2D.Raycast(new Vector2 (GroundEnemyLOSCheck.position.x, GroundEnemyLOSCheck.position.y + 2f), playerPos.position - GroundEnemyLOSCheck.position);
        Debug.DrawRay(new Vector2(GroundEnemyLOSCheck.position.x, GroundEnemyLOSCheck.position.y + 2f), playerPos.position - GroundEnemyLOSCheck.position, Color.green);
        if (GroundEnemyRb.velocity.x != 0)
        {
            animator.SetBool("GolemSpeed", true);
        }
        if (GroundEnemyRb.velocity.x == 0)
        {
            animator.SetBool("GolemSpeed", false);
        }
        if (playerScript.tookDamage)
        {
            Physics2D.IgnoreCollision(enemyCollider2D, playerCollider2D, true);
        }
        if (hitUpper.collider.gameObject.CompareTag("Player") && Mathf.Abs(playerPos.position.x - transform.position.x) < EnemyDetectRange && canMove) //31
        {
            if (playerPos.position.x - transform.position.x < -1f)
            {
                GroundEnemyRb.velocity = new Vector2(-GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
            if (playerPos.position.x - transform.position.x > 1f)
            {
                GroundEnemyRb.velocity = new Vector2(GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
            if (Mathf.Abs(playerPos.position.x - transform.position.x) <= 1f)
            {
                GroundEnemyRb.velocity = new Vector2(0f, GroundEnemyRb.velocity.y);
            }
            lastSeenPos = playerPos.position;
            hasSeenPlayer = true; ;
        }
        if (!hitUpper.collider.gameObject.CompareTag("Player") && canMove)
        {
            if (lastSeenPos.x - transform.position.x < -30f)
            {
                GroundEnemyRb.velocity = new Vector2(-GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
            if (lastSeenPos.x - transform.position.x > 30f)
            {
                GroundEnemyRb.velocity = new Vector2(GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
            if (Mathf.Abs(transform.position.x - lastSeenPos.x) <= 30f)
            {
                GroundEnemyRb.velocity = new Vector2(0f, GroundEnemyRb.velocity.y);
                hasSeenPlayer = false;
            }
        }
        if (!playerScript.tookDamage)
        {
            Physics2D.IgnoreCollision(enemyCollider2D, playerCollider2D, false);
        }
        currentPatrolTime += Time.deltaTime;
        if (currentPatrolTime >= patrolTurnAroundTime)
        {
            patrolDirection = patrolDirection * -1;
            currentPatrolTime = 0f;
        }
        if (!hasSeenPlayer && canMove)
        {
            GroundEnemyRb.velocity = new Vector2(GroundEnemySpeedStrength * patrolDirection, GroundEnemyRb.velocity.y);
        }
        if (GroundEnemyRb.velocity.x > 0f)
        {
            sprite.flipX = false;
        }
        if (GroundEnemyRb.velocity.x < 0f)
        {
            sprite.flipX = true;
        }
    }
    
}
