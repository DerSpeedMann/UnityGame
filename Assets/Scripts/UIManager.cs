using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public Canvas[] availableHUDs;
    public Button[] availableTools;

    public LevelManager levelManager;
    public DrawManager drawManager;
    public ToolManager toolManager;

    private Canvas activeHud;
    private Button activeTool;

    void Start()
    {
        foreach (Canvas hud in availableHUDs)
        {
            hud.gameObject.SetActive(false);
        }
        SetActiveHud("EditorHud");
        SetActiveTool("EditorHud");

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
        SetActiveHud("GameHud");
        levelManager.RestartGame();
    }
    public void RespawnPressed()
    {
        SetActiveHud("GameHud");
        levelManager.SpawnPlayer();
    }
    public bool SetActiveTool(string name)
    {
        foreach (Button tool in availableTools)
        {
            if (tool.name == name)
            {
                if (activeTool != null)
                {
                    //TODO: change Button appearance
                }

                activeTool = tool;
                switch (name)
                {
                    case "Draw":
                        toolManager.activeTool = ToolManager.tools.Draw;
                        break;
                    case "Move":
                        toolManager.activeTool = ToolManager.tools.Move;
                        break;
                    case "Erase":
                        toolManager.activeTool = ToolManager.tools.Erase;
                        break;
                }
                
                //TODO: change Button appearance
                return true;
            }
        }
        return false;
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
