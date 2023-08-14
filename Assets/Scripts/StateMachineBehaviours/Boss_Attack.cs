using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : StateMachineBehaviour
{
    public float stepSpeed = 5f;
    public float stepTime = 0.1f;

    Rigidbody2D rb;
    Transform player;
    int animationDirection;
    float timeTillStepEnds;
    float resetStrength;
    Vector2 resetVector;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

        timeTillStepEnds = Time.fixedTime + stepTime;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;

        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 lookVector = target - (Vector2)animator.transform.position;
        Debug.DrawLine((Vector2)animator.transform.position, (Vector2)animator.transform.position + lookVector, Color.cyan);

        // For determining the direction of the next attack
        DetermineAnimationDirection(lookVector);
        animator.SetInteger("Direction", animationDirection);

        if (Time.fixedTime < timeTillStepEnds)
        {
            //Vector2 newPos = Vector2.MoveTowards(rb.position, target, stepSpeed * Time.fixedDeltaTime);
            rb.velocity = lookVector.normalized * stepSpeed;
        }

        if (animationDirection < 3) resetStrength = 2f;
        if (animationDirection > 2) resetStrength = 1f;
        resetVector = (Vector2)animator.transform.position + (lookVector.normalized * resetStrength);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.updateMode = AnimatorUpdateMode.Normal;
    }
    
    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (rb == null) rb = animator.GetComponent<Rigidbody2D>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        rb.velocity = Vector2.zero;
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        rb.MovePosition(resetVector);
        //Debug.Log(resetVector);
    }

    void DetermineAnimationDirection(Vector2 look)
    {
        bool rangeX = false;
        bool rangeY = false;

        float xlook = look.normalized.x;
        float ylook = look.normalized.y;

        //Debug.Log(new Vector2(xlook, ylook));

        rangeX = (xlook > -0.7f) && (xlook < 0.7f); //can do 0.71 or 0.707 for more precision
        rangeY = (ylook > -0.7f) && (ylook < 0.7f);

        if (ylook > 0 && rangeX)
        {
            animationDirection = 1;
        }

        if (ylook < 0 && rangeX)
        {
            animationDirection = 2;
        }

        if (xlook > 0 && rangeY)
        {
            animationDirection = 3;
        }

        if (xlook < 0 && rangeY)
        {
            animationDirection = 4;
        }
    }
}
