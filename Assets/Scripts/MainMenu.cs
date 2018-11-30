using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GP3Coursework");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void HighScores()
    {
        SceneManager.LoadScene("HighScoreTable");
    }
}
