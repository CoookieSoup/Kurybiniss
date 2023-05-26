using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckRightScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public FlyingEnemyScript2 flyingEnemyScript;
    public GroundEnemyScript groundEnemyScript;
    public Rigidbody2D groundEnemyRigidBody2D;
    public Transform playerTransform;
    public Transform getransfrom;
    /*void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        playerTransform = GetComponentInParent<Transform>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && Input.GetButtonDown("Fire1"))
        {
            flyingEnemyScript = collision.GetComponent<FlyingEnemyScript2>();
            
            if (Input.GetButtonDown("Fire1") && playerScript.flipX)
            {
                if (flyingEnemyScript.currentKnockbackDuration <= 0f)
                {
                    flyingEnemyScript.currentKnockbackDuration = flyingEnemyScript.defaultKnockbackDuration;
                    flyingEnemyScript.hasBeenHit = true;
                    flyingEnemyScript.knockbackOrigin = playerTransform.position;
                    flyingEnemyScript.currentHealth--;
                }

            }
        }
        
    }*/
}
