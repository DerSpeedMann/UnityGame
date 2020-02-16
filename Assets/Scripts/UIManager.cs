using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public Canvas[] availableHUDs;
    public Button[] availableToolButtons;

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
        SetActiveTool("Draw");

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
    public void SetActiveTool(string name)
    {
        Debug.Log("length: " + availableToolButtons.Length);

        foreach (Button toolButton in availableToolButtons)
        {
            Debug.Log("tools: " + toolButton.name);
            if (toolButton.name == name)
            {
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
                    default:
                        return;
                }

                // disables old active
                if (activeTool != null)
                {
                    activeTool.image.color = Color.gray;
                }

                // sets and enables new button
                activeTool = toolButton;
                activeTool.image.color = Color.white;
            }
        }
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
