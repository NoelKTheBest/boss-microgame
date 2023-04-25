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
    [HideInInspector] public float baseMoveSpd;
    public bool dead;
    [HideInInspector] public float attackPotential;
    [HideInInspector] public float force;
    [HideInInspector] public HealthBar hb;
    public DamageTaken OnDamageTaken;
    private PlayerPreset pp;
    private HealingItem drinks;

    void Awake()
    {
        drinks = Resources.Load<HealingItem>("Scriptable Objects/Items/Maintenance Drink");
        if (drinks.itemCount > 0) SceneData.playerHasHealingItems = true;

        if (gameObject.tag == "Player")
        {

            //put in charactercontroller
            pp = Resources.Load<PlayerPreset>("Scriptable Objects/Entities/" + gameObject.name);

            hp = pp.hp;
            hb.SetHP(100);
            atk = pp.atk;
            def = pp.def;
            baseMoveSpd = pp.baseMoveSpd;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
