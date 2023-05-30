using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageTaken : UnityEvent<float> { }

public class PlayerStatistics : MonoBehaviour
{
    //public bool hitSomeone;
    public float currentHP;
    public float currentAtk;
    public float currentDef;
    public float currentSpd;
    private float hp;//100
    private float atk;//50
    private float def;//30
    public bool dead;
    [HideInInspector] public float attackPotential;
    [HideInInspector] public float force;
    [HideInInspector] public HealthBar hb;
    public DamageTaken OnDamageTaken;
    //private HealingItem drinks;

    #region For Healing
    /*
    void Awake()
    {
        //drinks = Resources.Load<HealingItem>("Scriptable Objects/Items/Maintenance Drink");
        //if (drinks.itemCount > 0) SceneData.playerHasHealingItems = true;
    }
    
    public void UseHealingItem()
    {
        if (drinks.itemCount > 0)
        {
            drinks.itemCount--;
            currentHP += drinks.healingAmount;
            if (currentHP > hp) currentHP = hp;
            hb.AddToHP(currentHP, hp);
        }
        if (drinks.itemCount == 0)
        {
            SceneData.playerHasHealingItems = false;
        }
    }
    */
    #endregion

    void Update()
    {
        if (currentHP < 0)
        {
            dead = true;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (dmg == 0) return;
        float d = dmg - currentDef;
        if (d <= 0) return;

        currentHP -= (dmg - currentDef);
        if (currentHP < 0) currentHP = 0;
        hb.SubtractFromHP(currentHP, hp);

        if (gameObject.tag == "Enemy" && OnDamageTaken != null)
        {
            OnDamageTaken.Invoke(dmg);
        }
    }

    public void ApplyKnockback(float magnitude, Vector2 direction)
    {
        // Apply knockback to player
    }
}
