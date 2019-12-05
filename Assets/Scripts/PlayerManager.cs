using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("player spawned");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("win");
        }else if (other.CompareTag("Checkpoint"))
        {
            Debug.Log("Checkpoint");
            gameManager.spawnPoint = other.gameObject;
        }
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

}
