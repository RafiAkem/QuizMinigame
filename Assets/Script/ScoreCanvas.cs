using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCanvas : MonoBehaviour
{
    
    //MainMenu
public void LoadMainMenu()
{
    SceneManager.LoadScene("SelectLevel");
}

    //Retry Level
public void ReloadCurrentLevel()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    //Next Level
public void LoadLevel(int levelIndex)
{
    SceneManager.LoadScene(levelIndex);
}
}
