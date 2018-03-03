using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A pawn
 * 
 */ 
public class Pawn : MonoBehaviour, IChessPiece {
    private bool startingPosition = true;
    private int enPassant = 0;    // If greater than zero, an En Passant is possible
    private int passantBoardIndex = -1; // where this En Passant can occur
    private int direction;
    private AudioForPieces audioPlayer;

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

    /*
     * A pawn can move forward one square unless the pawn has not yet moved in which case 
     * it can move forward 2 squares.  A pawn captures diagonaly. 
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
            if(board[index].GetComponent<Square>().Piece == null)
            {
                validMoves.Add(index);

                // Two moves forward possible if never moved
                if (startingPosition)
                {
                    index = Library.GetBoardIndex(x, y + 2 * direction);
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
        }

        // Capture logic
        if(x != 0 && y != 7 && y != 0){
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

        // En Passant Logic
        if(enPassant > 0)
        {
            validMoves.Add(passantBoardIndex);
        }

        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        if(startingPosition){
            startingPosition = false;
        }

        List<ChessCommand> moves = new List<ChessCommand>();

        if(board[to].GetComponent<Square>().Piece != null)
        {
            audioPlayer.SE_Capture();
            moves.Add(new CaptureCommand(to));
        }else if(to == passantBoardIndex)
        {
            audioPlayer.SE_Capture();
            moves.Add(new CaptureCommand(passantBoardIndex - 4 * direction));
        }
        moves.Add(new MoveCommand(this, from, to));

        int ystart = from / 4;
        int yend = to / 4;

        // En Passant logic
        if(Mathf.Abs(yend - ystart) == 2)
        {
            int x = to % 4;
            if(x != 0)
            {
                IChessPiece p = board[Library.GetBoardIndex(x - 1, yend)].GetComponent<Square>().Piece;
                if(p != null && p.Team != team && p.gameObject.GetComponent<Pawn>() != null)
                {
                    p.gameObject.GetComponent<Pawn>().EnPassantPossible(Library.GetBoardIndex(x, yend - direction));
                }
            }
            if (x != 3)
            {
                IChessPiece p = board[Library.GetBoardIndex(x + 1, yend)].GetComponent<Square>().Piece;
                if (p != null && p.Team != team && p.gameObject.GetComponent<Pawn>() != null)
                {
                    p.gameObject.GetComponent<Pawn>().EnPassantPossible(Library.GetBoardIndex(x, yend - direction));
                }
            }
        }

        // Promotion logic
        if(yend == 0 || yend == 7)
        {
            moves.Add(new PromoteCommand(to, (team == Affiliation.White? "White" : "Black") + "Pawn"));  
        }

        return moves;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    public void EnPassantPossible(int atLocation)
    {
        enPassant = 2; // 2 to count first oponent move, then my own
        passantBoardIndex = atLocation;
        GameManager.Instance.TurnChanged.AddListener(enPassantTurnCounter);
    }

    private void enPassantTurnCounter()
    {
        enPassant--;
        if(enPassant <= 0)
        {
            passantBoardIndex = -1;
            GameManager.Instance.TurnChanged.RemoveListener(enPassantTurnCounter);
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.TurnChanged.RemoveListener(enPassantTurnCounter);  // A good habit to get into
        }
    }

}
