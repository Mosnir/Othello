using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBehaviour : StateMachineBehaviour
{
    public delegate void DelegateEndBehaviour();
    public static event DelegateEndBehaviour OnEnter = () => { };
    public static event DelegateEndBehaviour OnUpdate = () => { };
    public static event DelegateEndBehaviour OnExit = () => { };

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnEnter();
    }

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnUpdate();
    }

	//OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnExit();
    }

}
