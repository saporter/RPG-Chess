using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour, IChessPiece
{
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

        if(emptyOrOpponent(board, x + 1, y + 2))
        {
            validMoves.Add(Library.GetBoardIndex(x + 1, y + 2));
        }
        if (emptyOrOpponent(board, x - 1, y + 2))
        {
            validMoves.Add(Library.GetBoardIndex(x - 1, y + 2));
        }
        if (emptyOrOpponent(board, x + 1, y - 2))
        {
            validMoves.Add(Library.GetBoardIndex(x + 1, y - 2));
        }
        if (emptyOrOpponent(board, x - 1, y - 2))
        {
            validMoves.Add(Library.GetBoardIndex(x - 1, y - 2));
        }
        if (emptyOrOpponent(board, x + 2, y + 1))
        {
            validMoves.Add(Library.GetBoardIndex(x + 2, y + 1));
        }
        if (emptyOrOpponent(board, x + 2, y - 1))
        {
            validMoves.Add(Library.GetBoardIndex(x + 2, y - 1));
        }
        if (emptyOrOpponent(board, x - 2, y + 1))
        {
            validMoves.Add(Library.GetBoardIndex(x - 2, y + 1));
        }
        if (emptyOrOpponent(board, x - 2, y - 1))
        {
            validMoves.Add(Library.GetBoardIndex(x - 2, y - 1));
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
        Debug.Log(name + " Captured");
    }

    private bool emptyOrOpponent(List<GameObject> board, int x, int y)
    {
        int index = Library.GetBoardIndex(x, y);
        if (index < 0)
            return false;
        return board[index].GetComponent<Square>().Piece == null || board[index].GetComponent<Square>().Piece.Team != team;
    }
}
