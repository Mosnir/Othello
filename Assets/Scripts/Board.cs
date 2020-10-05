using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public Othello othello;
    public GameObject board;
    public GameObject boardPion;
    public GameObject parent;
    private List<GameObject> pions;

    // Use this for initialization
    void Start()
    {

        pions = new List<GameObject>();

        PlayerTurnBehaviour.OnEnter += Refresh;
        AITurnBehaviour.OnEnter += Refresh;
        EndBehaviour.OnEnter += Refresh;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tmpBoard = Instantiate(board , parent.transform);
                tmpBoard.transform.localPosition = new Vector3(j * -1 + 4, 0, i * -1 + 3.5f);
                GameObject b = Instantiate(boardPion, parent.transform);
                b.transform.localPosition = new Vector3(j * -1 + 3.5f, 0, i * -1 + 3.5f);
                b.GetComponent<Pion>().id = i * 8 + j;
                b.GetComponent<Pion>().othello = othello;
                pions.Add(b);
            }
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            foreach(RaycastHit hit in Physics.RaycastAll(ray))
            {
               Pion pion = hit.collider.GetComponent<Pion>();
                if(pion)
                {
                    pion.Play();
                }
            }
        }
    }

    void OnDestroy()
    {
        PlayerTurnBehaviour.OnEnter -= Refresh;
        AITurnBehaviour.OnEnter -= Refresh;
        EndBehaviour.OnEnter -= Refresh;
    }

    public void Refresh()
    {
        Data data = othello.rootTree;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (data.board[i, j] == Data.STATE.WHITE)
                {
                    pions[i * 8 + j].SetActive(true);
                    pions[i * 8 + j].transform.Find("Pivot").localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
                    pions[i * 8 + j].GetComponentInChildren<TextMesh>().color = new Color(0, 0, 0, 0);
                }
                if (data.board[i, j] == Data.STATE.BLACK)
                {
                    pions[i * 8 + j].SetActive(true);
                    pions[i * 8 + j].transform.Find("Pivot").localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    pions[i * 8 + j].GetComponentInChildren<TextMesh>().color = new Color(0, 0, 0, 0);
                }
                if (data.board[i, j] == Data.STATE.EMPTY) pions[i * 8 + j].SetActive(false);
            }
        }
        foreach (Data.Playable playable in data.GetPlayables())
        {
            if(data.currentPlayer == Data.STATE.BLACK)
            {
                pions[playable.position.x * 8 + playable.position.y].GetComponentInChildren<TextMesh>().color = Color.white;
                pions[playable.position.x * 8 + playable.position.y].transform.Find("Pivot").localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                pions[playable.position.x * 8 + playable.position.y].GetComponentInChildren<TextMesh>().color = Color.black;
                pions[playable.position.x * 8 + playable.position.y].transform.Find("Pivot").localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
            }
            pions[playable.position.x * 8 + playable.position.y].SetActive(true);
            pions[playable.position.x * 8 + playable.position.y].GetComponentInChildren<TextMesh>().text = playable.flips.Count.ToString();
        }
    }

}

