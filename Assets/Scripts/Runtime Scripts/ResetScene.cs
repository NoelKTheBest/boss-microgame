using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetScene : MonoBehaviour
{
    public Stats player;
    public Stats[] enemies;
    public bool enemiesDead;
    public GameObject UI;
    // Start is called before the first frame update
    void Awake()
    {
        enemiesDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i < enemies.Length; i++)
        {
            Debug.Log(i + ": " + enemies[i].dead);
        }
        */

        if (enemies[0].dead && enemies[1].dead && enemies[2].dead && enemies[3].dead && enemies[4].dead && enemies[5].dead)
        {
            enemiesDead = true;
        }

        if (enemiesDead)
        {
            UI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }

        if (player.dead)
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
