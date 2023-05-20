using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBarrierVerticalScript : MonoBehaviour
{
    FollowPlayer cameraScript;
    void Start()
    {
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraScript.stopVertical = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraScript.stopVertical = false;
        }
    }
}
