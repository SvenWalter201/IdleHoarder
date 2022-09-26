using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject PauseMenu = default;
    
    [SerializeField]
    GameObject TitelScreen = default;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }


    public void ReturnToGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void EndGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
