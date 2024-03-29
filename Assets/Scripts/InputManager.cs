﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public DrawManager drawManager;
    public CameraManager camManager;
    public LevelManager levelManager;
    public ToolManager toolManager;

    public bool touchControl = true;



    public float pinchMultiMin; //min zoom increase while pinching (zoomed all the way in)
    public float pinchMultiMax; //max zoom increase while pinching (zoomed all the way out)
    public float dragDelay; //time before drag can be used after zooming (in seconds)

    private float startPinchDist;
    private float noDragTimer = 0;
    private bool drawEnabled;

    // Start is called before the first frame update
    void Start()
    {

        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            touchControl = true;
        }
        drawEnabled = toolManager.activeTool == ToolManager.tools.Draw;
    }

    private void Update()
    {
        if (levelManager.editorMode)
        {
            if (touchControl)
            {
                // Touch Drawing
                if (Input.touchCount == 1 && toolManager.activeTool == ToolManager.tools.Draw)
                {
                    drawEnabled = true;

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
            // Mouse Control
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
                noDragTimer += Time.deltaTime;

                if (Input.touchCount == 2)
                {
                    var finger1 = Input.GetTouch(0);
                    var finger2 = Input.GetTouch(1);

                    // Touch start Zoom / Drag
                    if (finger2.phase == TouchPhase.Began)
                    {
                        StartDrag(finger2.position);
                        startPinchDist = Vector2.Distance(finger1.position, finger2.position);
                    }

                    // Touch Zoom
                    if (finger1.phase == TouchPhase.Moved || finger2.phase == TouchPhase.Moved)
                    {
                        var actualPinchDist = Vector2.Distance(finger1.position, finger2.position);
                        var pinchChange = actualPinchDist - startPinchDist;
                        
                        // calculate center of touch
                        var touch1 = finger1.position;
                        var touch2 = finger2.position;
                        Vector3 center = Vector3.Lerp(touch1, touch2, 0.5f);
                        
                        // pinch zoom logic
                        var pinchMulti = camManager.zoomPercentage * (pinchMultiMax - pinchMultiMin) + pinchMultiMin;

                        Zoom(pinchChange * pinchMulti, center);
                        startPinchDist = actualPinchDist;

                        noDragTimer = 0;
                    }
                }
                // Touch Drag
                else if (Input.touchCount == 1 && toolManager.activeTool == ToolManager.tools.Move && noDragTimer > dragDelay)
                {
                    // delays move after changing to avoid instant move
                    if (drawEnabled)
                    {
                        drawEnabled = false;
                        noDragTimer = 0;
                        return;
                    }
                    var touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        StartDrag(touch.position);
                    }

                    Drag(touch.position);
                }
            }
            // Mouse Control
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
                var center = Input.mousePosition;
                Zoom(Input.GetAxis("Mouse ScrollWheel"), center);
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
    public void Zoom(float zoomValue, Vector3 center)
    {
        camManager.Zoom(zoomValue, center);
    }
}
