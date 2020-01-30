using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public LevelManager levelManager;

    private Vector3 spawnPoint;

    private bool alive = true;
    private bool won = false;
    private Rigidbody2D rigidBody;

    private bool boostEnabled = false;
    private float speedBoost;
    private float defaultGravityScale;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        defaultGravityScale = rigidBody.gravityScale;
    }
    private void FixedUpdate()
    {
        if (boostEnabled)
        {
            var currentMovementVector = transform.right * speedBoost;
            rigidBody.AddForce(currentMovementVector);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alive && !won)
        {
            if (other.CompareTag("Finish"))
            {
                won = levelManager.WinCheck();
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

    public void SetSpawn(Vector3 point)
    {
        spawnPoint = point;
    }

    //Spawn and Respawn logic
    public void ResetPlayer()
    {
        SetAlive(true);
        won = false;

        boostEnabled = false;
        rigidBody.gravityScale = defaultGravityScale;

        StopSleth();
    }
    public void Respawn()
    {
        ResetPlayer();
        transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }
    public void StopSleth()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
    }
    public void SetAlive(bool alive)
    {
        if (this.alive != alive && alive == false)
        {
            levelManager.Lost();
        }
    }

    //Powerups
    private void GotPowerup(GameObject powerupObject)
    {
        powerupObject.SetActive(false);
        var powerupManager = powerupObject.GetComponent<Powerup>();

        switch (powerupManager.powerup)
        {
            case Powerup.Powerups.SpeedBoost:
                StartCoroutine(BoostSpeed(powerupManager.speedBoost, powerupManager.duration));
                break;
            case Powerup.Powerups.Hover:
                StartCoroutine(Hover(powerupManager.hoverValue, powerupManager.duration));
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
        if(rigidBody.gravityScale == defaultGravityScale)
        {
            defaultGravityScale = rigidBody.gravityScale;
            Debug.Log("hover started");
            rigidBody.gravityScale *= -drag;
        }
        else
        {
            Debug.Log("hover started");
            rigidBody.gravityScale = defaultGravityScale * -drag;
        }
 
        yield return new WaitForSeconds(duration);

        rigidBody.gravityScale = defaultGravityScale;
        Debug.Log("hover ended");
    }

    //Checkpoint
    private void GotCkeckpoint(GameObject checkpoint)
    {
        if(spawnPoint != checkpoint.transform.position)
        {
            levelManager.GotCheckpoint();
            spawnPoint = checkpoint.transform.position;
            checkpoint.SetActive(false);
        }
    }

    
}
