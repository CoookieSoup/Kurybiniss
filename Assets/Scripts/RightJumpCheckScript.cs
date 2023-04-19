using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightJumpCheckScript : MonoBehaviour
{
    Rigidbody2D GroundEnemyRb;
    GroundEnemyScript groundEnemyScript;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && groundEnemyScript.isGrounded && GroundEnemyRb.velocity.x >= 0f)
        {
            GroundEnemyRb.velocity = new Vector2(GroundEnemyRb.velocity.x, groundEnemyScript.jumpStrength);
        }
    }
    void Start()
    {
        groundEnemyScript = GetComponentInParent<GroundEnemyScript>();
        GroundEnemyRb = GetComponentInParent<Rigidbody2D>();
    }
}
