using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlT : Character
{
    //-----------------Attack and Dash Variables
    public string directionForDash;
    public string directionForAttack;
    public string directionForAbility;
    int dCount = 0;
    bool dashed = false;
    Vector2 dashVector = Vector2.zero;
    Vector2 attackVector;
    [HideInInspector] public float attackScalar;
    [HideInInspector] public bool attackReady;
    //[HideInInspector] public bool projectileReady;
    [HideInInspector] public bool dodgeReady;
    [HideInInspector] public bool abilityReady;
    bool attacked;
    [HideInInspector] public bool openWindow;
    [HideInInspector] public bool useFastAdvance;
    //public GameObject[] ammoCount;

    ObjectPooler objectPooler;
    private float timeToCount;
    public float shootingWait;

    //this could either be the time the game waits until it starts to take away more points
    //or it could be the time that the game waits to take points away in the first place
    //-----------------Coroutine Variables
    [SerializeField] private float timeTillUpdate;
    private float timeTillUpdateCopy;
    [SerializeField] private float frameTime;
    private float frameTimeCopy;
    [SerializeField] private float currentPointCost;
    [SerializeField] private float pointCostInc;
    bool coroutineFinished;

    //-----------------Necessary Components
    private Rigidbody2D rb;
    private InputProcessor IP;
    private Stats stats;
    private Hitbox hitbox;
    public Transform firepath;
    private Animator animator;
    //private ForDashMechanic dashAttackScript;
    public GameObject upgradeSymbol;
    //public GameObject dashAttackObject;
    private CircleCollider2D col;
    private AudioSource sound;
    public AudioClip[] clips;


    //-----------------Other
    bool dead;

    //-----------------Direction Variables
    //do something about all of these direction variables
    int dirState;
    int attackDirection;
    int abilityDirection;
    int dashDirection;
    int moveDirection;
    string currentDirection;//-----------| All necessary
    string startDirection = "right";//---|
    string previousDirection;//----------V
    bool idle;
    bool moving;
    
    void Awake()
    {
        //later on i will incorporate scriptable objects so data can persist between scenes
        sound = GetComponentInChildren<AudioSource>();
        //dashAttackScript = GetComponentInChildren<ForDashMechanic>();
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
        IP = GetComponent<InputProcessor>();
        hitbox = GetComponent<Hitbox>();
        col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        timeTillUpdateCopy = timeTillUpdate;
        frameTimeCopy = frameTime;
        currentDirection = startDirection;
        previousDirection = currentDirection;
        openWindow = false;
        //dashAttackConnected = false;
        //dashAttackPosition = Vector2.zero;
        objectPooler = ObjectPooler.Instance;
        attackReady = true;
        dodgeReady = true;
        abilityReady = true;
        idle = true;
        coroutineFinished = false;
        dead = false;
        //dashAttackObject.SetActive(false);
    }

    //edit rb.velocity in this function
    void FixedUpdate()
    {
        /*  In this function, the player will default to a stationary pose. 
         *    - Whenever there is no input, the player will smoothly come to a halt.
         *          The time it will take for the player to stop will be dependent on how fast they were going.
         *      
         *    - All input will lead to the player moving a little bit.
         *          The halting process will naturally cause the player to come to a stop after this.
         *      
         *    - Attacking, running, or even being hit will cause the player to move.
         *          The rules around getting hit will work a bit differently, instead of the player trying to come to a stop,
         *          the player will be knocked back and be eventually slowed down by friction if the knockback is strong
         *          enough.
         *      
         *    - The basic idea is to code player movement as someone would move in a fight. It's here, where I can add some
         *          in-game secrets about playing better.
         *      
         *    - A neat idea could be to allow the player to step instead of always running. This could allow them to 
         *          position themselves very slightly in order to dodge with oncoming attacks or take advantage of an opening.
         */
    }

    //everything else pertaining to controlling the character on screen will be handled in Update and LateUpdate
    void Update()
    {
        /*
        if (!dashAttackScript.isAnimationFinished)
        {
            Debug.Log(dashAttackScript.isAnimationFinished);
            dashAttackObject.SetActive(false);
        }
        */

        if (!dead)
        {
            /*
            if (stats.currentHP <= 0)
            {
                if (!dead)
                {
                    Die();
                }

                dead = true;
                stats.dead = true;
            }
            */

            DetermineDirections();
            
            AnimateAttacks();

            if (IP.pbutton && timeToCount == shootingWait)
            {
                StartCoroutine("ShootProjectile");
                //ShootProjectile();
            }

            if (IP.ebutton && dodgeReady == true)
            {
                animator.SetTrigger("Dodge Stance");
                dodgeReady = false;
                attackReady = false;
            }

            //Debug.Log(stats.attackPotential);
            //IP.movement = IP.movement * stats.spdInc;
            if (!dashed && !hitbox.wasHit && !attacked) rb.velocity = new Vector2(IP.movement.x * IP.multiValH, 
                IP.movement.y * IP.multiValV) * attackScalar * Time.timeScale;

            //might delete
            if (!dashed && !hitbox.wasHit && attacked) rb.velocity = new Vector2(attackVector.x * IP.multiValH,
                attackVector.y * IP.multiValV) * attackScalar * Time.timeScale;

            if (!dashed && !attacked && !hitbox.wasHit)
            {
                AnimateMovement();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        //Debug.Log(dashReady);
    }

    //might need to go into input manager
    public override void DetermineDirections()
    {
        
        //firepath.right = (IP.tempV - transform.position) + transform.position;
        
        ///Determine Attack Direction based on either the current look direction or movement direction
        bool rangeX = false;
        bool rangeY = false;

        if (directionForAttack == "look")
        {
            rangeX = (IP.lookX > -0.7f) && (IP.lookX < 0.7f); //can do 0.71 or 0.707 for more precision
            rangeY = (IP.lookY > -0.7f) && (IP.lookY < 0.7f);
        }
        else if (directionForAttack == "move")
        {
            rangeX = (IP.moveH > -0.7f) && (IP.moveH < 0.7f);
            rangeY = (IP.moveV > -0.7f) && (IP.moveV < 0.7f);
        }

        if (IP.lookY > 0 && rangeX)
        {
            //direction = 1;
            attackDirection = 1;
        }

        if (IP.lookY < 0 && rangeX)
        {
            //direction = 2;
            attackDirection = 2;
        }

        if (IP.lookX > 0 && rangeY)
        {
            //direction = 3;
            attackDirection = 3;
        }

        if (IP.lookX < 0 && rangeY)
        {
            //direction = 4;
            attackDirection = 4;
        }

        //might have to put back in the dash function if the results of moving it here are proving to be harmful
        ///Determine Dash Direction based on either the current look direction or movement direction
        

        ///Determine Dash Direction based on either the current look direction or movement direction
        if (directionForAbility == "look")
        {
            if (IP.lookX > 0)
            {
                abilityDirection = 3;
            }
            else if (IP.lookX < 0)
            {
                abilityDirection = 4;
            }
            else if (IP.lookX == 0)
            {
                if (IP.lookY < 0)
                {
                    dashDirection = 4;
                }
                else if (IP.lookY > 0)
                {
                    dashDirection = 3;
                }
            }
        }
        else if (directionForAbility == "move")
        {
            if (IP.movement.x > 0)
            {
                abilityDirection = 3;
            }
            else if (IP.movement.x < 0)
            {
                abilityDirection = 4;
            }
            else if (IP.movement.x == 0)
            {
                if (IP.movement.y < 0)
                {
                    dashDirection = 4;
                }
                else if (IP.movement.y > 0)
                {
                    dashDirection = 3;
                }
            }
        }
    }

    //Should go into input manager
    public override void AnimateAttacks()
    {
        
        if (IP.abutton && attackReady)
        {
            //dirState = attackDirection;
            attacked = true;
            attackReady = false;
            dodgeReady = false;
            animator.SetInteger("Direction", attackDirection);
            animator.SetTrigger("Attack A");
            if (directionForAttack == "look") attackVector = IP.lookV * attackScalar;
            if (directionForAttack == "move") attackVector = IP.movement * attackScalar;
            Debug.Log(attackScalar);
            sound.clip = clips[0];
            sound.Play();
        }
        else if (IP.abutton && !attackReady && openWindow)
        {
            //stats.SetAttackPotential(0);
            attackScalar = 1;
            animator.SetInteger("Direction", attackDirection);
            animator.SetBool("Fast Advance", useFastAdvance);
            animator.SetTrigger("Sub-Combo");
            animator.SetTrigger("Combo");
            if (directionForAttack == "look") attackVector = IP.lookV * attackScalar;
            if (directionForAttack == "move") attackVector = IP.movement * attackScalar;
            Debug.Log(attackScalar);
            sound.clip = clips[0];
            sound.Play();
        }
        
    }
    
    //might also need to go into input manager
    IEnumerator ShootProjectile()
    {

        /*
        if (IP.abuttonC && attackReady)
        {
            attackReady = false;
            dashReady = false;
            dodgeReady = false;
            animator.SetTrigger("Attack C");
            //animator.SetInteger("Direction", dirState); //use this when you have attack c animation ready
        }
        */

        animator.SetTrigger("Shoot");

        //firepath.right = (((Vector3)lookVector + firepath.position) - transform.position) + transform.position;
        firepath.right = ((Vector3)IP.lookV - transform.position) + transform.position;
        
        //sound.clip = clips[1];
        //sound.Play();

        GameObject ammoInstance;
        ammoInstance = objectPooler.SpawnFromPool("Tissy's Blade", firepath.position, firepath.right, firepath.rotation);
        //ammoInstance = objectPooler.GetPooledObject();

        //projectileReady = false;
        timeToCount = shootingWait;

        while (timeToCount > 0)
        {
            timeToCount -= Time.deltaTime;

            yield return null;
        }

        //projectileReady = true;
    }

    //this can stay in the character controller
    public override void AnimateMovement()
    {
        int direction = 0;
        
        if (IP.moveH < 0)
        {

            if (currentDirection == "none")
            {
                previousDirection = "left";
            }

            currentDirection = "left";
            direction = 4;
            moving = true;
            idle = false;
        }
        else if (IP.moveH > 0)
        {
            if (currentDirection == "none")
            {
                previousDirection = "right";
            }

            currentDirection = "right";
            direction = 3;
            moving = true;
            idle = false;
        }

        if (IP.moveV < 0)
        {
            if (currentDirection == "none")
            {
                previousDirection = "down";
            }

            currentDirection = "down";
            direction = 2;
            moving = true;
            idle = false;
        }
        else if (IP.moveV > 0)
        {
            if (currentDirection == "none")
            {
                previousDirection = "up";
            }

            currentDirection = "up";
            direction = 1;
            moving = true;
            idle = false;
        }

        if (currentDirection == "up" && IP.moveH == 0)
        {
            previousDirection = "up";
            direction = 1;
            moveDirection = direction;
            moving = true;
            idle = false;
        }

        if (currentDirection == "down" && IP.moveH == 0)
        {
            previousDirection = "down";
            direction = 2;
            moveDirection = direction;
            moving = true;
            idle = false;
        }

        if (currentDirection == "right" && IP.moveV == 0)
        {
            previousDirection = "right";
            direction = 3;
            moveDirection = direction;
            moving = true;
            idle = false;
        }

        if (currentDirection == "left" && IP.moveV == 0)
        {
            previousDirection = "left";
            direction = 4;
            moveDirection = direction;
            moving = true;
            idle = false;
        }

        if (IP.hDir == 0 && IP.vDir == 0)
        {
            currentDirection = "none";
            idle = true;
            moving = false;
        }

        //if ()
        //Debug.Log(moveDirection + " : " + previousDirection);

        animator.SetInteger("Direction", moveDirection);
        animator.SetBool("Idle", idle);
        animator.SetBool("Moving", moving);
    }
    
    //maybe reset direction var here using dirstate?
    public override void ResetVars()
    {
        //make sure that resetvars is only called at the end of animations or at the beginning
        //dashAttackObject.transform.position = Vector2.zero;
        if (attacked || dodgeReady == false)
        {
            attacked = false;
            attackReady = true;
            dodgeReady = true;
            abilityReady = true;
            //dashAttackConnected = false;
        }
    }

    public override void AnimateDamage(Vector2 a)
    {
        if (!dead)
        {
            //for later:
            //setup dash to not be available when getting hit
            int direction = 0;
            if (a.x < 0)
            {
                direction = 3;
            }
            else if (a.x > 0)
            {
                direction = 4;
            }
            moveDirection = direction;
            animator.SetInteger("Direction", direction);
            animator.SetTrigger("Hit");
            rb.velocity = a * hitbox.forceOfAttack;
            //sound.clip = clips[2];
            //sound.Play();
        }
    }

    public override void Die()
    {
        animator.SetInteger("Direction", dirState);//might change dirState
        animator.SetTrigger("Dead");
        animator.SetBool("Stay Dead", true);
        //sound.clip = clips[3];
        //sound.Play();
    }
}
