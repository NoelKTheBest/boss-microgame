using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BiohazardController : Character, IEnemyCharacter
{
    [Tooltip("Proximity Range Order is as they appear in the array for all properties")] public string tip;
    private bool abutton;
    private Vector2 movement;
    private Animator animator;
    private Camera cam;
    private CircleCollider2D col;
    private Rigidbody2D rb;
    private Hitbox hitbox;
    private Stats stats;
    private AudioSource sound;
    public AudioClip[] clips;
    private float multiValH, multiValV;
    [HideInInspector] public bool attackReady;
    //[HideInInspector] public bool specialReady;
    [HideInInspector] public bool dashReady;
    private bool dead;
    bool idle;
    bool moving;
    bool inPosition;
    public Transform transformationPosition;
    public UnityEvent OnEnterPosition;

    [HideInInspector] public Transform targetPos;
    private float targetXPosition;
    private float targetYPosition;

    string currentDirection;
    string previousDirection;

    float x, y;
    
    [HideInInspector] public Vector3 tempV;
    private Vector2 lookVector;

    bool attacked = false;
    int dirState;

    public float radius1;
    public float radius2;
    public float radius3;
    public float radius4;
    public float radiusOffset;
    public Vector2 detectionCenter;
    public Vector2 centerOffset;
    public Vector2 animCenterOffset;

    [HideInInspector] public float attackScalar;
    private int animNumber;
    private float movementMultiplier;
    public bool[] attackTimes;
    public int[] animNumbers;
    public float[] movementMultipliers;

    //private ContactFilter2D contactFilter;

    void Awake()
    {
        dead = false;
        attackReady = true;
        dashReady = true;
        stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();
        hitbox = GetComponent<Hitbox>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        cam = FindObjectOfType<Camera>().GetComponent<Camera>();
        sound = GetComponentInChildren<AudioSource>();
        targetPos = FindObjectOfType<ControlT>().transform;
        idle = true;
    }

    void Start()
    {
        currentDirection = "right";
        previousDirection = currentDirection;
        multiValH = 1;
        multiValV = 1;
    }
    
    void Update()
    {
        if (targetPos.position.y >= transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, targetPos.position.z - 1);
        }

        if (targetPos.position.y < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, targetPos.position.z + 1);
        }

        if (!dead)
        {
            if (stats.currentHP <= 0)
            {
                if (!dead)
                {
                    Die();
                }

                dead = true;
                stats.dead = true;
            }

            targetXPosition = targetPos.position.x;
            targetYPosition = targetPos.position.y;

            DetermineInputs();

            DetermineDirections();

            AnimateAttacks();

            if (!hitbox.wasHit) rb.velocity = new Vector2(movement.x * multiValH, movement.y * multiValV)
                    * Time.timeScale;
            
            if (!hitbox.wasHit)
            {
                AnimateMovement();
            }

            if (movementMultiplier != -1)
            {
                if (lookVector.x < 0)
                {
                    animator.SetInteger("Direction", 4);
                }
                else if (lookVector.x > 0)
                {
                    animator.SetInteger("Direction", 3);
                }
            }

            //Debug.Log(moveH + " + " + moveV + " + " + previousDirection); DON'T DELETE THIS LINE

        }
        else
        {
            rb.velocity = Vector2.zero;
            col.enabled = false;
        }
    }

    public void DetermineInputs()
    {
        //int i = 0;
        int j = 0;

        detectionCenter = (Vector2)transform.position + centerOffset + animCenterOffset;

        x = targetPos.position.x - transform.position.x;
        y = targetPos.position.y - transform.position.y;

        lookVector = new Vector2(x, y);
        //lookVector = lookVector / lookVector.magnitude;
        //lookVector = lookVector.normalized;

        bool lFarAwayX = targetPos.position.x > transform.position.x;
        bool gFarAwayX = targetPos.position.x < transform.position.x;
        bool lFarAwayY = targetPos.position.y > transform.position.y;
        bool gFarAwayY = targetPos.position.y < transform.position.y;

        //also setup attack potential values
        if (lookVector.magnitude <= (radius1 + radiusOffset) && !PauseMenu.isGamePaused)
        {
            abutton = attackTimes[j];
            animNumber = animNumbers[j];
            movementMultiplier = movementMultipliers[j];
            movement = lookVector.normalized * stats.baseMoveSpd * attackScalar * movementMultiplier;
        }
        else if (lookVector.magnitude <= (radius2 + radiusOffset) && !PauseMenu.isGamePaused)
        {
            j = 1;
            abutton = attackTimes[j];
            animNumber = animNumbers[j];
            movementMultiplier = movementMultipliers[j];
            movement = lookVector.normalized * stats.baseMoveSpd * attackScalar * movementMultiplier;
        }
        else if (lookVector.magnitude <= (radius3 + radiusOffset) && !PauseMenu.isGamePaused)
        {
            j = 2;
            abutton = attackTimes[j];
            animNumber = animNumbers[j];
            movementMultiplier = movementMultipliers[j];
            movement = lookVector.normalized * stats.baseMoveSpd * attackScalar * movementMultiplier;
        }
        else if (lookVector.magnitude <= (radius4 + radiusOffset) && !PauseMenu.isGamePaused)
        {
            j = 3;
            abutton = attackTimes[j];
            animNumber = animNumbers[j];
            movementMultiplier = movementMultipliers[j];
            movement = lookVector.normalized * stats.baseMoveSpd * attackScalar * movementMultiplier;
            Debug.Log(attackScalar);
        }
        else
        {
            abutton = false;
            movement = Vector2.zero;
        }

        multiValH = 1;
        multiValV = 1;
    }

    public override void DetermineDirections()
    {
        int direction;

        bool rangeX = (lookVector.x > -0.7f) && (lookVector.x < 0.7f); //can do 0.71 or 0.707 for more precision
        bool rangeY = (lookVector.y > -0.7f) && (lookVector.y < 0.7f);
        bool xp = (lookVector.x > 0.5f) && (lookVector.x < 0.866f);
        bool yp = (lookVector.y > 0.5f) && (lookVector.y < 0.866f);
        bool xn = (lookVector.x < -0.5f) && (lookVector.x > -0.866f);
        bool yn = (lookVector.y < -0.5f) && (lookVector.y > -0.866f);

        if (lookVector.x > 0 && rangeY)
        {
            direction = 3;
            dirState = direction;
        }

        if (lookVector.x < 0 && rangeY)
        {
            direction = 4;
            dirState = direction;
        }
    }

    public override void AnimateAttacks()
    {
        if (abutton && attackReady)
        {
            attackReady = false;
            //dashReady = false;
            animator.SetInteger("Anim Number", animNumber);
            animator.SetInteger("Direction", dirState);
            animator.SetTrigger("Attack");
            attacked = true;
            sound.clip = clips[0];
            sound.Play();
        }

    }

    public void LetIntroPlay()
    {
        if (transform.position == transformationPosition.position)
        {
            inPosition = true;
            if (OnEnterPosition != null) OnEnterPosition.Invoke();
        }
    }

    public override void AnimateMovement()
    {
        int direction = 0;

        //lookVector
        if (movement.x < 0)
        {
            direction = 4;
        }
        else if (movement.x > 0)
        {
            direction = 3;
        }

        if (movement == Vector2.zero)
        {
            idle = true;
            moving = false;
        }
        else
        {
            idle = false;
            moving = true;
        }

        animator.SetInteger("Direction", direction);
        animator.SetBool("Idle", idle);
        animator.SetBool("Moving", moving);
    }

    public override void ResetVars()
    {
        if (attacked)
        {
            attacked = false;
            attackReady = true;
            dashReady = true;
            //stats.SetAttackPotential(-1);
        }
    }

    public override void AnimateDamage(Vector2 a)
    {
        if (!dead)
        {
            int direction = 0;
            if (a.x < 0)
            {
                direction = 3;
            }
            else if (a.x > 0)
            {
                direction = 4;
            }
            dirState = direction;
            animator.SetInteger("Direction", direction);
            animator.SetTrigger("Hit");
            rb.velocity = a * hitbox.forceOfAttack;
            sound.clip = clips[2];
            sound.Play();
        }
    }

    public override void Die()
    {
        animator.SetInteger("Direction", dirState);
        animator.SetTrigger("Dead");
        animator.SetBool("Stay Dead", true);
        sound.clip = clips[3];
        sound.Play();
    }

    void OnDrawGizmos()
    {
        detectionCenter = (Vector2)transform.position + centerOffset + animCenterOffset;
        //Color newColor;
        //newColor = new Color(242/255, 0, 0);
        Gizmos.color = Color.red;//newColor;//Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position, radius4 + radiusOffset);

        //newColor = new Color(247/255, 255/255, 0);
        Gizmos.color = Color.yellow;//newColor;//Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position, radius3 + radiusOffset);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere((Vector2)transform.position, radius2 + radiusOffset);

        //newColor = new Color(0, 153/255, 236/255);
        Gizmos.color = Color.blue;//newColor;//Color.blue;
        Gizmos.DrawWireSphere((Vector2)transform.position, radius1 + radiusOffset);
    }
}
