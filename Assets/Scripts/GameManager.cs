using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject startPoint;
    public Camera mainCamera;
    public UIManager uiManager;
    public DrawManager drawManager;

    public bool editorMode = true;

    private GameObject player;
    private CameraManager camManager;
    public GameObject[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        camManager = mainCamera.GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (editorMode)
        {
            if (Input.GetMouseButton(0))
            {

                drawManager.StartDrawing();
            }
            if (Input.GetMouseButtonUp(0))
            {
                drawManager.StopDrawing();
            }
        }
    }
    private void LateUpdate()
    {
        if (editorMode)
        {
            if (Input.GetMouseButtonDown(1) || Input.touchCount == 2)
            {
                camManager.StartDrag(Input.mousePosition);
            }
            if (Input.GetMouseButton(1) || Input.touchCount == 2)
            {
                camManager.Drag(Input.mousePosition);
            }

            camManager.Zoom(Input.GetAxis("Mouse ScrollWheel"));            
        }
    }
    public void StartGame()
    {
        editorMode = false;
        drawManager.StopDrawing();

        camManager.SetGameMode(true);
        uiManager.HideEditorUI(true);
        uiManager.HideGameUI(false);

        SpawnPlayer();
    }
    public void StopGame()
    {
        editorMode = true;
        Destroy(player);
        ResetGame();

        camManager.SetGameMode(false);
        uiManager.HideGameUI(true);
        uiManager.HideEditorUI(false);
    }
    public void ResetGame()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.SetActive(true);
        }
        player.GetComponent<PlayerManager>().spawnPoint = startPoint.transform.position;
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
            playerManager.SetGameManager(this);
            playerManager.spawnPoint = startPoint.transform.position;

            camManager.SetPlayer(player);
        }
    }
}
