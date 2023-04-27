using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheckLeftScript : MonoBehaviour
{
    PlayerScript playerScript;
    // Start is called before the first frame update
    
    void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                collision.transform.SetPositionAndRotation(new Vector3( 1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 0f)); 
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
