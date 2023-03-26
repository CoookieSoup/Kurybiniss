using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundEnemyScript : MonoBehaviour
{
    RaycastHit2D hit;
    private Transform playerPos;
    private Rigidbody2D GroundEnemyRb;
    [SerializeField] private float GroundEnemySpeedStrength = 10f;
    [SerializeField] private float EnemyDetectRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GroundEnemyRb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, playerPos.position - transform.position);
        if (hit.collider.gameObject.CompareTag("Player") && Mathf.Abs(playerPos.position.x - transform.position.x) < EnemyDetectRange) //31
        {
            if (playerPos.position.x - transform.position.x < 0f)
            {
                GroundEnemyRb.velocity = new Vector2(-GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
            if (playerPos.position.x - transform.position.x > 0f)
            {
                GroundEnemyRb.velocity = new Vector2(GroundEnemySpeedStrength, GroundEnemyRb.velocity.y);
            }
        }
        if (hit.collider.gameObject.CompareTag("Player") == false || Mathf.Abs(playerPos.position.x - transform.position.x) >= EnemyDetectRange)
        {
            GroundEnemyRb.velocity = new Vector2(0f, 0f);
        }
    }
    
}
