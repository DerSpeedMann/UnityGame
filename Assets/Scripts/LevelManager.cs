using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public CameraManager camManager;
    public DrawManager drawManager;
    public UIManager uiManager;

    public bool editorMode = true;

    public GameObject[] checkpoints;
    public GameObject[] powerups;

    private GameObject startPoint;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        powerups = GameObject.FindGameObjectsWithTag("Powerup");
        startPoint = GameObject.FindGameObjectsWithTag("Start")[0];
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
        player.GetComponent<PlayerManager>().SetSpawn(startPoint.transform.position);
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
            player = Instantiate(playerPrefab, startPoint.transform.position, Quaternion.identity);

            var playerManager = player.GetComponent<PlayerManager>();
            playerManager.SetLevelManager(this);
            playerManager.SetSpawn(startPoint.transform.position);

            camManager.SetPlayer(player);
        }
    }
}
