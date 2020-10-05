using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private PlayerTurnBehaviour ptb;
    public Othello othello;
    public RawImage board;
    public Button button;
    public GameObject buttonParent;
    public List<Button> buttons;

    // Use this for initialization
    void Start()
    {

        ptb = othello.GetPlayerTurnBehavior();

        PlayerTurnBehaviour.OnEnter += Refresh;
        PlayerTurnBehaviour.OnEnter += AddListeners;
        PlayerTurnBehaviour.OnExit += RemoveListeners;
        AITurnBehaviour.OnEnter += Refresh;
        EndBehaviour.OnEnter += Refresh;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {

                RawImage tmpBoard = Instantiate(board, buttonParent.transform);
                tmpBoard.transform.localPosition = new Vector2(j * -100 + 350, i * -100 + 350);

                Button b = Instantiate(button, buttonParent.transform).GetComponent<Button>();
                b.transform.localPosition = new Vector2(j * -100+ 350, i * -100 +350);
                buttons.Add(b);
                b.GetComponent<Image>().color = Color.green;
            }
        }
    }

    void OnDestroy()
    {
        PlayerTurnBehaviour.OnEnter -= Refresh;
        PlayerTurnBehaviour.OnEnter -= AddListeners;
        PlayerTurnBehaviour.OnExit -= RemoveListeners;
        AITurnBehaviour.OnEnter -= Refresh;
        EndBehaviour.OnEnter -= Refresh;
    }

    public void AddListeners()
    {
        int i = 0;
        foreach (Button b in buttons)
        {
            int _i = i++;
            b.onClick.AddListener(() => ptb.Play(_i));
        }
    }

    public void RemoveListeners()
    {
        foreach (Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
        }
    }

    public void Refresh()
    {
        Data data = othello.rootTree;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (data.board[i, j] == Data.STATE.WHITE) buttons[i * 8 + j].GetComponent<Image>().color = Color.white;
                if (data.board[i, j] == Data.STATE.BLACK) buttons[i * 8 + j].GetComponent<Image>().color = Color.black;
                if (data.board[i, j] == Data.STATE.EMPTY) buttons[i * 8 + j].GetComponent<Image>().color = new Color(0,0,0,0);
                buttons[i * 8 + j].GetComponentInChildren<Text>().text = "";
            }
        }
        foreach (Data.Playable playable in data.GetPlayables())
        {
            buttons[playable.position.x * 8 + playable.position.y].GetComponent<Image>().color = Color.yellow;
            buttons[playable.position.x * 8 + playable.position.y].GetComponentInChildren<Text>().text = playable.flips.Count.ToString();
        }
    }

}

