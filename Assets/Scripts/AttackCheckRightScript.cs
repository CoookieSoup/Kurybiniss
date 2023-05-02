using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckRightScript : MonoBehaviour
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
            if (Input.GetButtonDown("Fire1") && !playerScript.flipX)
            {
                flyingEnemyScript = collision.GetComponent<FlyingEnemyScript2>();
                flyingEnemyScript.hasBeenHit = true;
                flyingEnemyScript.knockbackOrigin = playerTransform.position;
                flyingEnemyScript.currentHealth--;
            }
        }
    }
}
