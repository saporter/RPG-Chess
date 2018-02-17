using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour, IChessPiece
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
        int x = currentPos % 4;
        int y = currentPos / 4;
        List<int> validMoves = new List<int>();

        if(emptyOrOpponent(board, x + 1, y + 2))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x + 1, y + 2));
        }
        if (emptyOrOpponent(board, x - 1, y + 2))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x - 1, y + 2));
        }
        if (emptyOrOpponent(board, x + 1, y - 2))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x + 1, y - 2));
        }
        if (emptyOrOpponent(board, x - 1, y - 2))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x - 1, y - 2));
        }
        if (emptyOrOpponent(board, x + 2, y + 1))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x + 2, y + 1));
        }
        if (emptyOrOpponent(board, x + 2, y - 1))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x + 2, y - 1));
        }
        if (emptyOrOpponent(board, x - 2, y + 1))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x - 2, y + 1));
        }
        if (emptyOrOpponent(board, x - 2, y - 1))
        {
            validMoves.Add(GameManager.Instance.GetBoardIndex(x - 2, y - 1));
        }

        return validMoves;
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

    private bool emptyOrOpponent(List<GameObject> board, int x, int y)
    {
        int index = GameManager.Instance.GetBoardIndex(x, y);
        if (index < 0)
            return false;
        return board[index].GetComponent<Square>().Piece == null || board[index].GetComponent<Square>().Piece.Team != team;
    }
}
