using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;

    public Button leftItemButton;
    public Button currentItemButton;
    public Button rightItemButton;

    public Image leftItemImage;
    public Image currentItemImage;
    public Image rightItemImage;

    public TMP_Text leftItemText;
    public TMP_Text currentItemText;
    public TMP_Text rightItemText;

    public TMP_Text noItemsText;
    
    /*
    void Awake()
    {
        //leftItemImage = leftItemButton.GetComponentInChildren<Image>();
        //leftItemText = leftItemButton.GetComponentInChildren<TMP_Text>();

        //currentItemImage = currentItemButton.GetComponentInChildren<Image>();
        //currentItemText = currentItemButton.GetComponentInChildren<TMP_Text>();

        //rightItemImage = rightItemButton.GetComponentInChildren<Image>();
        //rightItemText = rightItemButton.GetComponentInChildren<TMP_Text>();
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!SceneData.playerHasHealingItems)
        {
            currentItemButton.gameObject.SetActive(false);
            noItemsText.gameObject.SetActive(true);
        }
        else
        {
            currentItemButton.gameObject.SetActive(true);
            noItemsText.gameObject.SetActive(false);
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isGamePaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene("Main Menu");
    }

    public void MoveCurrentItemLeft()
    {
        /**/
        if (leftItemButton.gameObject.activeSelf) leftItemButton.gameObject.SetActive(true);
        leftItemImage.sprite = currentItemImage.sprite;
        leftItemText.text = currentItemText.text;

        currentItemImage.sprite = rightItemImage.sprite;
        currentItemImage.SetNativeSize();
        currentItemText.text = rightItemText.text;
        rightItemButton.gameObject.SetActive(false);
        /**/

        //leftItemButton.transform.position = 
        //currentItemButton.transform.position = leftItemButton.transform.position;
        //currentItemButton.transform.localScale = leftItemButton.transform.localScale;
        //right
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
