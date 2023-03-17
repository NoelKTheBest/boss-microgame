using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public int dummyHP;
    public Transform playerPos;

    private Stats stats;
    private Hitbox hitbox;
    private SpriteRenderer sr;
    public Sprite[] sprites;
    
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        stats = GetComponent<Stats>();
        hitbox = GetComponent<Hitbox>();
    }
    
    void Update()
    {
        if (playerPos.position.y >= transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, playerPos.position.z - 1);
        }

        if (playerPos.position.y < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, playerPos.position.z + 1);
        }

        if (stats.currentHP <= 0) stats.currentHP = dummyHP;

        if (stats.currentHP > stats.currentHP * 0.5f) sr.sprite = sprites[0];
        if (stats.currentHP <= stats.currentHP * 0.5f) sr.sprite = sprites[1];
    }
}
