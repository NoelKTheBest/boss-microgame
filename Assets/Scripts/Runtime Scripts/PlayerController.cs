using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    // I forgot that this class doesn't directly inherit from Monobehaviour
    // Considering that the class is somewhat generic, I will leave it alone

    //---Fields for movement
    public float speed;
    float moveSpeed;

    //---Fields for Attack---------------------------
    public string attackDirectionOverride = "none";
    public string directionForAttack;
    Vector2 orientationVector;
    Vector2 attackVector;
    public bool attackReady;
    bool attacked;
    public bool openWindow;
    public bool useFastAdvance;
    //public GameObject[] ammoCount;

    //---Fields for Attack Movement & Step-----------
    public string stepDirectionOverride;
    public string directionForStep;
    Vector2 stepVector;
    public float attackScalar;
    public bool isStepping;
    private bool canStep = true;
    bool stepped = false;
    public float stepTime;
    public float stepCooldown;
    public float stepSpeed;

    //---Fields for dashing--------------------------
    public string dashDirectionOverride = "none";
    public string directionForDash;
    public bool isDashing;
    private bool canDash = true;
    bool dashed = false;
    
    public float accelTime;
    public float accelAmount;
    private float dashAccel;

    public float totalDashTime;
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed;
    
    public float decelTime;
    public float decelAmount;
    private float dashDecel;

    //---Fields for projectiles----------------------
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

    //---Components----------------------------------
    public Transform firepath;
    private Rigidbody2D rb;
    private InputProcessor IP;
    private PlayerStatistics stats;
    private Hitbox hitbox;
    private PlayerHurtbox hurtbox;
    private Animator animator;
    private CircleCollider2D col;


    //---Other---------------------------------------
    public bool testingHurtbox;

    //---Direction Variables-------------------------
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

        if (dashed || stepped)
        {
            return;
        }

        if (!dashed && !attacked) rb.velocity = new Vector2(IP.movement.x, IP.movement.y) * moveSpeed;
        if (!dashed && attacked) rb.velocity = new Vector2(attackVector.x, attackVector.y) * attackScalar;

        //Debug.Log(rb.velocity);
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

            // for testing
            if (Input.GetKeyDown(KeyCode.F) && canStep)
            {
                StartCoroutine(Step());
            }

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
        //Debug.Log(attackReady);
        
        // The first attack will always execute and have attackScalar changed accordingly
        if (IP.abutton && attackReady && !testingHurtbox)
        {
            //dirState = attackDirection;
            attacked = true;
            attackReady = false;
            animator.SetInteger("Direction", attackDirection);
            animator.SetTrigger("Attack A");
            //Debug.Log(directionForAttack);
            if (directionForAttack == "look") attackVector = IP.lookV * attackScalar;
            if (directionForAttack == "move") attackVector = IP.movement * attackScalar;
            //Debug.Log(attackVector);
        }
        // The second attack won't always execute and when the animation for the attack isn't played in time
        //  this condition is still true and the regular value of 1 for the attackScalar gets used
        // There must be a small window between the combo advance part of an animation and when the animation 
        //  finally ends or ResetVars() is called, maybe.
        else if (IP.abutton && !attackReady && !testingHurtbox && openWindow)
        {
            //stats.SetAttackPotential(0);
            attackScalar = 1;
            animator.SetInteger("Direction", attackDirection);
            animator.SetBool("Fast Advance", useFastAdvance);
            animator.SetTrigger("Sub-Combo");
            animator.SetTrigger("Combo");
            //Debug.Log(directionForAttack);
            if (directionForAttack == "look") attackVector = IP.lookV * attackScalar;
            if (directionForAttack == "move") attackVector = IP.movement * attackScalar;
            //Debug.Log(attackVector);
        }
        
    }
    
    IEnumerator Step()
    {
        // test the step mechanic later, right now attacking is broken for some reason
        #region Note
        // When the player moves with the attack, i want it to feel as if they are 
        //  taking a step instead of gliding across the floor. This can be controlled
        //  in the animation, however:

        // There are couple of problem areas to think about when using animations to
        //  alter the magnitude of the movement vector.

        // Problem Area 1: Holes, or gaps in the animator that will allow code to still
        //  run without the animation altering key variables

        // Problem Area 2: The animator does not run in a fixed time loop, and as such
        //  the physics of movement in the animations will be off and not work properly
        //  when the framerate dips.

        // Problem Area 3: I have to go into each animation and manually change the
        //  value of the scalar and if I need to change that value of that scalar
        //  often, it soon becomes a huge pain.

        // I could go on, but I think it's clear that I need to switch to a more
        //  physics-based approach. So I guess I will be having a step mechanic.
        #endregion
        // Wait for FixedUpdate before updating values
        yield return new WaitForFixedUpdate();

        stepped = true;
        canStep = false;
        isStepping = true;

        //Vector2 initialMovement = Vector2.zero;
        
        rb.velocity = new Vector2(IP.lookX, IP.lookY) * stepSpeed;

        for (float duration = stepTime; duration > 0; duration -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        isStepping = false;
        stepped = false;

        for (float duration = stepCooldown; duration > 0; duration -= Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;

        canStep = true;
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
        attackReady = true;
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
        if (attacked)
        {
            attacked = false;
            attackReady = true;
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
