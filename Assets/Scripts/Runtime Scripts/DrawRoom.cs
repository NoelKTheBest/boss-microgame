using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRoom : MonoBehaviour
{
    public BoxCollider2D room;

    void OnDrawGizmos()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(room.bounds.center, room.size);
    }
}
