using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog("Quit", "Are you sure you want to quit?", "Yes", "No");
#else
        Application.Quit();
#endif
    }
}
