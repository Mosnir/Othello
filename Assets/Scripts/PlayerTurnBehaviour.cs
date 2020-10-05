using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnBehaviour : StateMachineBehaviour
{

    Othello othello;
    Animator _animator;

    public delegate void DelegatePlayerTurnBehaviour();

    public static event DelegatePlayerTurnBehaviour OnEnter = () => { };
    public static event DelegatePlayerTurnBehaviour OnExit = () => { };

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        othello = animator.GetComponent<Othello>();

        _animator = animator;
        othello.UpdatePlayables();
        //ui.Refresh(othello.rootTree);
        //ui.AddListeners();
        Settings.turn++;
        if (!othello.hasPlayables())
        {
            if (Settings.pass >= 2)
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

    private void nextStep()
    {
        if(othello.rootTree.GetOpponent() == Data.STATE.BLACK)
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

    public void Play(int id)
    {
        Vector2Int tmp = new Vector2Int(id / 8, id % 8);
        foreach (Data.Playable playable in othello.playerPlayables)
        {
            if (playable.position == tmp)
            {
                foreach (Vector2Int flip in playable.flips)
                {
                    othello.rootTree.board[flip.x, flip.y] = othello.rootTree.currentPlayer;
                }
                othello.rootTree.board[tmp.x, tmp.y] = othello.rootTree.currentPlayer;
                nextStep();
            }
        }

    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ui.RemoveListeners();
        othello.rootTree.SwitchPlayer();
        OnExit();
    }
}
