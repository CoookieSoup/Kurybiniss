using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundConsumableScript : MonoBehaviour
{
    PlayerScript playerScript;
    public float floatTimer = 0f;
    public Transform groundConsumableTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        groundConsumableTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        floatTimer += Time.deltaTime;
        if (floatTimer < 1f)
        {
            groundConsumableTransform.position = new Vector2(groundConsumableTransform.position.x, groundConsumableTransform.position.y + 0.45f * Time.deltaTime);
        }
        if (floatTimer >= 1f && floatTimer < 2f)
        {
            groundConsumableTransform.position = new Vector2(groundConsumableTransform.position.x, groundConsumableTransform.position.y + 0.25f * Time.deltaTime);
        }
        if (floatTimer >= 2f && floatTimer < 3f)
        {
            groundConsumableTransform.position = new Vector2(groundConsumableTransform.position.x, groundConsumableTransform.position.y - 0.45f * Time.deltaTime);
        }
        if (floatTimer >= 3f && floatTimer < 4f)
        {
            groundConsumableTransform.position = new Vector2(groundConsumableTransform.position.x, groundConsumableTransform.position.y - 0.25f * Time.deltaTime);
        }
        if (floatTimer >= 4f)
        {
            floatTimer = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && playerScript.currentHealth < playerScript.maxHealth)
        {
            playerScript.currentHealth++;
            Destroy(gameObject);
        }
    }
}
