using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
    public Animator animator;
    public Vector2 boxsize;
    public Vector2 boxcenter;
    public Vector2 boxOffset;
    public int sceneBuildIndex;
    bool functionCalled = false;
    public UnityEvent OnChangeScene;
    public Transform playerSceneStartPosition;
    public LayerMask mask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger("Open");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetTrigger("Close");
    }

    void Start()
    {
        //if entered from door
        //SetupPlayer();
    }

    void Update()
    {
        boxcenter = (Vector2)transform.position + boxOffset;
        Collider2D playerCollider = new Collider2D();
        playerCollider = Physics2D.OverlapBox(boxcenter, boxsize, 0, mask);

        if (playerCollider != null && playerCollider.gameObject.tag == "Player" && !functionCalled)
        {
            functionCalled = true;
            SceneData.playerEnteredFromDoor = true;
            SceneData.nextSceneIndex = sceneBuildIndex;
            if (OnChangeScene != null) OnChangeScene.Invoke();
        }
    }
    
    private void OnDrawGizmos()
    {
        boxcenter = (Vector2)transform.position + boxOffset;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(boxcenter, boxsize);
    }
}
