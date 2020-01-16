using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
   public void GoToLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GoToLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void GoToLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
