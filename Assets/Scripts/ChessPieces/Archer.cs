using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour, IChessPiece {
    [SerializeField]
    Affiliation team;

    public Affiliation Team
    {
        get
        {
            return team;
        }
    }

    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        int x = currentPos % 4;
        int y = currentPos / 4;
        List<int> ValidMoves = new List<int>();
        int[]Corners = new int[4];

        Corners[0] = Library.GetBoardIndex(x - 2, y + 2);
        Corners[1] = Library.GetBoardIndex(x + 2, y + 2);
        Corners[2] = Library.GetBoardIndex(x - 2, y - 2);
        Corners[3] = Library.GetBoardIndex(x + 2, y - 2);

        foreach (int i in Corners)
        {
            if (i >= 0)
            {
                if (board[i].GetComponent<Square>().Piece == null || board[i].GetComponent<Square>().Piece.Team != team)
                {
                    ValidMoves.Add(i);
                }
            }
        }
        return ValidMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        List<ChessCommand> moves = new List<ChessCommand>();

        if (board[to].GetComponent<Square>().Piece != null)
        {
            moves.Add(new CaptureCommand(to));
        }
        moves.Add(new MoveCommand(this, from, to));

        return moves;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    private bool empty(List<GameObject> board, int index)
    {
        if (index < 0 || index >= board.Count)
            return false;
        return board[index].GetComponent<Square>().Piece == null;
    }

    private bool opponent(List<GameObject> board, int index)
    {
        if (index < 0 || index >= board.Count)
            return false;
        return board[index].GetComponent<Square>().Piece != null ? board[index].GetComponent<Square>().Piece.Team != team : false;
    }

}
