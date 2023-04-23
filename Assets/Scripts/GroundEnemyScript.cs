using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundEnemyScript : MonoBehaviour
{
    //RaycastHit2D hit;
    public RaycastHit2D hitUpper;
    public Transform playerPos;
    private Rigidbody2D GroundEnemyRb;
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
    public Animator animator;
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
        if (hitUpper.collider.gameObject.CompareTag("Player") && Mathf.Abs(playerPos.position.x - transform.position.x) < EnemyDetectRange) //31
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
        if (!hitUpper.collider.gameObject.CompareTag("Player"))
        {
            if (lastSeenPos.x - transform.position.x < -1f)
            {
                GroundEnemyRb.velocity = new Vector2(-GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
            if (lastSeenPos.x - transform.position.x > 1f)
            {
                GroundEnemyRb.velocity = new Vector2(GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
            if (Mathf.Abs(transform.position.x - lastSeenPos.x) <= 1f)
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
        if (!hasSeenPlayer)
        {
            GroundEnemyRb.velocity = new Vector2(GroundEnemySpeedStrength * patrolDirection, GroundEnemyRb.velocity.y);
        }
    }
    
}
