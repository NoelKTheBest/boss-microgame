using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePrefab : MonoBehaviour
{
    public float durationTime = 3f;
    float newTime;

    private void Start()
    {
        newTime = Time.fixedTime + durationTime;
    }

    private void Update()
    {
        if (Time.fixedTime > newTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.GetMask("Player Hitboxes") || 
            collision.gameObject.layer != LayerMask.GetMask("Enemy Hitboxes"))
            gameObject.SetActive(false);
    }
}
