using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
    {
        //Overwrite or create new json save file 
    }

    public void ContinueGame()
    {
        //Load data from json save file
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
