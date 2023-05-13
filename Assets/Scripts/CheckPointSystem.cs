using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    private static CheckPointSystem instance;
    public Vector2 lastCheckpointPos;
    PlayerScript playerScript;
    public float currentHealth;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
        if (PlayerPrefs.GetFloat("currentHealth") == 0f)
        {
            PlayerPrefs.SetFloat("currentHealth", 4f);
            lastCheckpointPos = new Vector2(0f, 0f);
        }
    }
    
    public void SetFloat(string currentHealth, float health)
    {
        if (PlayerPrefs.GetFloat("currentHealth") == 1f)
        {
            PlayerPrefs.SetFloat("currentHealth", 4f);
        }
    }

    public float GetFloat(string currentHealth)
    {
        return PlayerPrefs.GetFloat(currentHealth);
    }
    
}
