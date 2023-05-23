using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This will allow me to alter the boss' behaviour based one how much damage they have taken
public enum BossHealthState { HEALTHY, HURT, WEAK, LAST_LEGS};

public enum BossPhase { FIRST, SECOND};

public class BossStatistics : MonoBehaviour
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
    private BossHurtbox myHurtbox;
    public DamageTaken OnDamageTaken;

    // Start is called before the first frame update
    void Start()
    {
        //EnvironmentStatus.fireExists = false;
        //EnvironmentStatus.fireExists

    }

    // Update is called once per frame
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

    public void ReFillHPBar()
    {
        // Fill HP bar again for boss' second phase
    }

    public void ApplyKnockback(float magnitude, Vector2 direction)
    {
        // Apply knockback to player
    }
}
