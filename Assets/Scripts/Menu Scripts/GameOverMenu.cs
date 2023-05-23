using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public static bool gameOver;
    public PlayerStatistics playerStats;
    public GameObject winScreen;
    public GameObject loseScreen;

    void Update()
    {
        if (playerStats.dead)
        {
            gameOver = true;
        }
    }

    public void DisplayWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void DisplayLoseScreen()
    {
        loseScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        // build index 0 refers to the main menu scene
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
