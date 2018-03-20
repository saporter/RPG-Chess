using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRider : MonoBehaviour, IChessPiece {
    [SerializeField]
    Affiliation team;   // White is at the bottom of the screen.  Black on top.  This is important for movement

    private int direction;
    private AudioForPieces audioPlayer;

    void Start()
    {
        direction = team == Affiliation.White ? -1 : 1;
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
        audioPlayer.SE_PickUp();

        // Pawn-like movement logic
        if (y + direction <= 7 && y + direction >= 0)
        {
            // One move forward
            int index = Library.GetBoardIndex(x, y + 1 * direction);
            if (board[index].GetComponent<Square>().Piece == null)
            {
                validMoves.Add(index);
            }
        }

        // Capture logic
        int lastRow = direction < 0 ? 0 : 7;
        if (x != 0 && y != lastRow)
        {
            int index = Library.GetBoardIndex(x - 1, y + 1 * direction);

            // if(an enemy piece is found)
            if (board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team)
            {
                validMoves.Add(index);
            }
        }
        if (x != 3 && y != lastRow)
        {
            int index = Library.GetBoardIndex(x + 1, y + 1 * direction);

            // if(an enemy piece is found)
            if (board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team)
            {
                validMoves.Add(index);
            }
        }

        // Knight-like logic
        if (emptyOrOpponent(board, x + 1, y + 2))
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
        throw new System.NotImplementedException();
    }

    private bool emptyOrOpponent(List<GameObject> board, int x, int y)
    {
        int index = Library.GetBoardIndex(x, y);
        if (index < 0)
            return false;
        return board[index].GetComponent<Square>().Piece == null || board[index].GetComponent<Square>().Piece.Team != team;
    }
}
