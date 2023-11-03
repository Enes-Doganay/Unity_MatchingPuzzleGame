using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int currentLevel;
    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("Level", 1);
    }
    
    public void NextLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("Level", currentLevel);
        ReloadScene();
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
