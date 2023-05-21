using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    //public bool hitSomeone;
    public float currentHP;
    public float currentAtk;
    public float currentDef;
    public float currentSpd;
    private float hp;//100
    private float atk;//50
    private float def;//30
    [HideInInspector] public float baseMoveSpd;
    public bool dead;
    [HideInInspector] public float attackPotential;
    [HideInInspector] public float force;
    [HideInInspector] public HealthBar hb;
    public DamageTaken OnDamageTaken;
    private PlayerPreset pp;
    private EnemyPreset ep;
    private HealingItem drinks;
    //private Animator amim;
    [Tooltip("Only applicable if gameobject is a bullet prefab.")]public float bulletPower;

    void Awake()
    {
        drinks = Resources.Load<HealingItem>("Scriptable Objects/Items/Maintenance Drink");
        if (drinks.itemCount > 0) SceneData.playerHasHealingItems = true;

        if (gameObject.tag == "Enemy")
        {
            hb = GetComponentInChildren<HealthBar>();
            ep = Resources.Load<EnemyPreset>("Scriptable Objects/Entities/" + gameObject.name);
            
            hp = ep.hp;
            hb.SetHP(hp);
            atk = ep.atk;
            def = ep.def;
            baseMoveSpd = ep.baseMoveSpd;
        }
        else if (gameObject.tag == "Player")
        {

            //put in charactercontroller
            pp = Resources.Load<PlayerPreset>("Scriptable Objects/Entities/" + gameObject.name);

            hp = pp.hp;
            //hb.SetHP(100);
            atk = pp.atk;
            def = pp.def;
            baseMoveSpd = pp.baseMoveSpd;
        }
        else if (gameObject.tag == "Projectile")
        {
            atk = bulletPower;
        }
        
        if (gameObject.tag != "Projectile")
        {
            //hb.SetHP(hp);
            currentHP = hp;
            //currentHP -= 50;
            //if (gameObject.tag == "Player") hb.SubtractFromHP(currentHP, hp);
            currentAtk = atk;
            currentDef = def;
            currentSpd = baseMoveSpd;
            dead = false;
            attackPotential = 1;
        }
        else
        {
            currentAtk = atk;
            attackPotential = 1f;
        }
    }

    void Update()
    {

        //Debug.Log(currentAtk + ": " + gameObject.tag);
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
}
