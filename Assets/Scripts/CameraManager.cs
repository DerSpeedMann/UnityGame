using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public GameObject SpawnPoint;
    private float CameraZ;         

    // Use this for initialization
    void Start()
    { 
        CameraZ = transform.position.z;

        UpdatePosition(SpawnPoint);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {   
        if(player)
        {
            UpdatePosition(player);
        }
    }

    private void UpdatePosition(GameObject target)
    {
        transform.position = new Vector3(
            target.transform.position.x, 
            target.transform.position.y, 
            CameraZ);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
