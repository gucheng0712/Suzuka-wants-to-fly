using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Shoot : StateMachineBehaviour
{
    Player playerScript;

    void Awake()
    {
        playerScript = FindObjectOfType<Player>();
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Shoot Layer"), 1.0f);
        playerScript.isShooting = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetLayerWeight(animator.GetLayerIndex("Melee Layer"),
        //Mathf.Lerp(animator.GetLayerWeight(animator.GetLayerIndex("Shoot Layer")), 0, 0.1f));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
