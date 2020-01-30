using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public Canvas[] availableHUDs;

    public LevelManager levelManager;
    public DrawManager drawManager;

    private Canvas activeHud;

    void Start()
    {
        foreach (Canvas hud in availableHUDs)
        {
            hud.gameObject.SetActive(false);
        }
        SetActiveHud("EditorHud");

    }

    //Editor HUD
    public void StartPressed()
    {
        SetActiveHud("GameHud");

        levelManager.StartGame();
    }
    public void UndoPressed()
    {
        drawManager.RemoveLast();
    }
    public void ResetPressed()
    {
        drawManager.RemoveAll();
    }

    //Game HUD
    public void StopPressed()
    {
        SetActiveHud("EditorHud");

        levelManager.StopGame();
    }
    public void RestartPressed()
    {
        levelManager.RestartGame();
    }
    public void RespawnPressed()
    {
        levelManager.SpawnPlayer();
    }

    public bool SetActiveHud(string name)
    {
        foreach (Canvas hud in availableHUDs)
        {
           if(hud.name == name)
           {
                if(activeHud != null)
                {
                    activeHud.gameObject.SetActive(false);
                }

                activeHud = hud;
                activeHud.gameObject.SetActive(true);
                return true;
            }
        }
        return false;
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
