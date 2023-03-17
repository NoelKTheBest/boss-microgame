using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private GameObject[] roomObjects;

    // Start is called before the first frame update
    void Awake()
    {
        roomObjects = GameObject.FindGameObjectsWithTag("Room");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
