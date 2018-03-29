using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A pawn
 * 
 */ 
public class Brute : MonoBehaviour, IChessPiece {

    bool isPushing;
    private int direction;
    private AudioForPieces audioPlayer;

    //public IChessPiece pushedPiece;
    //public IChessPiece opposingPiece;

    [SerializeField]
    Affiliation team; // White is at the bottom of the screen.  Black on top.  This is important for movement

    public Affiliation Team
    {
        get
        {
            return team;
        }
    }

    void Start()
    {
        isPushing = false;
        direction = team == Affiliation.White ? -1 : 1;
        audioPlayer = GetComponent<AudioForPieces>();
    }

    /*
     Brute does not move two spaces at beginning or enpassant. Otherwise, brute is identical to pawn except for
     ability to "push" pieces.
     */
    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        int x = currentPos % 4;
        int y = currentPos / 4;
        List<int> validMoves = new List<int>();
        audioPlayer.SE_PickUp();

        // Movement logic
        if(y + direction <= 7 && y + direction >= 0)
        {
            // One move forward
            int index = Library.GetBoardIndex(x, y + 1 * direction);
            int nextSquare = Library.GetBoardIndex(x, y + 2 * direction);
            if (board[index].GetComponent<Square>().Piece == null)
            {
                validMoves.Add(index);
            }
            else 
            {
                isPushing = true;
                if (board[nextSquare].GetComponent<Square>().Piece == null)
                {
                    Debug.Log("Push this guy into blank");
                    validMoves.Add(index);
                }
                else if(board[nextSquare].GetComponent<Square>().Piece.Team != team)
                {
                    Debug.Log("Push this guy into enemy");
                    validMoves.Add(index);
                }
            }
        }

        // Capture logic
        if(x != 0 && y != 7 && y != 0)
        {
            int index = Library.GetBoardIndex(x - 1, y + 1 * direction);

            // if(an enemy piece is found)
            if(board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team){
                validMoves.Add(index);
            }
        }
        if (x != 3 && y != 7 && y != 0)
        {
            int index = Library.GetBoardIndex(x + 1, y + 1 * direction);

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
        List<ChessCommand> moves = new List<ChessCommand>();

        if(board[to].GetComponent<Square>().Piece != null )
        {
            audioPlayer.SE_Capture();
            if (isPushing)
            {
                isPushing = false;
                moves.Add(new PushCommand(to));
            }
            else
            {
                moves.Add(new CaptureCommand(to));
            }
        }
        moves.Add(new MoveCommand(this, from, to));

        int ystart = from / 4;
        int yend = to / 4;

        // Promotion logic
        if(yend == 0 || yend == 7)
        {
            moves.Add(new PromoteCommand(to, (team == Affiliation.White? "White" : "Black") + "Pawn"));  
        }

        return moves;
    }

    public void OnDeath()
    {
        Debug.Log(name + " Captured");
    }

    private void OnDestroy()
    {
    }
}
