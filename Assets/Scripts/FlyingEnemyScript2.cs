using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlyingEnemyScript2 : MonoBehaviour
{
    public float EnemyFollowSpeed = 0.5f;
    public Transform player;
    public SpriteRenderer playerSprite;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D (Collision2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (playerSprite.flipX && transform.position.x > player.position.x || !playerSprite.flipX && transform.position.x < player.position.x)
        {
            Vector3 newPos = new Vector3(player.position.x, player.position.y, 0f);
            transform.position = Vector3.Slerp(transform.position, newPos, EnemyFollowSpeed * Time.deltaTime);
        }
        
    }
    
}
