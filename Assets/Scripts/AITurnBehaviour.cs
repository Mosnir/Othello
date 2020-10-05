using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnBehaviour : StateMachineBehaviour
{

    public delegate void DelegateAITurnBehaviour();
    public static event DelegateAITurnBehaviour OnEnter = () => { };
    public static event DelegateAITurnBehaviour OnUpdate = () => { };
    public static event DelegateAITurnBehaviour OnExit = () => { };

    Othello othello;
    Animator _animator;
    bool finish = false;
    float Timer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        othello = animator.GetComponent<Othello>();
        _animator = animator;
        othello.UpdatePlayables();
        finish = false;
        Timer = Settings.WaitTimeAi;
        Settings.turn++;

        if (othello.rootTree.currentPlayer == Data.STATE.BLACK) Settings.current = Settings.player1;
        else Settings.current = Settings.player2;

        if (!othello.hasPlayables())
        {
            if(Settings.pass >= 2)
            {
                animator.SetTrigger("end");
            }
            else
            {
                Settings.pass++;
                nextStep();
            }
        }
        else
        {
            Settings.pass = 0;
        }
        OnEnter();
        
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer -= Time.deltaTime;
        if (finish)
        {
            if(Timer < 0) nextStep();
        }
        else
        {
            othello.rootTree.children.Clear();
            othello.rootTree.isOpponent = false;
            othello.rootTree.value = 0;
            othello.nbSimulation = 0;

            othello.Simulate(othello.rootTree, Settings.depth, othello.rootTree.currentPlayer);

            int value = othello.rootTree.Alphabeta();

            for (int i = 0; i < othello.rootTree.children.Count; i++)
            {
                if (othello.rootTree.children[i].value == value)
                {
                    othello.rootTree.board = othello.rootTree.children[i].board;
                    finish = true;
                    return;
                }
            }
        }
        OnUpdate();
    }

    private void nextStep()
    {
        if (othello.rootTree.GetOpponent() == Data.STATE.BLACK)
        {
            if (Settings.player1 == Settings.PLAYERTYPE.PLAYER) _animator.SetTrigger("player");
            else _animator.SetTrigger("ia");
        }
        else
        {
            if (Settings.player2 == Settings.PLAYERTYPE.PLAYER) _animator.SetTrigger("player");
            else _animator.SetTrigger("ia");
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        othello.rootTree.SwitchPlayer();
        OnExit();
    }

}
