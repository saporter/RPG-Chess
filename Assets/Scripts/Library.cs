using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Library
{
    public static List<int> MoveInLine(List<GameObject> board,
        int currentPos, int xHop, int yHop, Affiliation team, int limit = 100)
    {
        if (limit <= 0)
            return new List<int>(); //we are done
        var toReturn = new List<int>();

        int x = currentPos % 4 + xHop;
        int y = currentPos / 4 + yHop;

        //if we are on the board still
        if (GameManager.Instance.GetBoardIndex(x, y) != -1)
        {
            int nextMove = currentPos + xHop + yHop * 4;
            if (board[nextMove].GetComponent<Square>().Piece == null)
            {
                toReturn.Add(nextMove);
                toReturn.AddRange(MoveInLine(board, nextMove, xHop, yHop, team, limit - 1));
            }
            else if (board[nextMove].GetComponent<Square>().Piece.Team != team)
            {
                toReturn.Add(nextMove);
            }
        }

        return toReturn;

    }


}
