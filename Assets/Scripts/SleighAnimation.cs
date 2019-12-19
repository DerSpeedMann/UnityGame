using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleighAnimation : MonoBehaviour
{
    float speed = 8f;
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * speed), transform.position.z);

        if (transform.position.y > 0.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.y < -0.1f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
