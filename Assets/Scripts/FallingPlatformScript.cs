using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }
    
    private bool startCoundown;
    private float timer;
    private Rigidbody2D fallingPlatformRb;
    // Update is called once per frame
    void OnCollisionEnter2D()
    {
        startCoundown = true;
        fallingPlatformRb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (startCoundown)
        {
            timer += Time.deltaTime;
        }
        if (timer > 0.2)
        {
            fallingPlatformRb.constraints = ~RigidbodyConstraints2D.FreezePositionY;
            fallingPlatformRb.AddForce(-transform.up * 20f);
        }
        if (timer > 6)
        {
            Destroy(transform.gameObject);
        }
    }
}
