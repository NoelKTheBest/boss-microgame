using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    public float speed = 7;
    int animationDirection;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        #region Note
        // Because I was directly causing changes in the animator's state machine different functions 
        //  were being called than what I had expected. It turns out that OnStateUpdate and
        //  OnStateMachineEnter have been fighting with each other the whole time and that's why I
        //  got glitchy movement where the animator seemed like couldn't decide which direction it
        //  should animate in. Each function was working against each other. And the reason why 
        //  OnStateMachineEnter is still getting called is because we are transitioning out of it
        //  in order to go into a new directional state.
        #endregion
        
        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 lookVector = target - (Vector2)animator.transform.position;
        Debug.DrawLine((Vector2)animator.transform.position, (Vector2)animator.transform.position + lookVector, Color.red);
        
        DetermineAnimationDirection(lookVector);
        animator.SetInteger("Direction", animationDirection);

        if (animationDirection == 1)
        {
            animator.transform.position = new Vector3(animator.transform.position.x, 
                animator.transform.position.y, player.position.z - 1);
        }
        else if (animationDirection == 2)
        {
            animator.transform.position = new Vector3(animator.transform.position.x,
                animator.transform.position.y, player.position.z + 1);
        }

        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.updateMode = AnimatorUpdateMode.Normal;
    }

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        Vector2 target = new Vector2(player.position.x, player.position.y);
        Vector2 lookVector = target - (Vector2)animator.transform.position;

        DetermineAnimationDirection(lookVector);
        animator.SetBool("Moving", true);
        animator.SetInteger("Direction", animationDirection);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        
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
