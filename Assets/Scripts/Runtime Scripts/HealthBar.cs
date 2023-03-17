using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [HideInInspector]public float hptotal;
    [HideInInspector]public float currenthp;
    public float delayTime = 0.25f;
    public float smoothingValue = 20;
    public Color strongHitColor;
    public Color mediumHitColor;
    public Color weakHitColor;
    public Transform losingBar;
    SpriteRenderer barSprite;
    private float previousFloat;
    private float currentFloat;
    private float scaleDown;
    private float scaleUp;
    private bool startUpdating;

    void Awake()
    {
        barSprite = losingBar.GetComponent<SpriteRenderer>();
    }
    
    public void SetHP(float hp)
    {
        hptotal = hp;
        currenthp = hptotal;
        //Debug.Log(currenthp);
    }

    void Update()
    {
        //Debug.Log(currenthp);
        previousFloat = losingBar.localScale.x;
        currentFloat = currenthp / hptotal;

        float newFloat = ((currentFloat - previousFloat) / 20) + losingBar.localScale.x;
        if (newFloat < 0) newFloat = 0;
        if (newFloat > 1) newFloat = 1;
        losingBar.localScale = new Vector2(newFloat, losingBar.localScale.y);
    }

    public void SubtractFromHP(float currenthp, float hp)
    {
        scaleDown = currenthp / hp;
        this.currenthp = currenthp;
        scaleDown = Mathf.Clamp(scaleDown, 0, 1);
        transform.localScale = new Vector2(scaleDown, 1);
    }

    public void AddToHP(float currenthp, float hp)
    {
        scaleUp = currenthp / hp;
        this.currenthp = currenthp;
        scaleUp = Mathf.Clamp(scaleUp, 0, 1);
        transform.localScale = new Vector2(scaleUp, 1);
    }

    public void SetBarColor(int hitboxStrength)
    {
        switch (hitboxStrength)
        {
            case 0:
                barSprite.color = mediumHitColor;
                break;
            case 1:
                barSprite.color = strongHitColor;
                break;
            case 2:
                barSprite.color = weakHitColor;
                break;
        }
    }
}
