using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour, IChessPiece {
    [SerializeField]
    Affiliation team;

    private AudioForPieces audioPlayer;

    void Start()
    {
        audioPlayer = GetComponent<AudioForPieces>();
    }

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
        int index = -1;

        if (x != 0)
        {
            // Left
            index = GameManager.Instance.GetBoardIndex(x - 1, y);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }

            // Up and Left
            if (y != 0)
            {
                index = GameManager.Instance.GetBoardIndex(x - 1, y - 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
            // Down and Left
            if (y != 7)
            {
                index = GameManager.Instance.GetBoardIndex(x - 1, y + 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
        }
        if (x != 3)
        {
            index = GameManager.Instance.GetBoardIndex(x + 1, y);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }

            // Up and Right
            if (y != 0)
            {
                index = GameManager.Instance.GetBoardIndex(x + 1, y - 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }

            // Down and Right
            if (y != 7)
            {
                index = GameManager.Instance.GetBoardIndex(x + 1, y + 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
        }
        if (y != 0)
        {
            // Up
            index = GameManager.Instance.GetBoardIndex(x, y - 1);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }
        }
        if (y != 7)
        {
            // Down
            index = GameManager.Instance.GetBoardIndex(x, y + 1);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }
        }

        audioPlayer.SE_PickUp();
        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        List<ChessCommand> moves = new List<ChessCommand>();

        if (board[to].GetComponent<Square>().Piece != null)
        {
            moves.Add(new CaptureCommand(to));
            audioPlayer.SE_Capture();
        }
        moves.Add(new MoveCommand(this, from, to));

        return moves;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    private bool emptyOrOpponent(List<GameObject> board, int index)
    {
        return board[index].GetComponent<Square>().Piece == null || board[index].GetComponent<Square>().Piece.Team != team;
    }
}
