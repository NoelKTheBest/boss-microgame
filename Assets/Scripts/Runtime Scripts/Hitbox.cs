using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HitDetected : UnityEvent<Vector2> { }

[System.Serializable]
public class DashAttackConnected : UnityEvent<Vector2> { }

//may rename later to DetectCollisions
public class Hitbox : MonoBehaviour
{
    //private int i = 3;
    [HideInInspector] public bool wasHit;
    private bool checkForNoContact;
    //private bool doDodgeCheck;
    public float blockHealth;//i could get rid of this
    private float currentBlockHealth;//and this //might need to be public
    public float blockDefense;//and this too
    public float hitboxHealth;//and this
    private float currenthitboxHealth;//and this
    public float hitboxDefense;//and this
    public float hbStrongMultiplier;//and this
    public float hbWeakMultiplier;//and this
    [HideInInspector] public float forceOfAttack;
    public bool isPlayerUpgraded;
    public bool hasPlayerDashed;
    public bool hitByProjectile;
    [HideInInspector] public Stats myStats;
    private Rigidbody2D rb;
    public HitDetected OnHitDetected;
    public UnityEvent OnDashAttackConnected;
    private Vector2 hitDirection;
    public Vector2 hitboxPoint;
    public Vector2 hitboxOffset;
    public Vector2 hitboxSize;
    public Vector2 animHitboxOffset;
    public Vector2 hitboxSizeOffset;
    public enum HitboxType
    {
        regular
    }
    public enum HitboxStrength
    {
        medium,
        strong,
        weak
    }
    public HitboxType hitboxType;
    public HitboxStrength hitboxStrength;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myStats = GetComponent<Stats>();
        //anim = GetComponent<Animator>();
        wasHit = false;
        checkForNoContact = false;
    }

    void Update()
    {
        hitboxPoint = (Vector2)transform.position + hitboxOffset + animHitboxOffset;
        DetectHit();
    }

    void DetectHit()
    {
        LayerMask mask1 = LayerMask.GetMask("Player Hitboxes");
        LayerMask mask2 = LayerMask.GetMask("Enemy Hitboxes");
        Collider2D collider1 = new Collider2D();// = Physics2D.OverlapBox(hitboxPoint, hitboxSize, 0, mask1);
        Collider2D collider2 = new Collider2D();// = Physics2D.OverlapBox(hitboxPoint, hitboxSize, 0, mask2);

        if (gameObject.tag == "Enemy")
        {
            collider1 = Physics2D.OverlapBox(hitboxPoint, hitboxSize + hitboxSizeOffset, 0, mask1);
        }

        if (gameObject.tag == "Player")
        {
            collider2 = Physics2D.OverlapBox(hitboxPoint, hitboxSize + hitboxSizeOffset, 0, mask2);
        }
        
        if (hitboxType == HitboxType.regular)
        {
            if (wasHit == false && collider1 != null && gameObject.tag == "Enemy")
            {
                wasHit = true;
                float b = 0;
                float hbMultiplier = 0;

                Hitbox hb = collider1.GetComponentInParent<Hitbox>();
                Stats stats = hb.myStats;
                Vector2 a = transform.position - collider1.transform.position;
                forceOfAttack = stats.force;
                //Debug.Log(stats.currentAtk);

                if (hb.gameObject.tag == "Projectile") hitByProjectile = true;

                switch (hitboxStrength)
                {
                    case HitboxStrength.medium:
                        hbMultiplier = 1;
                        break;
                    case HitboxStrength.strong:
                        hbMultiplier = hbStrongMultiplier;
                        break;
                    case HitboxStrength.weak:
                        hbMultiplier = hbWeakMultiplier;
                        break;
                }
                
                if (hb.isPlayerUpgraded && hb.hasPlayerDashed)
                {
                    //Debug.Log("atk: " + stats.currentAtk);
                    //Debug.Log("pot: " + stats.attackPotential);
                    //Debug.Log("b: " + b);
                    //Debug.Log("mul: " + hbMultiplier);
                    //Debug.Log(((stats.currentAtk * stats.attackPotential) + b) * hbMultiplier);
                    if (OnDashAttackConnected != null) OnDashAttackConnected.Invoke();
                }

                rb.velocity = a * (stats.force);
                //Debug.Log(stats.attackPotential);
                Debug.Log(((stats.currentAtk * stats.attackPotential) + b) * hbMultiplier);
                myStats.TakeDamage(((stats.currentAtk * stats.attackPotential) + b) * hbMultiplier);
                if (OnHitDetected != null) OnHitDetected.Invoke(a);
                
                checkForNoContact = true;
            }
            
            if (wasHit == false && collider2 != null && gameObject.tag == "Player")
            {
                wasHit = true;

                Hitbox hb = collider2.GetComponentInParent<Hitbox>();
                Stats stats = hb.myStats;
                Vector2 a = transform.position - collider2.transform.position;
                forceOfAttack = stats.force;

                if (hb.gameObject.tag == "Projectile") hitByProjectile = true;

                rb.velocity = a * (stats.force);
                myStats.TakeDamage(stats.currentAtk * stats.attackPotential);
                if (OnHitDetected != null) OnHitDetected.Invoke(a);

                checkForNoContact = true;
            }

            if (checkForNoContact)
            {
                if (gameObject.tag == "Enemy")
                {
                    if (collider1 == null)
                    {
                        wasHit = false;
                    }
                }

                if (gameObject.tag == "Player")
                {
                    if (collider2 == null)
                    {
                        wasHit = false;
                    }
                }
            }
        }
    }
    
    //called during animation events
    public void ChangePlayerHitboxType(string currentStyle)
    {
        switch (currentStyle)
        {
            case "regular":
                hitboxType = HitboxType.regular;
                break;
        }
    }

    public void ChangeEnemyHitboxType(int num)
    {
        switch (num)
        {
            case 0:
                hitboxType = HitboxType.regular;
                break;
        }
    }

    public void ChangeHitboxStrength(int num)
    {
        switch (num)
        {
            case 0:
                hitboxStrength = HitboxStrength.medium;
                myStats.hb.SetBarColor(0);
                break;
            case 1:
                hitboxStrength = HitboxStrength.strong;
                myStats.hb.SetBarColor(1);
                break;
            case 2:
                hitboxStrength = HitboxStrength.weak;
                myStats.hb.SetBarColor(2);
                break;
        }
    }
    
    /*
    //this method will get called by the player's main animator through its own hit animation events
    public void ResetHitVar(bool a)
    {
        wasHit = false;
    }
    */
    
    //must always show the hitbox for the character in question
    void OnDrawGizmos()
    {
        hitboxPoint = (Vector2)transform.position + hitboxOffset + animHitboxOffset;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(hitboxPoint, hitboxSize + hitboxSizeOffset);
    }
}
