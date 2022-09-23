using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitelScreenController : MonoBehaviour
{
    [SerializeField]
    string NewGame = "TEST";

    public void StartGame()
    {
        SceneManager.LoadScene(NewGame, LoadSceneMode.Single);
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
