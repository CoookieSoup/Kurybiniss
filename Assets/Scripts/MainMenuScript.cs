using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public int levelId;
    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
    public void ExitOutOfGame()
    {
        Application.Quit();
    }
    public void RetryLevel()
    {
        //SceneManager.LoadScene(PlayerPrefs.GetInt("levelId", levelId));
        SceneManager.LoadScene(2);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
