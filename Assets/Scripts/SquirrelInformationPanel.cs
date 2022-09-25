using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SquirrelInformationPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt, statusTxt;

    public void UpdateUI(string name, int status, Transform follow = null)
    {
        nameTxt.text = name;
        statusTxt.text = "Status: " + ((status == 0) ? "Idle" : ((status == 1) ? "Working" : "Tired"));

        f = follow;
    }

    Transform f;

    void Update() 
    {
        if(f != null)
        {
            transform.position = f.position;
        }
    }
}
