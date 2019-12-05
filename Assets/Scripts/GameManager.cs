using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject spawnPoint;
    public Camera mainCamera;
    public UIManager UIManager;

    private GameObject player;
    private CameraManager camManager;

    // Start is called before the first frame update
    void Start()
    {
        camManager = (CameraManager)mainCamera.GetComponent(typeof(CameraManager));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Spawn()
    {
        if (player)
        {
            var body = (Rigidbody2D)player.GetComponent(typeof(Rigidbody2D));
            body.velocity = Vector2.zero;
            player.transform.SetPositionAndRotation(spawnPoint.transform.position, Quaternion.identity);
        }
        else
        {
            player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);

            var playerManager = (PlayerManager) player.GetComponent(typeof(PlayerManager));
            playerManager.SetGameManager(this);

            camManager.SetPlayer(player);
        }

    }
}
