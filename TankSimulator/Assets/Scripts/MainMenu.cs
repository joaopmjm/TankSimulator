using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenu : MonoBehaviour
{
    public void Tutorial()
    {
        Debug.Log("Button Tutorial");
    }
    public void Begin()
    {
        Debug.Log("Button Begin Game");
    }
    public void Teste()
    {
        SceneManager.LoadScene("Teste");
    }
    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Debug.Log("Quit");
            Application.Quit();
        #endif
    }
}
