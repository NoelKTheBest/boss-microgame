using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseHitbox : MonoBehaviour
{
    public ControlT player;

    public GameObject playerTestHitbox;
    public GameObject enemyTestHitbox;
    private Camera cam;
    private SpriteRenderer srp;
    private SpriteRenderer sre;
    private BoxCollider2D bc2Dp;
    private BoxCollider2D bc2De;
    bool buttonClicked;
    bool hitboxActivated;

    // 0 for enemy; 1 for player
    int hitboxType;

    void Awake()
    {
        cam = Camera.main;
        srp = playerTestHitbox.GetComponent<SpriteRenderer>();
        sre = enemyTestHitbox.GetComponent<SpriteRenderer>();
        bc2Dp = playerTestHitbox.GetComponent<BoxCollider2D>();
        bc2De = enemyTestHitbox.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        hitboxType = 0;

    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldCoord = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldCoord.z = -1;
        transform.position = mouseWorldCoord;
    }

    void OnGUI()
    {
        GUI.skin.button.fontSize = 75;

        if (!buttonClicked)
        {
            if (GUILayout.Button("Test Hurtbox"))
            {
                if (hitboxType == 0)
                {
                    // enable enemy
                    sre.enabled = true;
                    //bc2De.enabled = true;

                    // disable player
                    srp.enabled = false;
                    //bc2Dp.enabled = false;
                }
                else if (hitboxType == 1)
                {
                    // enable player
                    srp.enabled = true;
                    //bc2Dp.enabled = true;

                    // disable enemy
                    sre.enabled = false;
                    //bc2De.enabled = false;
                }

                buttonClicked = true;
                player.testingHurtbox = true;
            }
        }
        else if (buttonClicked)
        {
            // enable and disable components as needed
            if (hitboxType == 0)
            {
                // enable enemy
                sre.enabled = true;
                //bc2De.enabled = true;

                // disable player
                srp.enabled = false;
                //bc2Dp.enabled = false;
            }
            else if (hitboxType == 1)
            {
                // enable player
                srp.enabled = true;
                //bc2Dp.enabled = true;

                // disable enemy
                sre.enabled = false;
                //bc2De.enabled = false;
            }

            // disable all components
            if (GUILayout.Button("Stop Testing Hurtbox"))
            {
                // diable enemy
                sre.enabled = false;
                //bc2De.enabled = false;

                // disable player
                srp.enabled = false;
                //bc2Dp.enabled = false;

                buttonClicked = false;
                player.testingHurtbox = false;
            }
        }

        if (hitboxType == 0)
        {
            if (GUILayout.Button("Use Player Hitboxes"))
            {
                hitboxType = 1;
            }
        }
        else if (hitboxType == 1)
        {
            if (GUILayout.Button("Use Enemy Hitboxes"))
            {
                hitboxType = 0;
            }
        }

        // Test the hitboxes later
        //  right now when hovering over the player with the mouse, nothing happens.
        //  that is what I want, but I need to know what's keeping the physics overlap
        //  detecting the hit, so that I know how to make sure that physics overlap
        //  detects hits when i am testing the hurtbox.

        if (Input.GetButtonDown("Fire1"))
        {
            if (!hitboxActivated) StartCoroutine(ActivateHitbox());
        }
    }

    IEnumerator ActivateHitbox()
    {
        hitboxActivated = true;

        for (float duration = 0.1f; duration > 0; duration = -Time.deltaTime)
        {
            if (hitboxType == 0)
            {
                // enable enemy
                bc2De.enabled = true;
            }
            else if (hitboxType == 1)
            {
                // enable player
                bc2Dp.enabled = true;
            }
            yield return null;
        }

        if (hitboxType == 0)
        {
            // disable enemy
            bc2De.enabled = false;
        }
        else if (hitboxType == 1)
        {
            // disable player
            bc2Dp.enabled = false;
        }

        hitboxActivated = false;
    }
}
