using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public LevelManager levelManager;
    public DrawManager drawManager;
    public ToolManager toolManager;

    public string startHUDName;
    public string startToolName;

    public Canvas[] availableHUDs;
    public Button[] availableToolButtons;

    public Color disabledToolColor;
    public Color enabledToolColor;

    private Canvas activeHud;
    private Button activeTool;

    // Disables all HUDs and tool buttons, and reanables selected
    void Start()
    {
        foreach (Canvas hud in availableHUDs)
        {
            hud.gameObject.SetActive(false);
        }
        foreach (Button button in availableToolButtons)
        {
            button.image.color = disabledToolColor;
        }
        SetActiveHud(startHUDName);
        SetActiveTool(startToolName);

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

    // select tool in Toolmanager and changes apearane of buttons (disabled / enabled)
    public void SetActiveTool(string name)
    {
        foreach (Button toolButton in availableToolButtons)
        {
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
                    activeTool.image.color = disabledToolColor;
                }

                // sets and enables new button
                activeTool = toolButton;
                activeTool.image.color = enabledToolColor;
            }
        }
    }

    // unhides selected hud and hides all other
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

    // return to level select screen
    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
