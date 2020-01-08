using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIManager uiManager;

    private Vector3 spawnPoint;
    private int checkpoints = 0;

    private bool alive = true;
    private Rigidbody2D rigidBody;

    private bool boostEnabled = false;
    private float speedBoost;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        if (boostEnabled)
        {
            var currentMovementVector = transform.right * speedBoost;
            Debug.Log(currentMovementVector);
            rigidBody.AddForce(currentMovementVector);
        }
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
                GotCkeckpoint(other.gameObject);
            }
            else if (other.CompareTag("Powerup"))
            {
                GotPowerup(other.gameObject);
            }
            else if (other.CompareTag("NoDraw"))
            {

            }
            else
            {
                SetAlive(false);
            }
        }
    }
 
    public void SetLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }
    public void SetUIManager(UIManager manager)
    {
        uiManager = manager;
        Debug.Log(uiManager);
    }

    public void SetSpawn(Vector3 point)
    {
        spawnPoint = point;
    }

    //Spawn and Respawn logic
    public void ResetPlayer()
    {
        checkpoints = 0;
        SetAlive(true);
        boostEnabled = false;
    }
    public void Respawn()
    {
        boostEnabled = false;
        SetAlive(true);
        StopSleth();
        transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }
    public void StopSleth()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
    }
    public void SetAlive(bool alive)
    {
        this.alive = alive;
        if (!alive)
        {
            uiManager.EnableLose(true);
        }
        else
        {
            uiManager.EnableLose(false);
            uiManager.EnableWin(false);
        }
    }

    //Powerups
    private void GotPowerup(GameObject powerupObject)
    {
        powerupObject.SetActive(false);
        var powerup = powerupObject.GetComponent<Powerup>().powerup;

        switch (powerup)
        {
            case Powerup.Powerups.SpeedBoost:
                Debug.Log("SpeedBoost");
                StartCoroutine(BoostSpeed(4, 5));
                break;
            case Powerup.Powerups.Hover:
                Debug.Log("Hover");
                StartCoroutine(Hover(2, 5));
                break;
        }
    }
    IEnumerator BoostSpeed(float speed, float duration)
    {
        Debug.Log("boost started");
        speedBoost = speed*5;
        boostEnabled = true;

        yield return new WaitForSeconds(duration);

        boostEnabled = false;
        Debug.Log("boost ended");
    }
    IEnumerator Hover(float drag, float duration)
    {
        var oldGravity = rigidBody.gravityScale;
        Debug.Log("hover started");
        rigidBody.gravityScale /= drag;
 
        yield return new WaitForSeconds(duration);

        rigidBody.gravityScale = oldGravity;
        Debug.Log("boost ended");
    }

    //Checkpoint and Win Logic
    private void GotCkeckpoint(GameObject checkpoint)
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
            uiManager.EnableWin(true);
            return true;
        }
        return false;
    }
    
}
