using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{

    public Othello othello;
    public Text turn;
    public Text winner;
    public Button btn;

    // Use this for initialization
    void Start ()
    {
        PlayerTurnBehaviour.OnEnter += Refresh;
        AITurnBehaviour.OnEnter += Refresh;
        EndBehaviour.OnEnter += ShowEnd;
        btn.onClick.AddListener(GoToMenu);
    }
	
    void OnDestroy()
    {
        PlayerTurnBehaviour.OnEnter -= Refresh;
        AITurnBehaviour.OnEnter -= Refresh;
        EndBehaviour.OnEnter -= ShowEnd;
    }

	// Update is called once per frame
	void Refresh ()
    {
        turn.text = "turn " + Settings.turn + " ( " + othello.rootTree.currentPlayer + " )";
    }

    void ShowEnd()
    {
        int result = othello.rootTree.TrueEvaluate();
        if (result > 0)
        {
            winner.text = othello.rootTree.currentPlayer + " win by " + result + " points";
        }
        else if(result < 0)
        {
            winner.text = (Data.STATE)(-(int)othello.rootTree.currentPlayer) +" win by " + -result + " points";
        }
        else
        {
            winner.text = "Draw";
        }
        
        winner.gameObject.SetActive(true);
        btn.gameObject.SetActive(true);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
