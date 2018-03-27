using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : MonoBehaviour, IChessPiece {

    private AudioForPieces audioPlayer;
    private int direction;

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
        direction = team == Affiliation.White ? -1 : 1;
        audioPlayer = GetComponent<AudioForPieces>();
    }

    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        //Note: Squire does not advance 2 squares on first move or en passant.

        int x = currentPos % 4;
        int y = currentPos / 4;
        List<int> validMoves = new List<int>();
        audioPlayer.SE_PickUp();

        // Movement logic
        if(y + direction <= 7 && y + direction >= 0)
        {
            // One move forward if opposing piece is in front of squire -only captures forward

            int index = Library.GetBoardIndex(x, y + 1 * direction);
            if(board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team)
            {
                validMoves.Add(index);
            }
        }

        // Regular movement is diagonal

        if(x != 0 && y != 7 && y != 0){
            int index = Library.GetBoardIndex(x - 1, y + 1 * direction);

            if(board[index].GetComponent<Square>().Piece == null)
            {
                validMoves.Add(index);
            }
        }
        if (x != 3 && y != 7 && y != 0)
        {
            int index = Library.GetBoardIndex(x + 1, y + 1 * direction);

            if(board[index].GetComponent<Square>().Piece == null)
            {
                validMoves.Add(index);
            }
        }

        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        List<ChessCommand> moves = new List<ChessCommand>();

        if(board[to].GetComponent<Square>().Piece != null)
        {
            audioPlayer.SE_Capture();
            moves.Add(new CaptureCommand(to));
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
