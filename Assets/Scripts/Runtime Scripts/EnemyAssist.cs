using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpace
{
    public Vector2 position;
    public float radius;
    public bool isSpaceAvailable;
}

public class EnemyAssist : MonoBehaviour
{
    public CircleSpace[] spaces;

    void Update()
    {
        for (int i = 0; i < spaces.Length - 1; i++)
        {
            if (Physics2D.OverlapCircle(spaces[i].position, spaces[i].radius) != null)
            {
                spaces[i].isSpaceAvailable = true;
            }
            else
            {
                spaces[i].isSpaceAvailable = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < spaces.Length - 1; i++)
        {
            if (spaces[i].isSpaceAvailable)
            {
                //Draw a white circle if space is available
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(spaces[i].position, spaces[i].radius);
            }
            else
            {
                //Draw a red circle if space is not available
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(spaces[i].position, spaces[i].radius);
            }
        }
    }
}
