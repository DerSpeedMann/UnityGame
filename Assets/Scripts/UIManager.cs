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

    void Start()
    {
        editorHud.SetActive(true);
        gameHud.SetActive(false);
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
}
