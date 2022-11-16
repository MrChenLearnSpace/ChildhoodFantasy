using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FSMOnEvent : StateMachineBehaviour
{

    public string[] onEnterMsg;
    public string[] onUpdateMsg;
    public string[] onExitMsg;
    ActorController ac;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(ac == null) {
            ac = animator.transform.parent.parent.GetComponent<ActorController>();
        }
        for (int i = 0; i < onEnterMsg.Length; i++) {
            ac.OnFSMEventNotify(onEnterMsg[i]);
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (ac == null) {
            ac = animator.transform.parent.parent.GetComponent<ActorController>();
        }
        for (int i = 0; i < onUpdateMsg.Length; i++) {
            ac.OnFSMEventNotify(onUpdateMsg[i]);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (ac == null) {
            ac = animator.transform.parent.parent.GetComponent<ActorController>();
        }
        for (int i = 0; i < onExitMsg.Length; i++) {
            
            ac.OnFSMEventNotify(onExitMsg[i]);
            //ac.OnFSMevent(onExitMsg[i]);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
