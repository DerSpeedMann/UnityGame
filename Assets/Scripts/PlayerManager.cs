using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public LevelManager levelManager;

    private Vector3 spawnPoint;
    private int checkpoints = 0;
    private bool alive = true;
    private Rigidbody2D rigidBody;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

    }
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
            else if (other.CompareTag("NoDraw"))
            {

            }else if (other.CompareTag("Powerup"))
            {

            }
            else
            {
                Debug.Log("ded");
                alive = false;
            }
        }
    }
    public void SetLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }
    public void ResetPlayer()
    {
        checkpoints = 0;
    }

    public void Respawn()
    {
        alive = true;
        StopSleth();
        transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }
    public void StopSleth()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
    }
    public void SetSpawn(Vector3 point)
    {
        spawnPoint = point;
    }

    private void SetCkeckpoint(GameObject checkpoint)
    {
        if(spawnPoint != checkpoint.transform.position)
        {
            checkpoints++;
            spawnPoint = checkpoint.transform.position;
            checkpoint.SetActive(false);

            Debug.Log("got:" + checkpoints);
        }
    }
    private bool WinCheck()
    {
        //Debug.Log("max:"+ levelManager.checkpoints.Length+" got:"+checkpoints);

        if (checkpoints >= levelManager.checkpoints.Length)
        {
            Debug.Log("Win");
            return true;
        }
        return false;
    }
    
}
