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
        Application.Quit();
    }
}
