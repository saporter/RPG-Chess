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
        if (GetBoardIndex(x, y) != -1)
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

    /*
     * Maps the x and y parameters (denoting a place on the board) to the single dimensional index used by the List<> array
     * Static so that it can be used by Chess Piece implementations for convenience
     */
    public static int GetBoardIndex(int x, int y)
    {
        if (x < 0 || x > 3 || y < 0 || y > 7)
        {
            return -1;
        }
        return 4 * y + x;
    }

    /*
     * Determines what index this square corresponds to
     * */
    public static int GetIndex(List<GameObject> board, Square square)
    {
        for (int i = 0; i < board.Count; ++i)
        {
            if (board[i].GetComponent<Square>() == square) { return i; }
        }
        Debug.LogWarning("Square not found on board");
        return -1;
    }

}
