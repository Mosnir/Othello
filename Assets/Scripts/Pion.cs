using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pion : MonoBehaviour
{

    public int id;
    private PlayerTurnBehaviour ptb;
    public Othello othello;

    public void Start()
    {
        ptb = othello.GetPlayerTurnBehavior();
    }

    public void Play()
    {
        ptb.Play(id);
    }
}
