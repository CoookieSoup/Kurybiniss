using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundEnemyScript : MonoBehaviour
{
    //RaycastHit2D hit;
    RaycastHit2D hitUpper;
    private Transform playerPos;
    private Rigidbody2D GroundEnemyRb;
    private Transform GroundEnemyLOSCheck;
    [SerializeField] private float GroundEnemySpeedStrength = 10f;
    [SerializeField] private float EnemyDetectRange;
    public float jumpStrength = 10f;
    public bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    public Transform groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GroundEnemyRb = GetComponent<Rigidbody2D>();
        GroundEnemyLOSCheck = GetComponent<Transform>();
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
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(2.2f, 0.2f), 0, groundLayer);
        hitUpper = Physics2D.Raycast(new Vector3 (GroundEnemyLOSCheck.position.x, GroundEnemyLOSCheck.position.y + 2f), playerPos.position - GroundEnemyLOSCheck.position);
        Debug.DrawRay(new Vector3(GroundEnemyLOSCheck.position.x, GroundEnemyLOSCheck.position.y + 2f), playerPos.position - GroundEnemyLOSCheck.position, Color.green);
        if (hitUpper.collider.gameObject.CompareTag("Player") && Mathf.Abs(playerPos.position.x - transform.position.x) < EnemyDetectRange) //31
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
        
        if (hitUpper.collider.gameObject.CompareTag("Player") == false)
        {
            GroundEnemyRb.velocity = new Vector2(0f, GroundEnemyRb.velocity.y);
        }
        if (Mathf.Abs(playerPos.position.x - transform.position.x) >= EnemyDetectRange)
        {
            GroundEnemyRb.velocity = new Vector2(0f, GroundEnemyRb.velocity.y);
        }
    }
    
}
