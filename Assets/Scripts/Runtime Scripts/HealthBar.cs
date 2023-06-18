using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [HideInInspector]public float hptotal;
    [HideInInspector]public float currenthp;
    public float delayTime = 0.25f;
    public float smoothingValue = 20;
    public Color strongHitColor;
    public Color mediumHitColor;
    public Color weakHitColor;
    public RectTransform losingBar;
    public Image barSprite;
    private float previousFloat;
    private float currentFloat;
    private float scaleDown;
    private float scaleUp;
    private bool startUpdating;

    void Awake()
    {
        //barSprite = losingBar.GetComponent<SpriteRenderer>();
    }
    
    public void SetHP(float hp)
    {
        //Debug.Log("Someone called me!");
        Debug.Log(hp);
        hptotal = hp;
        currenthp = hptotal;
    }

    void Update()
    {
        previousFloat = losingBar.localScale.x;
        currentFloat = currenthp / hptotal;

        float newFloat = ((currentFloat - previousFloat) / 20) + losingBar.localScale.x;
        if (newFloat < 0) newFloat = 0;
        if (newFloat > 1) newFloat = 1;
        //Debug.Log(newFloat);
        //Debug.Log(currentFloat - previousFloat);
        //Debug.Log("previous: " + previousFloat);
        //Debug.Log(hptotal);
        
        losingBar.localScale = new Vector3(newFloat, losingBar.localScale.y, losingBar.localScale.z);
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
