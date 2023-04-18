using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlyingEnemyScript2 : MonoBehaviour
{
    public float EnemyFollowSpeed;
    public Transform player;
    private Vector2 lastSeenPos;
    public Collider2D enemyCollider2D;
    public Collider2D playerCollider2D;
    public PlayerScript playerScript;
    RaycastHit2D LOSCheck;
    public float EnemyDetectRange;
    private bool hasSeen = false;
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyCollider2D = GetComponent<Collider2D>();
        playerCollider2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
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
        LOSCheck = Physics2D.Raycast(transform.position, player.position - transform.position);
        Debug.DrawRay(transform.position, player.position - transform.position);
        if (LOSCheck.collider.gameObject.CompareTag("Player") && Mathf.Abs(player.position.x - transform.position.x) < EnemyDetectRange)
        {
            Vector2 newPos = new Vector2(player.position.x, player.position.y);
            transform.position = Vector3.MoveTowards(transform.position, newPos, EnemyFollowSpeed * Time.deltaTime);
            lastSeenPos = player.position;
            hasSeen = true;
        }
        else {
            if (hasSeen)
            {
                transform.position = Vector3.MoveTowards(transform.position, lastSeenPos, EnemyFollowSpeed * Time.deltaTime);
            }
        }
        if (!playerScript.tookDamage)
        {
            Physics2D.IgnoreCollision(enemyCollider2D, playerCollider2D, false);
        }
    }
    
}
