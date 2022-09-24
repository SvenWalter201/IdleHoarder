using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float panSpeed;

    [SerializeField] Vector2 minBound, maxBound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(x < 0 && transform.position.x > minBound.x)
        {
            transform.position += Vector3.right * x * panSpeed * Time.deltaTime;
        }
        if(x > 0 && transform.position.x < maxBound.x)
        {
            transform.position += Vector3.right * x * panSpeed * Time.deltaTime;
            
        }
        if(z < 0 && transform.position.z > minBound.y)
        {
            transform.position += Vector3.forward * z * panSpeed * Time.deltaTime;
            
        }
        if(z > 0 && transform.position.z < maxBound.y)
        {
            transform.position += Vector3.forward * z * panSpeed * Time.deltaTime;
            
        }
    }
}
