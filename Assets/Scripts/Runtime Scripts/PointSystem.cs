using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make 2 separate classes for both the enemy and the player
public class PointSystem : MonoBehaviour
{
    public Transform pointBar;
    [Tooltip("Default amount of points, (Keep at 0)")]
    public float currentPoints = 0;
    private float previousPoints = 0;
    private float currentFloat;
    private float previousFloat;
    [Tooltip("Amount of point boost to apply to damage dealt, (Range: From 0 to 2)"), Range(0, 2)]
    public float pointGainFromDmgPercent;
    public float atkACost; //change tier1 to atkA and tier2 to atkB
    public float atkBCost;
    public float abilityCost;
    public float uAbilityCost;
    [HideInInspector] public bool atkAUpgrade;
    [HideInInspector] public bool atkBUpgrade;
    [HideInInspector] public bool abilityUse;
    [HideInInspector] public bool uAbilityUse;
    public bool wasAbilityUsed;
    public bool isPlayerUpgraded;

    void Awake()
    {
        currentPoints += 999; // testing currently
    }

    void Update()
    {
        atkAUpgrade = (currentPoints >= atkACost) ? true : false;
        atkBUpgrade = (currentPoints >= atkBCost) ? true : false;
        abilityUse = (currentPoints >= abilityCost) ? true : false;
        uAbilityUse = (currentPoints >= uAbilityCost) ? true : false;

        previousFloat = pointBar.localScale.x;
        currentFloat = currentPoints / 999;
        float newFloat = ((currentFloat - previousFloat) / 10) + pointBar.localScale.x;
        if (newFloat < 0) newFloat = 0;
        //Debug.Log("p: " + previousFloat);
        //Debug.Log("c: " + currentFloat);
        //Debug.Log("n: " + newFloat);
        pointBar.localScale = new Vector2(newFloat, pointBar.localScale.y);
    }
    
    //Adds points to current total while taking into account boosters
    public void AddPoints(float damage)
    {
        if (!isPlayerUpgraded || !wasAbilityUsed)
        {
            damage = Mathf.Floor(damage * pointGainFromDmgPercent);
            previousPoints = currentPoints;
            currentPoints += damage;
            if (currentPoints > 999) currentPoints = 999;
        }
    }
    
    //Subtracts points from current total while taking into account boosters
    public void SubtractPoints(float points)
    {
        previousPoints = currentPoints;
        currentPoints -= points;
        if (currentPoints < 0) currentPoints = 0;
        //ApplyPointBoost(1);//Resets boost after damage calculation
    }

    //Changes the amount of point boost
    void ApplyPointBoost(float amount)
    {
        pointGainFromDmgPercent = amount;
    }

    public void ResetPoints()
    {
        currentPoints = 0;
    }
}

