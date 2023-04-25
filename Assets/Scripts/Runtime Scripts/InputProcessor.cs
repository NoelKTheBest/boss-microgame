﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProcessor : MonoBehaviour
{
    //PREPARE TO MAKE A REMAPPING FEATURE THAT CAN WORK FOR BOTH CURRENT PROJECTS
    
    
    [HideInInspector] public float hDir, vDir, hDir2, vDir2;
    private float xPoint, yPoint, xPoint2, yPoint2;
    [HideInInspector] public float moveH, moveV, lookX, lookY;
    private bool attack, attack2, proj, proj2, evade, evade2;
    [HideInInspector] public bool abutton, pbutton, ebutton;
    [HideInInspector] public static bool pressedPause;
    [HideInInspector] public Vector2 movement;
    [HideInInspector] public Vector2 lookV;
    [HideInInspector] public Vector3 tempV;
    private Camera cam;
    [HideInInspector] public int controller;
    [HideInInspector] public float m;
    [HideInInspector] public float multiValH, multiValV;
    private Hitbox hb;
    Vector2 JoyVector;
    
    void Awake()
    {
        cam = FindObjectOfType<Camera>().GetComponent<Camera>();
        hb = GetComponent<Hitbox>();
        //m = stats.baseMoveSpd;
    }

    void Start()
    {
        multiValH = 1;
        multiValV = 1;
    }

    void Update()
    {
        //m = stats.baseMoveSpd;
        tempV = CalculatePlayerMouseVector();
        ProcessInputs();
        //AttacAndComboInputs();
    }

    //method to calculate mouse direction relative to the player
    Vector3 CalculatePlayerMouseVector()
    {
        Vector3 playerScreenCoord = cam.WorldToScreenPoint(transform.position);

        //will not need z axis
        float mx = Input.mousePosition.x;
        float my = Input.mousePosition.y;
        //float mz = Input.mousePosition.z;
        float px = playerScreenCoord.x;
        float py = playerScreenCoord.y;
        //float pz = playerScreenCoord.z;
        float xDif, yDif;//, zDif;
        float x, y;//, z;
        float ax, ay;
        float nx, ny;

        //using abs value tells us how far away the cursor is from the player in terms of x and y positions
        xDif = mx - px;
        yDif = my - py;
        //zDif = mz - pz;

        ax = Mathf.Abs(mx - px);
        ay = Mathf.Abs(my - py);

        x = transform.position.x + xDif;
        y = transform.position.y + yDif;
        //z = transform.position.z + zDif;

        nx = transform.position.x + ax;
        ny = transform.position.y + ay;

        //joyAngle = Mathf.Atan(lookX / lookY) * (180 / Mathf.PI);

        return new Vector3(x, y, 0);
    }
    
    public void ProcessInputs()
    {
        //These if statements are for determining whether we are dealing with a 
        //controller or keyboard in single player mode or online multiplayer
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1
            || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            controller = 0;
        }
        else if (Input.GetButton("Fire1"))
        {
            controller = 0;
        }
        else if (Input.GetButton("Fire2"))
        {
            controller = 0;
        }

        if (Input.GetAxis("HJoystick") > 0 || Input.GetAxis("HJoystick") < 0
            || Input.GetAxis("VJoystick") > 0 || Input.GetAxis("VJoystick") < 0)
        {
            controller = 1;
        }
        else if (Input.GetButton("F1Joystick"))
        {
            controller = 1;
        }
        else if (Input.GetButton("F2Joystick"))
        {
            controller = 1;
        }

        hDir = Input.GetAxisRaw("Horizontal");
        vDir = Input.GetAxisRaw("Vertical");

        hDir2 = Input.GetAxis("HJoystick");
        vDir2 = Input.GetAxis("VJoystick");

        //tempV = tempV / tempV.magnitude;

        xPoint = tempV.normalized.x;
        yPoint = tempV.normalized.y;

        xPoint2 = Input.GetAxis("RSXJoystick");
        yPoint2 = Input.GetAxis("RSYJoystick");
        
        attack = Input.GetButtonDown("Fire1");
        attack2 = Input.GetButtonDown("F1Joystick");
        
        proj = Input.GetButtonDown("Fire3");
        proj2 = Input.GetButtonDown("F3Joystick");
        
        evade = Input.GetKeyDown("Evade");
        evade2 = Input.GetKeyDown("EJoystick");

        if (controller == 0 && !PauseMenu.isGamePaused)
        {
            moveH = hDir;
            moveV = vDir;
            moveH = new Vector2(moveH, moveV).normalized.x; //might not need
            moveV = new Vector2(moveH, moveV).normalized.y;
            lookX = xPoint;
            lookY = yPoint;
            abutton = attack;
            pbutton = proj;
            ebutton = evade;
            multiValH = 1; //figure out why these vars exist
            multiValV = 1;

            
            //The components of these vectors can be used interchangeably with each other, 
            //but not with the variables that make up the vectors.
            //base movement vector
            movement = new Vector2(moveH, moveV).normalized * m;

            //base look vector
            lookV = new Vector2(lookX, lookY).normalized * m;
        }
        else if (controller == 1 && !PauseMenu.isGamePaused)
        {
            moveH = hDir2;
            moveV = vDir2;
            moveH = new Vector2(moveH, moveV).normalized.x; //might not need
            moveV = new Vector2(moveH, moveV).normalized.y;
            lookX = xPoint2;
            lookY = yPoint2;
            abutton = attack2;
            pbutton = proj2;
            multiValH = Mathf.Abs(Input.GetAxis("HJoystick")); //figure out why these vars exist
            multiValV = Mathf.Abs(Input.GetAxis("VJoystick"));
            JoyVector = new Vector2(lookY, lookX);

            //test later:
            //Debug.Log(new Vector2(moveH, moveV));
            //Debug.Log(new Vector2(moveH, moveV).normalized);

            //base movement vector
            movement = new Vector2(moveH, moveV).normalized * m;//might not need to be normalized for joystick controls

            //base look vector
            lookV = new Vector2(lookX, lookY).normalized * m;
        }

        AttacAndComboInputs();
    }

    void AttacAndComboInputs()
    {
        // read inputs for attacks and combos and potentially reset ready variables if needed
    }
}
