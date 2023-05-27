using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    void Awake()
    {
        if (SceneData.playerEnteredFromDoor)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            DoorScript[] doors = FindObjectsOfType<DoorScript>();

            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].sceneBuildIndex == SceneData.previousSceneBuildIndex)
                {
                    player.transform.position = doors[i].playerSceneStartPosition.position;
                }
            }

        }
    }

    void Start()
    {
        StartCoroutine(WaitForTransition());
    }

    public void ChangeScene()
    {
        //StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        StartCoroutine(LoadScene(SceneData.nextSceneIndex));
    }

    IEnumerator WaitForTransition()
    {
        PauseMenu.isGamePaused = true;

        yield return new WaitForSeconds(0.9f);

        PauseMenu.isGamePaused = false;
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneData.previousSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnApplicationQuit()
    {
        SceneData.playerEnteredFromDoor = false;
    }
}
