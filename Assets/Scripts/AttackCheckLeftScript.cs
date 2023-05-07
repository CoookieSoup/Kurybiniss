using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckLeftScript : MonoBehaviour
{
    PlayerScript playerScript;
    FlyingEnemyScript2 flyingEnemyScript;
    Transform playerTransform;
    void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        playerTransform = GetComponentInParent<Transform>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            flyingEnemyScript = collision.GetComponent<FlyingEnemyScript2>();
            if (Input.GetButtonDown("Fire1") && playerScript.flipX && flyingEnemyScript.currentKnockbackDuration <= 0f)
            {
                flyingEnemyScript.currentKnockbackDuration = flyingEnemyScript.defaultKnockbackDuration;
                flyingEnemyScript.hasBeenHit = true;
                flyingEnemyScript.knockbackOrigin = playerTransform.position;
                flyingEnemyScript.currentHealth--;
            }
        }
    }
}
