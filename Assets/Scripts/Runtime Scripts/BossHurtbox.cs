using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
//public class HitDetected : UnityEvent<Vector2> { }

public enum HitboxStrength
{
    medium,
    strong,
    weak
}

public class BossHurtbox : MonoBehaviour
{
    [HideInInspector] public bool wasHit;
    private bool checkForNoContact;
    public bool hitByProjectile;
    public float hitboxHealth;
    private float currenthitboxHealth;

    public HitboxStrength hitboxStrength;

    private BossStatistics myStats;
    private Rigidbody2D rb;

    private Vector2 hitDirection;
    public Vector2 hitboxPoint;
    public Vector2 hitboxOffset;
    public Vector2 hitboxSize;
    public Vector2 animHitboxOffset;
    public Vector2 hitboxSizeOffset;

    //The boss hurtbox doesn't need to worry about the player having a higher damage output and calculating damage 
    //using values belonging to the any of the player's classes. For the boss, she can again only concern herself with
    //only accepting that she was hit and that she needs to take damage. The difference maker will be in the defensive
    //stat of the boss.

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myStats = GetComponent<BossStatistics>();
        //anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        wasHit = false;
        checkForNoContact = false;
    }

    // Update is called once per frame
    void Update()
    {
        hitboxPoint = (Vector2)transform.position + hitboxOffset + animHitboxOffset;
        DetectHit();
    }

    void DetectHit()
    {
        //review the tutorial on melee combat
        //the mask makes it so that the collider only checks for things under the mask
        LayerMask mask = LayerMask.GetMask("Player Hitboxes");
        Collider2D collider = new Collider2D();// = Physics2D.OverlapBox(hitboxPoint, hitboxSize, 0, mask1);

        //collider1.GetComponent<AttackCollider>();

        collider = Physics2D.OverlapBox(hitboxPoint, hitboxSize + hitboxSizeOffset, 0, mask);

        if (wasHit == false && collider != null)
        {
            wasHit = true;
            float b = 0;
            float hbMultiplier = 0;

            //finding the hitbox component of the boss might be fine, but i need to ditch the stats class
            Hitbox hb = collider.GetComponentInParent<Hitbox>();
            Vector2 attackVector = transform.position - collider.transform.position;

            if (hb.gameObject.tag == "Projectile") hitByProjectile = true;

            switch (hitboxStrength)
            {
                case HitboxStrength.medium:
                    hbMultiplier = 1;
                    break;
                case HitboxStrength.strong:
                    hbMultiplier = 1;
                    break;
                case HitboxStrength.weak:
                    hbMultiplier = 1;
                    break;
            }

            //rb.velocity = attackVector * (stats.force);
            //Debug.Log(((stats.currentAtk * stats.attackPotential) + b) * hbMultiplier);
            //myStats.TakeDamage(((stats.currentAtk * stats.attackPotential) + b) * hbMultiplier);
            //if (OnHitDetected != null) OnHitDetected.Invoke(attackVector);

            checkForNoContact = true;
        }

        if (checkForNoContact)
        {
            if (collider == null)
            {
                wasHit = false;
                // the line below was not previously here. I'm not sure why i didn't include it. 
                //  i might have just forgot to include it
                checkForNoContact = false;
            }
        }
    }

    public void ChangeHitboxStrength(int num)
    {
        switch (num)
        {
            case 0:
                hitboxStrength = HitboxStrength.medium;
                //myStats.hb.SetBarColor(0);
                break;
            case 1:
                hitboxStrength = HitboxStrength.strong;
                //myStats.hb.SetBarColor(1);
                break;
            case 2:
                hitboxStrength = HitboxStrength.weak;
                //myStats.hb.SetBarColor(2);
                break;
        }
    }

    void OnDrawGizmos()
    {
        hitboxPoint = (Vector2)transform.position + hitboxOffset + animHitboxOffset;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(hitboxPoint, hitboxSize + hitboxSizeOffset);
    }
}
