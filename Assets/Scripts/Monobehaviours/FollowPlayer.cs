using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject playerObject;
    [HideInInspector] public GameObject roomObject;
    private Transform playerPosition;
    private Collider2D room;
    private Component camObject;
    private Camera cam;
    private float xTo, yTo;
    private float xMove, yMove;
    private float xBegR, yBegR, xEndR, yEndR;
    private float xBegV, yBegV, xEndV, yEndV;
    [SerializeField] private int a = 15;
    private Bounds camBounds;
    private float camWidth;
    private float xPos, yPos;
    //private Vector2 velocity;
    private float minCameraPosX, minCameraPosY, maxCameraPosX, maxCameraPosY;
    //public int a = 15;
    public int zCoord;

    void Awake()
    {
        //playerObject = GameObject.FindGameObjectWithTag("Player");
        playerPosition = playerObject.GetComponent<Transform>();
        roomObject = GameObject.FindGameObjectWithTag("Room");
        room = roomObject.GetComponent<Collider2D>();
        cam = GetComponent<Camera>();
        camBounds = CameraExtensions.OrthographicBounds(cam);
        camWidth = CameraExtensions.width(cam);
        //a = 15;
    }

    private void Start()
    {
        xEndR = room.bounds.max.x;
        yEndR = room.bounds.max.y;
        xBegR = room.bounds.min.x;
        yBegR = room.bounds.min.y;

        minCameraPosX = xBegR + (camWidth / 2);
        minCameraPosY = yBegR + cam.orthographicSize;// * 2;
        maxCameraPosX = xEndR - (camWidth / 2);
        maxCameraPosY = yEndR - cam.orthographicSize;// * 2;

        //velocity.x = 15;
        //velocity.y = 15;

        //a = 15;
    }

    //Fixed
    void FixedUpdate()
    {
        camBounds = CameraExtensions.OrthographicBounds(cam);
        camWidth = CameraExtensions.width(cam);

        xTo = playerPosition.position.x;
        yTo = playerPosition.position.y;

        xMove = (xTo - transform.position.x) / a;
        yMove = (yTo - transform.position.y) / a;

        xPos = xMove + transform.position.x;
        yPos = yMove + transform.position.y;

        
        transform.position = new Vector3(Mathf.Clamp(xPos, minCameraPosX, maxCameraPosX),
            Mathf.Clamp(yPos, minCameraPosY, maxCameraPosY), zCoord);
    }
}
