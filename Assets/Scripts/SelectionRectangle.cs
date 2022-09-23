using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionRectangle : MonoBehaviour
{
    MeshRenderer mR;

    void Awake() 
    {
        mR = GetComponent<MeshRenderer>();
    }

    public void UpdateRectangle(Vector3 beginPoint, Vector3 endPoint)
    {
        Vector3 min = new Vector3(
                    beginPoint.x < endPoint.x ? beginPoint.x : endPoint.x,
                    0.0f,
                    beginPoint.z < endPoint.z ? beginPoint.z : endPoint.z);
        Vector3 max = new Vector3(
            beginPoint.x > endPoint.x ? beginPoint.x : endPoint.x,
            0.0f,
            beginPoint.z > endPoint.z ? beginPoint.z : endPoint.z);

        Vector3 diff = max - min;
        Vector3 center = max - diff * 0.5f;
        transform.position = center + Vector3.up * 0.1f;
        Vector3 scale = new Vector3(Mathf.Abs(diff.x), 1.0f, Mathf.Abs(diff.z));
        scale /= 10.0f;
        mR.material.SetFloat("_XScale", scale.x);
        mR.material.SetFloat("_YScale", scale.z);
        transform.localScale = scale;
    }

}
