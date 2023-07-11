using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : StateMachineBehaviour
{
    Rigidbody2D rb;
    Transform player;
    int animationDirection;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;

        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 lookVector = target - (Vector2)animator.transform.position;
        Debug.DrawLine((Vector2)animator.transform.position, (Vector2)animator.transform.position + lookVector, Color.cyan);

        DetermineAnimationDirection(lookVector);
        Debug.Log(animationDirection);
        animator.SetInteger("Direction", animationDirection);
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

    void Step()
    {
        // Implement Step Mechanic
        // Think about how we'll implement it
    }
}
