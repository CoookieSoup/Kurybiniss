using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlyingEnemyScript2 : MonoBehaviour
{
    public float EnemyFollowSpeed;
    [HideInInspector] public Transform player;
    [HideInInspector] private Vector2 lastSeenPos;
    [HideInInspector] public Collider2D enemyCollider2D;
    [HideInInspector] public Collider2D playerCollider2D;
    [HideInInspector] public PlayerScript playerScript;
    RaycastHit2D LOSCheck;
    public float EnemyDetectRange;
    private bool hasSeen = false;
    [HideInInspector] public Rigidbody2D rigidBody2D;
    [HideInInspector] public bool hasBeenHit = false;
    [HideInInspector] public float currentKnockbackDuration;
    public float defaultKnockbackDuration;
    [HideInInspector] public bool hasResetKnockbackDuration;
    [HideInInspector] public Vector2 knockbackOrigin;
    public float knockbackMultiplier;
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyCollider2D = GetComponent<Collider2D>();
        playerCollider2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        currentKnockbackDuration = -1f;
    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(enemyCollider2D, playerCollider2D, true);
        }
        if (Physics2D.IsTouchingLayers(enemyCollider2D, playerScript.wallLayer))
        {
            hasBeenHit = false;
            currentKnockbackDuration = defaultKnockbackDuration;
            hasResetKnockbackDuration = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        LOSCheck = Physics2D.Raycast(transform.position, player.position - transform.position);
        Debug.DrawRay(transform.position, player.position - transform.position);
        if (LOSCheck.collider.gameObject.CompareTag("Player") && Mathf.Abs(player.position.x - transform.position.x) < EnemyDetectRange && !hasBeenHit)
        {
            Vector2 newPos = new Vector2(player.position.x, player.position.y);
            transform.position = Vector3.MoveTowards(transform.position, newPos, EnemyFollowSpeed * Time.deltaTime);
            lastSeenPos = player.position;
            hasSeen = true;
        }
        else {
            if (hasSeen && !hasBeenHit)
            {
                transform.position = Vector3.MoveTowards(transform.position, lastSeenPos, EnemyFollowSpeed * Time.deltaTime);
            }
        }
        if (!playerScript.tookDamage)
        {
            Physics2D.IgnoreCollision(enemyCollider2D, playerCollider2D, false);
        }
        if (hasBeenHit)
        {
            currentKnockbackDuration -= Time.deltaTime;
            hasResetKnockbackDuration = false;
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x + (transform.position.x - knockbackOrigin.x), transform.position.y + (transform.position.y - knockbackOrigin.y)), knockbackMultiplier * EnemyFollowSpeed * Time.deltaTime);
        }
        if (currentKnockbackDuration < 0f && !hasResetKnockbackDuration)
        {
            hasBeenHit = false;
            currentKnockbackDuration = defaultKnockbackDuration;
            hasResetKnockbackDuration = true;
        }
    }
    
}
