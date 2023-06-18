using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehavior : StateMachineBehaviour
{
    //public string code = "i work?";
    MonoBehaviour mb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // I plan to use AnimatePhysics update mode instead of only using Time.fixedDeltaTime
        //  I'm not sure whether I'm able to change the update mode from this script.
        //animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        //Input.GetButtonDown("Fire1");
        mb.StartCoroutine(TestCoroutine());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If we transition to other states we will need to know the direction to play the animation in
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    IEnumerator TestCoroutine()
    {
        Debug.Log("hello");
        yield return null;
    }
}
