using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToCheckpointScript : MonoBehaviour
{
    CheckPointSystem checkPointSystem;
    PlayerScript playerScript;
    private void Start()
    {
        checkPointSystem = GameObject.FindGameObjectWithTag("CheckpointSystem").GetComponent<CheckPointSystem>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("currentHealth", playerScript.currentHealth - 1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
