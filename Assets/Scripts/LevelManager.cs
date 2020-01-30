using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public CameraManager camManager;
    public DrawManager drawManager;
    public UIManager uiManager;

    public GameObject[] checkpoints;
    public GameObject[] powerups;

    public Vector3 spawnOffset = new Vector3(0, 1.5f, 0);

    public bool editorMode = true;

    public int levelNumber = 0;

    private Vector3 startPoint;
    private GameObject player;

    private int checkpointCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        powerups = GameObject.FindGameObjectsWithTag("Powerup");
        startPoint = GameObject.FindGameObjectsWithTag("Start")[0].transform.position + spawnOffset;
    }

    public void StartGame()
    {
        editorMode = false;
        drawManager.StopDrawing();

        camManager.SetGameMode(true);

        SpawnPlayer();
    }
    public void StopGame()
    {
        editorMode = true;
        Destroy(player);
        ResetGame();

        camManager.SetGameMode(false);
    }
    public void ResetGame()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.SetActive(true);
        }
        foreach (var powerup in powerups)
        {
            powerup.SetActive(true);
        }
        checkpointCounter = 0;

        player.GetComponent<PlayerManager>().SetSpawn(startPoint);
        player.GetComponent<PlayerManager>().ResetPlayer();
    }
    public void RestartGame()
    {
        ResetGame();
        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        if (player)
        {
            player.GetComponent<PlayerManager>().Respawn();
        }
        else
        {
            player = Instantiate(playerPrefab, startPoint, Quaternion.identity);

            var playerManager = player.GetComponent<PlayerManager>();
            playerManager.SetLevelManager(this);
            playerManager.SetSpawn(startPoint);

            camManager.SetPlayer(player);
        }
    }

    //Checkpoint / win- lose logic
    public void GotCheckpoint()
    {
        checkpointCounter += 1;
        Debug.Log("checkpoints reached:" + checkpoints);
    }
    public bool WinCheck()
    {
        Debug.Log("max:" + checkpoints.Length + " got:" + checkpointCounter);

        if (checkpointCounter >= checkpoints.Length)
        {
            uiManager.SetActiveHud("WinHud");
            LevelSelect.UnlockLevel(levelNumber);

            return true;
        }
        return false;
    }
    public void Lost()
    {
        uiManager.SetActiveHud("LoseHud");
    }
}
