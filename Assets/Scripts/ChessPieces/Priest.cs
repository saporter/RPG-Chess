using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : MonoBehaviour, IChessPiece
{
    private int direction;

    [SerializeField]
    Affiliation team; // White is at the bottom of the screen.  Black on top.


    public Affiliation Team
    {
        get { return this.team; }
    }

    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        List<int> moves = new List<int>();
        moves.AddRange(Library.MoveInLine(board, currentPos, 1, 1, Team, 1));
        moves.AddRange(Library.MoveInLine(board, currentPos, -1, -1, Team, 1));
        moves.AddRange(Library.MoveInLine(board, currentPos, 1, -1, Team, 1));
        moves.AddRange(Library.MoveInLine(board, currentPos, -1, 1, Team, 1));

        return moves;
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
        Debug.Log(name + " Captured");
    }
}
