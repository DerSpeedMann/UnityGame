using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;
    public Vector3 spawnPoint;

    private int checkpoints = 0;
    private bool alive = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
        {
            if (other.CompareTag("Finish"))
            {
                WinCheck();
            }
            else if (other.CompareTag("Checkpoint"))
            {
                SetCkeckpoint(other.gameObject);
            }
            else
            {
                Debug.Log("ded");
                alive = false;
            }
        }
    }

    public void ResetPlayer()
    {
        checkpoints = 0;
    }

    public void Respawn()
    {
        alive = true;
        var body = GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;

        transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }
    private void SetCkeckpoint(GameObject checkpoint)
    {
        checkpoints++;
        spawnPoint = checkpoint.transform.position;
        checkpoint.SetActive(false);
        Debug.Log(checkpoints);

    }
    private bool WinCheck()
    {
        if(checkpoints >= gameManager.checkpoints.Length)
        {
            Debug.Log("Win");
            return true;
        }
        return false;
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

}
