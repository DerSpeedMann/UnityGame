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
        UpdateEditorPosition(startPoint.transform.position);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {   
        if(player)
        {
            UpdateGamePosition(player);
        }
    }
    public void SetGameMode(bool gamemode)
    {
        if (gamemode)
        {
            Camera.main.orthographicSize = gameSize;
        }
        else
        {
            UpdateEditorPosition(camEditorLoacation);
            Camera.main.orthographicSize = editorSize;
        }
    }
    public void StartDrag(Vector3 origin)
    {
        dragOrigin = origin;
        cameraOrigin = Camera.main.transform.position;
    }
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

        UpdateEditorPosition(cameraOrigin - move);
    }
    public void Zoom(float axis)
    {
        if (axis != 0)
        {
            var size = Camera.main.orthographicSize;
            size -= axis * sensitivity;
            size = Mathf.Clamp(size, minSize, maxSize);

            Camera.main.orthographicSize = editorSize = size;
            zoomPercentage = (size - minSize) / (maxSize - minSize);
        }
    }
    public void UpdateGamePosition(GameObject target)
    {
        transform.position = new Vector3(
            target.transform.position.x, 
            target.transform.position.y,
            -10
            );
    }
    public void UpdateEditorPosition(Vector3 position)
    {
        transform.position = camEditorLoacation = new Vector3(
            position.x,
            position.y,
            -10);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
