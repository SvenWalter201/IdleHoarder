using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitelScreenController : MonoBehaviour
{
    [SerializeField]
    GameObject TitelScreen = default;

    void Awake() 
    {
        if(TitelScreen.activeInHierarchy)
            Time.timeScale = 0.0f;
    }

    public void StartGame()
    {
        TitelScreen.SetActive(false);
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
