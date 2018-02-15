using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A pawn.
 */ 
public class Pawn : MonoBehaviour, IChessPiece {
    private bool startingPosition = true;

    [SerializeField]
    Affiliation team; // White is at the bottom of the screen.  Black on top.  This is important for movement

    public Affiliation Team
    {
        get
        {
            return team;
        }
    }

    /*
     * A pawn can move forward one square unless the pawn has not yet moved in which case 
     * it can move forward 2 squares.  A pawn captures diagonaly. 
     */ 
    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        int x = currentPos % 4;
        int y = currentPos / 4;
        int direction = team == Affiliation.White ? -1 : 1;
        List<int> validMoves = new List<int>();

        // Movement logic
        if(y + direction <= 7 && y + direction >= 0)
        {
            // One move forward
            int index = GameManager.GetBoardIndex(x, y + 1 * direction);
            if(board[index].GetComponent<Square>().Piece == null)
            {
                validMoves.Add(index);
            }

            // Two moves forward if it's the first move
            if(startingPosition)
            {
                index = GameManager.GetBoardIndex(x, y + 2 * direction);
                if (board[index].GetComponent<Square>().Piece == null)
                {
                    validMoves.Add(index);
                }

                if ((y != 1 && team == Affiliation.Black) || (y != 6 && team == Affiliation.White))
                {
                    // An important warning as logic does not test boundaries for second move
                    Debug.LogWarning("Pawn is not starting on second rank.  Is this intentional?");
                }
            }
        }

        // Capture logic
        // TODO: add en passant logic
        if(x != 0 && y != 7 && y != 0){
            int index = GameManager.GetBoardIndex(x - 1, y + 1 * direction);

            // if(an enemy piece is found)
            if(board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team){
                validMoves.Add(index);
            }
        }
        if (x != 7 && y != 7 && y != 0)
        {
            int index = GameManager.GetBoardIndex(x + 1, y + 1 * direction);

            // if(an enemy piece is found)
            if (board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team)
            {
                validMoves.Add(index);
            }
        }

        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        if(startingPosition){
            startingPosition = false;
        }

        List<ChessCommand> moves = new List<ChessCommand>();

        if(board[to].GetComponent<Square>().Piece != null){
            moves.Add(new CaptureCommand(to));
        }
        moves.Add(new MoveCommand(this, from, to));

        return moves;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

}
