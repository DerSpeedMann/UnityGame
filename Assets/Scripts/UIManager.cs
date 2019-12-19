using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public GameObject editorHud;
    public GameObject gameHud;
    public LevelManager levelManager;
    public DrawManager drawManager;

    private Image loseImg;
    private Image winImg;

    void Start()
    {
        editorHud.SetActive(true);
        gameHud.SetActive(false);

        loseImg = gameHud.transform.Find("loseImg").GetComponent<Image>();
        winImg = gameHud.transform.Find("winImg").GetComponent<Image>();

        EnableLose(false);
        EnableWin(false);

        //TODO: on win EnableWin(true)
        // on lose EnableLose(true)
    }

    //Editor HUD
    public void StartPressed()
    {
        editorHud.SetActive(false);
        gameHud.SetActive(true);

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
        editorHud.SetActive(true);
        gameHud.SetActive(false);

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

    public void EnableWin(bool enable)
    {
        if(enable)
        {
            winImg.enabled = true;
        } else
        {
            winImg.enabled = false;
        }
    }

    public void EnableLose(bool enable)
    {
        if (enable)
        {
            loseImg.enabled = true;
        }
        else
        {
            loseImg.enabled = false;
        }
    }
}
