using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public DrawManager drawManager;
    public CameraManager camManager;
    public LevelManager levelManager;

    public bool touchControl = true;

    public float pinchDelta = 5;
    public float pincMulti = 0.025f;
    public float dragDelta = 4;

    private float startPinchDist;


    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            touchControl = true;
        }
    }

    private void FixedUpdate()
    {
        if (levelManager.editorMode)
        {
            if (touchControl)
            {
                if (Input.touchCount == 1)
                {
                    var touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)
                    {
                        Draw(touch.position);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        StopDrawing();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    Draw(Input.mousePosition);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    StopDrawing();
                }
            }
        }
    }
    private void LateUpdate()
    {
        if (levelManager.editorMode)
        {
            if (touchControl)
            {
                if (Input.touchCount == 2)
                {
                    var finger1 = Input.GetTouch(0);
                    var finger2 = Input.GetTouch(1);

                    if (finger2.phase == TouchPhase.Began)
                    {
                        StartDrag(finger2.position);
                        startPinchDist = Vector2.Distance(finger1.position, finger2.position);
                    }

                    if (finger1.phase == TouchPhase.Moved || finger2.phase == TouchPhase.Moved)
                    {
                        var actualPinchDist = Vector2.Distance(finger1.position, finger2.position);
                        var pinchChange = actualPinchDist - startPinchDist;
                        
                        if (Mathf.Abs(pinchChange) > pinchDelta)
                        {
                            Zoom(pinchChange * pincMulti);
                            startPinchDist = actualPinchDist;
                        }
                        else if(Mathf.Abs(pinchChange) < dragDelta)
                        {
                            Drag(finger2.position);
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    StartDrag(Input.mousePosition);
                }
                if (Input.GetMouseButton(1))
                {
                    Drag(Input.mousePosition);
                }

                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
        }
    }

    //Drawing
    public void Draw(Vector3 point)
    {
        drawManager.Draw(point);
    }
    public void StopDrawing()
    {
        drawManager.StopDrawing();
    }

    //Camera Movement
    public void StartDrag(Vector3 startPoint)
    {
        camManager.StartDrag(startPoint);
    }
    public void Drag(Vector3 nextPoint)
    {
        camManager.Drag(nextPoint);
    }
    public void Zoom(float zoomValue)
    {
        camManager.Zoom(zoomValue);
    }
}
