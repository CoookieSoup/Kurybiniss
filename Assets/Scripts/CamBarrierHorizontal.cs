using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBarrierHorizontal : MonoBehaviour
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
            cameraScript.stopHorizontal = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraScript.stopHorizontal = false;
        }
    }
}
