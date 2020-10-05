using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum PLAYERTYPE { PLAYER, AI1, AI2, AI3 };

    public static PLAYERTYPE player1 = PLAYERTYPE.PLAYER;
    public static PLAYERTYPE player2 = PLAYERTYPE.PLAYER;
    public static PLAYERTYPE current = PLAYERTYPE.PLAYER;
    public static int depth = 2;
    public static int pass = 0;
    public static int turn = 0;
    public static float WaitTimeAi = 0.1f;
}
