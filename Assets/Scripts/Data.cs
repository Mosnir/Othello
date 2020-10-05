using UnityEngine;
using System.Collections.Generic;

public class Data
{
    public enum STATE { WHITE=-1, EMPTY, BLACK };

    public int value = 0;

    public STATE IA = STATE.BLACK;

    public STATE currentPlayer = STATE.BLACK;

    public bool isOpponent = false;

    public struct Playable
    {
        public Vector2Int position;
        public List<Vector2Int> flips;
    }

    public STATE[,] board = {   
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY },
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY },
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY },
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.WHITE, STATE.BLACK, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY },
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.BLACK, STATE.WHITE, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY },
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY },
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY },
        { STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY, STATE.EMPTY,STATE.EMPTY, STATE.EMPTY }
    };

    public int[,] weight = {
        { 10, 5, 5, 5, 5, 5, 5, 10 },
        { 5,1,1,1,1,1,1,5 },
        { 5,1,1,1,1,1,1,5 },
        { 5,1,1,1,1,1,1,5 }, 
        { 5,1,1,1,1,1,1,5 },
        { 5,1,1,1,1,1,1,5 },
        { 5,1,1,1,1,1,1,5 },
        { 10, 5, 5, 5, 5, 5, 5, 10 },
    };

    public List<Data> children = new List<Data>();

    //determined points
    public int Easy()
    {
        int score = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if(currentPlayer == board[i, j]) score += Mathf.Abs((int)board[i, j]);
                else score -= Mathf.Abs((int)board[i, j]);
            }
        }
        return score;
    }

    public int Medium()
    {
        int score = 0;
        Vector2 centerPoint = new Vector2(3.5f, 3.5f);
        Vector2 tmpPoint = new Vector2(3.5f, 3.5f);
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                tmpPoint.x = i;
                tmpPoint.y = j;
                float dist = Vector2.Distance(centerPoint, tmpPoint);
                if (currentPlayer == board[i, j]) score += Mathf.Abs((int)((int)board[i, j] * dist * dist));
                else score -= Mathf.Abs((int)((int)board[i, j] * dist * dist));
            }
        }
        return score;
    }

    public int Hard()
    {
        int score = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (currentPlayer == board[i, j]) score += Mathf.Abs((int)board[i, j] * weight[i, j]);
                else score -= Mathf.Abs((int)board[i, j] * weight[i, j]);
            }
        }
        return score;
    }

    public int Alphabeta(int alpha=-100000, int beta = 100000)
    {
        int v;
        int alphabeta;
        if (children.Count == 0)
        {
            value = evaluate();
            return value;
        }
        else if (isOpponent)
        {
            v = 100000;
            for (int i = 0; i < children.Count; i++)
            {
                alphabeta = children[i].Alphabeta(alpha, beta);
                v = Mathf.Min(v, alphabeta);
                if (alpha >= v)
                {
                    value = v;
                    return v;
                }
                beta = Mathf.Min(beta, v);
            }
        }
        else
        {
            v = -100000;
            for (int i = 0; i < children.Count; i++)
            {
                alphabeta = children[i].Alphabeta(alpha, beta);
                v = Mathf.Max(v, alphabeta);
                if (v >= beta)
                {
                    value = v;
                    return v;
                }
                alpha = Mathf.Max(alpha, v);
            }
        }
        value = v;
        return v;
    }

    public int minmax()
    {
        if (children.Count == 0)
        {
            value = evaluate();
            return value;
        }
        else if (isOpponent)
        {
            int minValue = 100000;
            for (int i = 0; i < children.Count; i++)
            {
                int tmpValue = children[i].minmax();
                if (tmpValue < minValue)
                    minValue = tmpValue;
            }
            value = minValue;
            return minValue;
        }
        else
        {
            int maxValue = -100000;
            for (int i = 0; i < children.Count; i++)
            {
                int tmpValue = children[i].minmax();
                if (tmpValue > maxValue)
                    maxValue = tmpValue;
            }
            value = maxValue;
            return maxValue;
        }
    }

    public int evaluate()
    {
        switch(Settings.current)
        {
            case Settings.PLAYERTYPE.AI1:
                return Easy();
            case Settings.PLAYERTYPE.AI2:
                return Medium();
            case Settings.PLAYERTYPE.AI3:
                return Hard();
            default:
                return Easy();
        }
    }

    public int TrueEvaluate()
    {
        return Easy();
    }

    public List<Playable> GetPlayables()
    {
        List<Playable> playables = new List<Playable>();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {

                Vector2Int position = new Vector2Int(i,j);
                Playable playable = new Playable();
                playable.flips = GetFlippables(position);
                playable.position = position;

                if (playable.flips.Count > 0)
                {
                    playables.Add(playable);
                }
            }
        }
        return playables;
    }

    public void SwitchPlayer()
    {
        currentPlayer = (STATE)(-(int)currentPlayer);
    }

    public STATE GetOpponent()
    {
       return (STATE)(-(int)currentPlayer);
    }

    private List<Vector2Int> GetFlippables(Vector2Int point)
    {

        List<Vector2Int> flips = new List<Vector2Int>();
        List<Vector2Int> tmpFlips = new List<Vector2Int>();

        if (board[point.x, point.y] != STATE.EMPTY) return flips;

        Vector2Int[] offsets = new Vector2Int[8];
        offsets[0] = new Vector2Int(-1, -1);
        offsets[1] = new Vector2Int(0, -1);
        offsets[2] = new Vector2Int(1, -1);
        offsets[3] = new Vector2Int(-1, 0);
        offsets[4] = new Vector2Int(1, 0);
        offsets[5] = new Vector2Int(-1, 1);
        offsets[6] = new Vector2Int(0, 1);
        offsets[7] = new Vector2Int(1, 1);

        for (int i = 0; i < 8; i++)
        {
            tmpFlips.Clear();
            Vector2Int tmp = point + offsets[i];
            while (
                tmp.x > -1
                && tmp.x < board.GetLength(0)
                && tmp.y > -1
                && tmp.y < board.GetLength(1)
                && board[tmp.x, tmp.y] == GetOpponent()
                )
            {
                tmpFlips.Add(tmp);
                tmp += offsets[i];
            }

            if (tmp.x > -1 && tmp.x < board.GetLength(0) && tmp.y > -1 && tmp.y < board.GetLength(1) && board[tmp.x, tmp.y] == currentPlayer)
            {
                flips.AddRange(tmpFlips);
            }
        }
        return flips;
    }
}
