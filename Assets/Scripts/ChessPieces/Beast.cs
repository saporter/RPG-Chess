using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : MonoBehaviour, IChessPiece {
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

        if(x > 1)
        {
            // Left
            index = Library.GetBoardIndex(x - 2, y);
            if(empty(board, index))
                validMoves.Add(index);

            if(opponent(board, index))
                validMoves.Add(index);
        }

        if (x < 2)
        {
            // Right
            index = Library.GetBoardIndex(x + 2, y);
            if(empty(board, index))
                validMoves.Add(index);
  
            if (opponent(board, index))
                validMoves.Add(index);
        }

        if (y > 1)
        {
            // Up
            index = Library.GetBoardIndex(x, y - 2);
            if (empty(board, index))
                validMoves.Add(index);

            if (opponent(board, index))
                validMoves.Add(index);
        }

        y = currentPos / 4;
        if (y < 6)
        {
            // Down
            index = Library.GetBoardIndex(x, y + 2);
            if (empty(board, index))
                validMoves.Add(index);
            
            if (opponent(board, index))
                validMoves.Add(index);
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
