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
    void Start()
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
            //getransfrom.position = new Vector2(100f, 100f);
            

            //groundEnemyScript.currentHitTime = 100f;
            //groundEnemyRigidBody2D.AddForce(new Vector2(10f, 10f), ForceMode2D.Impulse);
            //if (Input.GetButtonDown("Fire1") && playerScript.flipX)
            //{

            //}
        }
        
    }
}
