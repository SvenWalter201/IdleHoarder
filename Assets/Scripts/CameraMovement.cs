using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float panSpeed, scrollSpeed;

    [SerializeField] Vector2 zoomBounds;
    [SerializeField] Vector2 marginMultipliers = new Vector2(1.5f, 0.7f);


    [SerializeField] Vector2 minBound, maxBound;

    void Start()
    {
        
    }

    // Update is called once per frame

    public void CheckCameraBounds()
    {
        float xMargin = transform.position.y * marginMultipliers.x;
        float zMargin = transform.position.y * marginMultipliers.y;

        if(transform.position.x > maxBound.x - xMargin)
        {
            transform.position = new Vector3(maxBound.x - xMargin, transform.position.y, transform.position.z);
        }
        if(transform.position.x < minBound.x + xMargin)
        {
            transform.position = new Vector3(minBound.x + xMargin, transform.position.y, transform.position.z);
        }
        if(transform.position.z > maxBound.y - zMargin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxBound.y - zMargin);
        }
        if(transform.position.z < minBound.y + zMargin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minBound.y + zMargin);
        }
    }
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

        Vector2 scrollDelta = Input.mouseScrollDelta;
        transform.position += Vector3.up * -scrollDelta.y  * scrollSpeed;
        if(transform.position.y > zoomBounds.y)
        {
            transform.position = new Vector3(transform.position.x, zoomBounds.y, transform.position.z);
        }
        else if(transform.position.y < zoomBounds.x)
        {
            transform.position = new Vector3(transform.position.x, zoomBounds.x, transform.position.z);
        }
        /*
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");



        transform.position += Vector3.right * x * panSpeed * Time.deltaTime;
        
        transform.position += Vector3.forward * z * panSpeed * Time.deltaTime;
            
        Vector2 scrollDelta = Input.mouseScrollDelta;
        transform.position += Vector3.up * -scrollDelta.y  * scrollSpeed;
        if(transform.position.y > zoomBounds.y)
        {
            transform.position = new Vector3(transform.position.x, zoomBounds.y, transform.position.z);
        }
        else if(transform.position.y < zoomBounds.x)
        {
            transform.position = new Vector3(transform.position.x, zoomBounds.x, transform.position.z);
        }

        CheckCameraBounds();
        */
    }
}
