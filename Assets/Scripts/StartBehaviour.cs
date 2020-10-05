using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBehaviour : StateMachineBehaviour
{
    Othello othello;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        othello = animator.GetComponent<Othello>();
        othello.rootTree.currentPlayer = Data.STATE.BLACK;
        if (Settings.player1 == Settings.PLAYERTYPE.PLAYER) animator.SetTrigger("player");
        else animator.SetTrigger("ia");
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
	
	}
}