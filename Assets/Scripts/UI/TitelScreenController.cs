using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitelScreenController : MonoBehaviour
{
    [SerializeField]
    GameObject TitelScreen = default;

    public void StartGame()
    {
        TitelScreen.SetActive(false);
        
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
