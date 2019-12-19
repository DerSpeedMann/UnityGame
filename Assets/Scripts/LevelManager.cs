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

    private GameObject startPoint;
    private GameObject player;
    public GameObject[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
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
