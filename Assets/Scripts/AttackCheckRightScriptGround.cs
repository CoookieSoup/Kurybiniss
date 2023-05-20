using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckRightScriptGround : MonoBehaviour
{
    public PlayerScript playerScript;
    public GroundEnemyScript groundEnemyScript;
    public Rigidbody2D groundEnemyRigidBody2D;
    public Transform playerTransform;
    public Transform getransfrom;
    void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        playerTransform = GetComponentInParent<Transform>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        getransfrom = collision.GetComponent<Transform>();
        groundEnemyScript = collision.GetComponent<GroundEnemyScript>();
        if (collision.CompareTag("Enemy") && Input.GetButtonDown("Fire1") && !playerScript.flipX && groundEnemyScript.canMove)
        {
            groundEnemyScript.currentHealth--;
            groundEnemyRigidBody2D = collision.GetComponent<Rigidbody2D>();
            getransfrom = collision.GetComponent<Transform>();
            groundEnemyScript.hasBeenHit = true;
            if (playerScript.flipX)
            {
                groundEnemyScript.knockBackDirection = -1;
            }
            if (!playerScript.flipX)
            {
                groundEnemyScript.knockBackDirection = 1;
            }
        }
    }
}
