using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTriggerScript : MonoBehaviour
{
    private CheckPointSystem checkPointSystem;

    private void Start()
    {
        checkPointSystem = GameObject.FindGameObjectWithTag("CheckpointSystem").GetComponent<CheckPointSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkPointSystem.lastCheckpointPos = transform.position;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
