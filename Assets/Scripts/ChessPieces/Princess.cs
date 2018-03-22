using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour, IChessPiece {
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
        int maxSpace = 0;
        int x = currentPos % 4;
        int y = currentPos / 4;
        List<int> validMoves = new List<int>();
        int index = -1;

        if(x != 0)
        {
            // Left
            index = Library.GetBoardIndex(x - 1, y);
            if(empty(board, index))
            {
                validMoves.Add(index);
                x--;
                index = Library.GetBoardIndex(x - 1, y);
                if(empty(board, index))
                    validMoves.Add(index);
            }
            if(opponent(board, index))
            {
                validMoves.Add(index);
            }

            // Up and Left
            x = currentPos % 4;
            if (y != 0)
            {
                index = Library.GetBoardIndex(x - 1, y - 1);
                if(empty(board, index))
                {
                    validMoves.Add(index);
                    x--;
                    y--;
                    index = Library.GetBoardIndex(x - 1, y - 1);
                    if(empty(board, index))
                        validMoves.Add(index);
                }
                if (opponent(board, index))
                {
                    validMoves.Add(index);
                }
            }

            // Down and Left
            x = currentPos % 4;
            y = currentPos / 4;
            if (y != 7)
            {
                index = Library.GetBoardIndex(x - 1, y + 1);
                if(empty(board, index))
                {
                    validMoves.Add(index);
                    x--;
                    y++;
                    index = Library.GetBoardIndex(x - 1, y + 1);
                    if(empty(board, index))
                        validMoves.Add(index);
                }
                if (opponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
        }

        x = currentPos % 4;
        y = currentPos / 4;
        if (x != 3)
        {
            // Right
            index = Library.GetBoardIndex(x + 1, y);
            if(empty(board, index))
            {
                validMoves.Add(index);
                x++;
                index = Library.GetBoardIndex(x + 1, y);
                if(empty(board, index))
                    validMoves.Add(index);
            }
            if (opponent(board, index))
            {
                validMoves.Add(index);
            }

            // Up and Right
            x = currentPos % 4;
            if (y != 0)
            {
                index = Library.GetBoardIndex(x + 1, y - 1);
                if (empty(board, index))
                {
                    validMoves.Add(index);
                    x++;
                    y--;
                    index = Library.GetBoardIndex(x + 1, y - 1);
                    if(empty(board, index))
                        validMoves.Add(index);
                }
                if (opponent(board, index))
                {
                    validMoves.Add(index);
                }
            }

            // Down and Right
            x = currentPos % 4;
            y = currentPos / 4;
            if (y != 7)
            {
                index = Library.GetBoardIndex(x + 1, y + 1);
                if (empty(board, index))
                {
                    validMoves.Add(index);
                    x++;
                    y++;
                    index = Library.GetBoardIndex(x + 1, y + 1);
                    if(empty(board, index))
                        validMoves.Add(index);
                }
                if (opponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
        }

        x = currentPos % 4;
        y = currentPos / 4;
        if (y != 0)
        {
            // Up
            index = Library.GetBoardIndex(x, y - 1);
            if (empty(board, index))
            {
                validMoves.Add(index);
                y--;
                index = Library.GetBoardIndex(x, y - 1);
                if(empty(board, index))
                    validMoves.Add(index);
            }
            if (opponent(board, index))
            {
                validMoves.Add(index);
            }
        }

        y = currentPos / 4;
        if (y != 7)
        {
            // Down
            index = Library.GetBoardIndex(x, y + 1);
            if (empty(board, index))
            {
                validMoves.Add(index);
                y++;
                index = Library.GetBoardIndex(x, y + 1);
                if(empty(board, index))
                    validMoves.Add(index);
            }
            if (opponent(board, index))
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
        Debug.Log(name + " Captured");
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
