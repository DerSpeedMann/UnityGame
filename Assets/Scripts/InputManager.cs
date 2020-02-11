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

    public bool moveEnabled;

    public float pinchMultiMin; //min drag increase while pinching (zoomed all the way in)
    public float pinchMultiMax; //max drag increase while pinching (zoomed all the way out)
    public float dragDelay; // time before drag can be used after zooming (in seconds)

    private float startPinchDist;
    private float timeTillLastZoom = 0;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.platform);
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            touchControl = true;
        }

        Debug.Log("touch: " + touchControl);
    }

    private void Update()
    {
        if (levelManager.editorMode)
        {
            if (touchControl)
            {
                if (Input.touchCount == 1 && !moveEnabled)
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
                timeTillLastZoom += Time.deltaTime;

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
                        
                        // calculate center of touch
                        var touch1 = finger1.position;
                        var touch2 = finger2.position;
                        Vector3 center = Vector3.Lerp(touch1, touch2, 0.5f);
                        
                        // pinch zoom logic
                        var pinchMulti = camManager.zoomPercentage * (pinchMultiMax - pinchMultiMin) + pinchMultiMin;

                        Zoom(pinchChange * pinchMulti, center);
                        startPinchDist = actualPinchDist;

                        timeTillLastZoom = 0;
                    }
                }
                else if (Input.touchCount == 1 && moveEnabled && timeTillLastZoom > dragDelay)
                {
                    var touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        StartDrag(touch.position);
                    }

                    Drag(touch.position);
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
