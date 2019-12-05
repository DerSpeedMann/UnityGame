using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public Canvas MainHud;
    public string StartButtonText = "Start";
    public string RestartButtonText = "Restart";

    private bool gameStarted = false;
    private Text spawnButtonText;

    void Start()
    {
        spawnButtonText = GameObject.Find("Start").GetComponentInChildren<Text>();
        spawnButtonText.text = StartButtonText;
    }
    public void StartPressed()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            spawnButtonText.text = RestartButtonText;
        }
    }
}
