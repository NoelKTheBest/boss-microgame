using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlT : MonoBehaviour
{
    // I forgot that this class doesn't directly inherit from Monobehaviour
    // Considering that the class is somewhat generic, I will leave it alone

    //---Fields for movement
    [Header("Fields for movement")]
    public float speed;
    float moveSpeed;

    //-----------------Attack Variables
    [Header("Attack Variables")]
    [Tooltip("This value lets you override the input for the direction of the attack for testing purposes")]
    public string attackDirectionOverride = "none";
    public string directionForAttack;
    Vector2 orientationVector;
    Vector2 attackVector;
    [HideInInspector] public float attackScalar;
    [HideInInspector] public bool attackReady;
    bool attacked;
    [HideInInspector] public bool openWindow;
    [HideInInspector] public bool useFastAdvance;
    //public GameObject[] ammoCount;

    //---Fields for dashing
    [Header("Dash Variables")]
    [Tooltip("This value lets you override the input for the direction of the dash for testing purposes")]
    public string dashDirectionOverride = "none";
    public string directionForDash;
    public bool isDashing;
    private bool canDash = true;
    bool dashed = false;

    [Header("   Acceleration")]
    public float accelTime;
    [Tooltip("The amount that acceleration increments")]
    public float accelAmount;
    //[Tooltip("The current acceleration speed")]
    private float dashAccel;

    [Header("   Main Dash")]
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed;

    [Header("   Deceleration")]
    public float decelTime;
    [Tooltip("The amount that deceleration decrements")]
    public float decelAmount;
    //[Tooltip("The current deceleration speed")]
    private float dashDecel;

    //---Fields for projectiles
    [Header("Projectile Variables")]
    public float shootingWait;
    ObjectPooler objectPooler;
    private float timeToCount;
    //[HideInInspector] public bool projectileReady;

    //this could either be the time the game waits until it starts to take away more points
    //or it could be the time that the game waits to take points away in the first place
    //-----------------Coroutine Variables
    //[SerializeField] private float timeTillUpdate;
    //private float timeTillUpdateCopy;
    //[SerializeField] private float frameTime;
    //private float frameTimeCopy;
    //bool coroutineFinished;

    //-----------------Necessary Components
    [Header("Components")]
    public Transform firepath;
    private Rigidbody2D rb;
    private InputProcessor IP;
    private PlayerStatistics stats;
    private Hitbox hitbox;
    private PlayerHurtbox hurtbox;
    private Animator animator;
    private CircleCollider2D col;


    //-----------------Other
    [Header("Other")]
    public bool testingHurtbox;

    //-----------------Direction Variables
    //do something about all of these direction variables
    int dirState; // review this variable
    int attackDirection;
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
        //sound = GetComponentInChildren<AudioSource>();
        //dashAttackScript = GetComponentInChildren<ForDashMechanic>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStatistics>();
        rb = GetComponent<Rigidbody2D>();
        IP = GetComponent<InputProcessor>();
        hitbox = GetComponent<Hitbox>();
        col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        moveSpeed = speed;
        //timeTillUpdateCopy = timeTillUpdate;
        //frameTimeCopy = frameTime;
        currentDirection = startDirection;
        previousDirection = currentDirection;
        openWindow = false;
        //dashAttackConnected = false;
        //dashAttackPosition = Vector2.zero;
        objectPooler = ObjectPooler.Instance;
        attackReady = true;
        //dodgeReady = true;
        //abilityReady = true;
        idle = true;
        //coroutineFinished = false;
        //dashAttackObject.SetActive(false);
    }

    //edit rb.velocity in this function
    void FixedUpdate()
    {
        // IP.movement and IP.lookV are two different kinds of direction
        //  vectors. Meanwhile, attackVector is only used for giving the player some
        //  movement when attacking so that they aren't just standing still.
        //  It's especially helpful when there is knockback in the game and you
        //  need to move with the enemy. However, normal movement stops when
        //  you attack. Attacking is based on either IP.lookV when using mouse
        //  and keyboard or it's based on IP.movement when using controller.
        //  I will allow for the player to change these to their liking, but as
        //  the default option, that will be how attacks work.
        // The rest of the movement vector could be applied, but we really don't
        //  need to do that. We can just use multiple if statements to determine
        //  which vector rb.velocity should use. Like in the Update() function.
        // Knockback vector will be similar the same as the attack vector or it
        //  could be the cross product between the movement and attack vectors.
        //  I'll have to do testing and think about how well cross product would
        //  work for my game if I decide to use it.
        // Now that I think about it, we really don't need to have a lot of knockback
        //  at all. We could use light knockback on regular hits and heavy knockback
        //  on combo finishers and heavy attacks.
        // If I had no knockback, would we need to have a stepping mechanic?
        //  I now want to have knockback in specific places, which should be easy enough
        //  to plan for. So stepping would be for following up on an attack after
        //  knockback has pushed them away, or for catching up to an enemy that
        //  moved after an attack, or maybe even moving to another side of the boss.


        //Debug.Log("dashed: " + dashed + ", attacked: " + attacked);
        //Debug.Log(rb.velocity);

        if (dashed)
        {
            return;
        }

        if (!dashed && !attacked) rb.velocity = new Vector2(IP.movement.x, IP.movement.y) * moveSpeed;
        if (!dashed && attacked) rb.velocity = new Vector2(IP.movement.x, IP.movement.y) * attackVector;
    }

    //everything else pertaining to controlling the character on screen will be handled in Update and LateUpdate
    void Update()
    {
        if (!stats.dead)
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

            // include if statement here that changes direction for attacking based
            //  on the input device used
            // if (IP.controller == 0) directionForAttack = "look"
            // else if (IP.controller == 1) directionForAttack = "move"
            
            DetermineLookDirection();
            
            AnimateAttacks();

            if (IP.pbutton && timeToCount == shootingWait)
            {
                StartCoroutine(ShootProjectile());
            }

            if (IP.dbutton && canDash)
            {
                StartCoroutine(Dash());
                //animator.SetTrigger("Dash");
                attackReady = false;
            }

            // now in fixedupdate
            // if we didn't dash, we were not hit, and we didn't attack somebody then we move normally
            //if (!dashed && !hitbox.wasHit && !attacked) rb.velocity = new Vector2(IP.movement.x * IP.multiValH, 
            //  IP.movement.y * IP.multiValV) * attackScalar * Time.timeScale;

            //might delete
            //if (!dashed && !hitbox.wasHit && attacked) rb.velocity = new Vector2(attackVector.x * IP.multiValH,
            //  attackVector.y * IP.multiValV) * attackScalar * Time.timeScale;

            //Debug.Log(testingHurtbox);

            if (!dashed && !attacked && !hitbox.wasHit)
            {
                AnimateMovement();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    //might need to go into input manager
    // DO NOT CHANGE THIS FUNCTION !!
    public void DetermineLookDirection()
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
        

        /*Determine Dash Direction based on either the current look direction or movement direction
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
        */
    }
    
    public void AnimateAttacks()
    {
        // I want to put this into the InputProcessor.cs, but
        //  I don't really need to have multiple references to
        //  the animator. Also, setting up combos with the animator
        //  is so much more convinient, and intuitive because you
        //  can get a sense of exactly when and where the combo
        //  will advance in a particular animation.

        // Besides, combos in this game are not advanced. So there
        //  is no need for a fancy combo system. AND, this method
        //  overrides an abstract method from a class that
        //  InputProcessor.cs doesn't inherit from.

        if (IP.abutton && attackReady && !testingHurtbox)
        {
            //dirState = attackDirection;
            attacked = true;
            attackReady = false;
            //dodgeReady = false;
            animator.SetInteger("Direction", attackDirection);
            animator.SetTrigger("Attack A");
            if (directionForAttack == "look") attackVector = IP.lookV * attackScalar;
            if (directionForAttack == "move") attackVector = IP.movement * attackScalar;
            //Debug.Log(attackScalar);
        }
        else if (IP.abutton && !attackReady && !testingHurtbox && openWindow)
        {
            //stats.SetAttackPotential(0);
            attackScalar = 1;
            animator.SetInteger("Direction", attackDirection);
            animator.SetBool("Fast Advance", useFastAdvance);
            animator.SetTrigger("Sub-Combo");
            animator.SetTrigger("Combo");
            if (directionForAttack == "look") attackVector = IP.lookV * attackScalar;
            if (directionForAttack == "move") attackVector = IP.movement * attackScalar;
            //Debug.Log(attackScalar);
        }
        
    }
    
    IEnumerator Dash()
    {
        // test the mechanics first, then add the animations
        
        // Wait for FixedUpdate before updating values
        yield return new WaitForFixedUpdate();

        // Setup
        dashed = true;
        canDash = false;
        isDashing = true; //not sure which one to use
        
        // set the initial acceleration and deceleration for the dash
        dashAccel = moveSpeed;
        dashDecel = dashSpeed;

        Vector2 initialMovement = Vector2.zero;
        if (IP.movement != Vector2.zero) initialMovement = IP.movement;
        if (IP.movement == Vector2.zero) initialMovement = orientationVector;

        //-------------------------------------------------------------------------------//

        // Wait for dashAccel to build up
        #region Note
        // WaitUntil and WaitWhile can't be used because it relies on a value outside of the coroutine
        //  Because the coroutine pauses its own execution until a certain condition is met,
        //  the coroutine will wait forever because it can't update it's own values when its execution
        //  is paused.

        // Saving this note for later, just in case I forget why I'm doing this.
        #endregion
        //yield return new WaitForSeconds(accelTime);
        for (float duration = accelTime; duration > 0; duration -= Time.fixedDeltaTime)
        {
            dashAccel += accelAmount;

            // Dash will accelerate until hitting the dash speed
            dashAccel = Mathf.Clamp(dashAccel, moveSpeed, dashSpeed);
            rb.velocity = new Vector2(initialMovement.x, initialMovement.y) * dashAccel;
            yield return new WaitForFixedUpdate();
        }

        //-------------------------------------------------------------------------------//

        // Main dash starts
        //yield return new WaitForFixedUpdate();
        rb.velocity = new Vector2(initialMovement.x, initialMovement.y) * dashSpeed;

        // Main dash stops
        //yield return new WaitForSeconds(dashTime);
        for (float duration = dashTime; duration > 0; duration -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        //-------------------------------------------------------------------------------//

        // Dash starts decelerating
        //yield return new WaitForSeconds(decelTime);
        for (float duration = decelTime; duration > 0; duration -= Time.fixedDeltaTime)
        {
            dashDecel -= decelAmount;

            // Dash will decelerate until hitting the base movement speed
            dashDecel = Mathf.Clamp(dashDecel, moveSpeed, dashSpeed);
            rb.velocity = new Vector2(initialMovement.x, initialMovement.y) * dashDecel;
            yield return new WaitForFixedUpdate();
        }

        //-------------------------------------------------------------------------------//

        // Player is no longer dashing however, they should still be able to move
        isDashing = false;
        dashed = false;

        // I force the player to wait for a cooldown period before they can dash again
        //  It won't be really long
        //yield return new WaitForSeconds(dashCooldown);
        for (float duration = dashCooldown; duration > 0; duration -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        // The player can dash again
        canDash = true;
    }
    
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
    
    public void AnimateMovement()
    {
        // review this script

        /*
         * one bug I've found is that when moving in two directions and you swtich one of the directions 
         *  you are moving and being animated in quickly enough that the animator either can't keep up or
         *  there isn't enough logic to help the cpu decide how to animate the character in those situations
         */
        
        int direction = 0;
        
        // The first 4 if statements determine the current direction

        // both of these if statements determine the current horizontal direction
        if (IP.moveH < 0)
        {
            // set previous direction is player was idle
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
            // set previous direction is player was idle
            if (currentDirection == "none")
            {
                previousDirection = "right";
            }

            currentDirection = "right";
            direction = 3;
            moving = true;
            idle = false;
        }

        // both of these if statements determine the current vertical direction
        if (IP.moveV < 0)
        {
            // set previous direction is player was idle
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
            // set previous direction is player was idle
            if (currentDirection == "none")
            {
                previousDirection = "up";
            }

            currentDirection = "up";
            direction = 1;
            moving = true;
            idle = false;
        }

        // These next 4 if statements determine the previous direction
        
        // Based on the logic, nothing changes when the player isn't moving
        //  in a direction that can change their orientation.
        // These statements allow for the previous direction to change when
        //  the player is no longer moving in a direction congruent with or
        //  opposite to the direction they started in.
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

        // Reset variables when player stops moving
        if (IP.moveH == 0 && IP.moveV == 0)
        {
            currentDirection = "none";
            idle = true;
            moving = false;
        }

        switch (direction)
        {
            // up
            case 1:
                orientationVector = Vector2.up;
                break;
            // down
            case 2:
                orientationVector = Vector2.down;
                break;
            // right
            case 3:
                orientationVector = Vector2.right;
                break;
            // left
            case 4:
                orientationVector = Vector2.left;
                break;
        }

        //Debug.Log("facing direction: " + orientationVector);
        //Debug.Log(moveDirection + " : " + previousDirection);

        animator.SetInteger("Direction", moveDirection);
        animator.SetBool("Idle", idle);
        animator.SetBool("Moving", moving);
    }
    
    //maybe reset direction var here using dirstate?
    public void ResetVars()
    {
        //make sure that resetvars is only called at the end of animations or at the beginning
        //dashAttackObject.transform.position = Vector2.zero;
        if (attacked)// || dodgeReady == false)
        {
            attacked = false;
            attackReady = true;
            //dodgeReady = true;
            //dashAttackConnected = false;
        }
    }

    public void AnimateDamage(Vector2 a)
    {
        if (!stats.dead)
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
            rb.velocity = a * stats.force;
            //sound.clip = clips[2];
            //sound.Play();
        }
    }

    public void Die()
    {
        animator.SetInteger("Direction", dirState);//might change dirState
        animator.SetTrigger("Dead");
        animator.SetBool("Stay Dead", true);
        //sound.clip = clips[3];
        //sound.Play();
    }

    #region Example Code
    // Example FixedTimeCoroutineThing
    /*
    IEnumerator FixedTimeCoroutineThing()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        // This while loop is not needed in this script
        while (true)
        {

            // Your per-update logic goes here

            // This for loop is equivalent to WaitForSeconds
            // but uses fixed/physics timestep instead of
            // the variable one used for non-essential/loose
            // game elements and graphics.
            for (float duration = 1f; duration > 0; duration -= Time.fixedDeltaTime)
            {
                // this yield statement can be swapped out for
                //  yield return new WaitForFixedUpdate();
                // this statement 
                yield return waitForFixedUpdate;
            }
        }
    }
    */
    #endregion
}
