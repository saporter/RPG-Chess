using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : MonoBehaviour, IChessPiece {

    [SerializeField]
    Affiliation team; // White is at the bottom of the screen.  Black on top.

    private AudioForPieces audioPlayer;

    void Start()
    {
        audioPlayer = GetComponent<AudioForPieces>();
    }

    public Affiliation Team
    {
        get
        {
            return this.team;
        }
    }

    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        int x = currentPos % 4;
        int y = currentPos / 4;
        var allMoves = new List<int>();
        audioPlayer.SE_PickUp();

        // Can move up or down one space if empty
        if(y != 0)
        {
            int index = Library.GetBoardIndex(x, y - 1);
            if (board[index].GetComponent<Square>().Piece == null)
                allMoves.Add(index);
        }
        if (y != 7)
        {
            int index = Library.GetBoardIndex(x, y + 1);
            if (board[index].GetComponent<Square>().Piece == null)
                allMoves.Add(index);
        }

        // Will clear row if no uncapturable piece in the way
        int direction = x == 0 ? 1 : -1;
        int space = x + direction;
        bool rowClear = true;
        for (int i = 0; i < 3; ++i)
        {
            int index = Library.GetBoardIndex(space, y);
            var piece = board[index].GetComponent<Square>().Piece;
            if (!(piece == null || piece.gameObject.GetComponent<King>() == null))
            {
                if (piece.Team == team)
                {
                    rowClear = false;
                    break;
                }
            }
            space += direction;
        }

        if (rowClear)
            allMoves.Add(Library.GetBoardIndex(x == 0 ? 3 : 0, y));

        return allMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        int x = from % 4;
        int y = from / 4;
        List<ChessCommand> moves = new List<ChessCommand>();


        if( y != to / 4)    // Vertical moves
        {
            moves.Add(new MoveCommand(this, from, to));
        }
        else{               // Horizontal moves
            int direction = x == 0 ? 1 : -1;
            int space = x + direction;
            for (int i = 0; i < 3; ++i)
            {
                int index = Library.GetBoardIndex(space, y);
                if(board[index].GetComponent<Square>().Piece != null)
                {
                    moves.Add(new CaptureCommand(index));

                }
                space += direction;
            }
            moves.Add(new MoveCommand(this, from, to));
            audioPlayer.SE_Move();
        }

        return moves;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}
