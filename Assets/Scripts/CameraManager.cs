using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float gameSize = 5;
    public float editorSize = 5;

    public float minSize = 10f;
    public float maxSize = 300f;
    public float sensitivity = 5f;

    public float zoomPercentage; //zoom percentage (0-1)

    private GameObject startPoint;
    private GameObject player;

    private Vector3 dragOrigin;
    private Vector3 cameraOrigin;
    private Vector3 camEditorLoacation;

    // Use this for initialization
    void Start()
    {
        startPoint = GameObject.FindGameObjectsWithTag("Start")[0];
        MoveCameraTo(startPoint.transform.position);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {   
        if(player)
        {
            MoveCameraTo(player);
        }
    }

    // changes between game mode (camera following player) and editor mode (free moving camera)
    public void SetGameMode(bool gamemode)
    {
        if (gamemode)
        {
            Camera.main.orthographicSize = gameSize;
        }
        else
        {
            MoveCameraTo(camEditorLoacation);
            Camera.main.orthographicSize = editorSize;
        }
    }

    // set start point for draging
    public void StartDrag(Vector3 origin)
    {
        dragOrigin = origin;
        cameraOrigin = Camera.main.transform.position;
    }

    // drag camera with height and width multiplayer for different screen proportions
    public void Drag(Vector3 actual, float heightMulti = 2, float widthMulti = 0.9f)
    {
        float xDrag, yDrag;

        if (Screen.width < Screen.height)
        {
            xDrag = widthMulti * Camera.main.orthographicSize;
            yDrag = heightMulti * Camera.main.orthographicSize;
        }
        else
        {
            xDrag = heightMulti * Camera.main.orthographicSize;
            yDrag = widthMulti * Camera.main.orthographicSize;
        }


        Vector3 pos = Camera.main.ScreenToViewportPoint(actual - dragOrigin);
        Vector3 move = new Vector3(pos.x * xDrag, pos.y * yDrag, 0);

        MoveCameraTo(cameraOrigin - move);
    }

    // zooms to or from selected center
    public void Zoom(float axis, Vector3 zoomCenter)
    {
        if (axis != 0)
        {
            var camPosition = transform.position;
            var oldCenter = Camera.main.ScreenToWorldPoint(zoomCenter);
            var size = Camera.main.orthographicSize;
    
            //Resize Camera
            size -= axis * sensitivity;
            size = Mathf.Clamp(size, minSize, maxSize);

            Camera.main.orthographicSize = editorSize = size;

            //Move Camera
            var newCenter = Camera.main.ScreenToWorldPoint(zoomCenter);
            var camDiff = (oldCenter - camPosition) - (newCenter - camPosition);
            
            MoveCameraTo(camPosition + camDiff);

            //Calculate Zoom Percentage
            zoomPercentage = (size - minSize) / (maxSize - minSize);
        }
    }

    // moves camera to target position
    public void MoveCameraTo(GameObject target)
    {
        transform.position = new Vector3(
            target.transform.position.x, 
            target.transform.position.y,
            -10
            );
    }

    // moves camera to target position
    public void MoveCameraTo(Vector3 position)
    {
        transform.position = camEditorLoacation = new Vector3(
            position.x,
            position.y,
            -10);
    }

    // set player to follow in game mode
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
