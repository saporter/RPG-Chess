using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : MonoBehaviour, IChessPiece
{
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
        List<int> validMoves = new List<int>();
        if (MoveCommand.LastMove != null)
            validMoves.Add(MoveCommand.LastMove.MoveTo);

        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        List<ChessCommand> moves = new List<ChessCommand>();

        if (board[to].GetComponent<Square>().Piece != null)
            moves.Add(new SwapCommand(from, to));

        return moves;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

}
