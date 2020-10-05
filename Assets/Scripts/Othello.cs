using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Othello : MonoBehaviour
{

    public Data rootTree;

    public Animator animator;

    public int nbSimulation = 0;

    public List<Data.Playable> playerPlayables;
    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        rootTree = new Data();
    }

    public void UpdatePlayables()
    {
        playerPlayables = rootTree.GetPlayables();
    }

    public bool hasPlayables()
    {
        return playerPlayables.Count > 0;
    }

    public PlayerTurnBehaviour GetPlayerTurnBehavior()
    {
        return animator.GetBehaviour<PlayerTurnBehaviour>();
    }

    public AITurnBehaviour GetAiTurnBehavior()
    {
        return animator.GetBehaviour<AITurnBehaviour>();
    }

    public EndBehaviour GetEndBehavior()
    {
        return animator.GetBehaviour<EndBehaviour>();
    }

    public void Simulate(Data data, int depth, Data.STATE currentPlayer)
    {
        nbSimulation++;

        foreach (Data.Playable playable in data.GetPlayables())
        {
            Data tmpData = new Data();
            Array.Copy(data.board, tmpData.board, 64);

            tmpData.isOpponent = !data.isOpponent;
            tmpData.currentPlayer = data.GetOpponent();

            tmpData.board[playable.position.x, playable.position.y] = data.currentPlayer;

            foreach (Vector2Int flip in playable.flips)
            {
                tmpData.board[flip.x, flip.y] = data.currentPlayer;
            }

            if (depth>0)
            {
                Simulate(tmpData, depth-1, currentPlayer);
                data.children.Add(tmpData);
            }
        }
    }
}