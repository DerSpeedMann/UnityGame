using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool touchControl = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            touchControl = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
