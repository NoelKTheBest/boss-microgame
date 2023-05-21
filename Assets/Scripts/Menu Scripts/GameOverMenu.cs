using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void DisplayWinScreen()
    {
        // Display screen for winning
    }

    public void DisplayLoseScreen()
    {
        // Display screen for game over
    }

    public void RestartGame()
    {
        // Reload Scene
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
