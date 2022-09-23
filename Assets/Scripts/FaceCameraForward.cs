using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraForward : MonoBehaviour
{
Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        transform.rotation = Quaternion.LookRotation(cameraTransform.forward);
    }
}
