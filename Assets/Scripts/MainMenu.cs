using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("GP3Coursework");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
