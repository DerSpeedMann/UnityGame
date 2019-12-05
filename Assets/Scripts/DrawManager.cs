using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject drawPrefab;
    GameObject trail;
    Plane planObj;
    // Start is called before the first frame update
    void Start()
    {
        planObj = new Plane(Camera.main.transform.forward * -1, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
