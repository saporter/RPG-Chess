using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour, IChessPiece
{
    private int direction;

    [SerializeField]
    Affiliation team; // White is at the bottom of the screen.  Black on top.  This is important for movement


    public Affiliation Team
    {
        get
        {
            return this.team;
        }
    }

    public List<int> MoveInLine(List<GameObject> board, int currentPos, int xHop, int yHop)
    {
        var toReturn = new List<int>();
        if (currentPos / 4 == (currentPos + xHop) / 4 && currentPos + xHop >= 0 &&
            currentPos + yHop * 4 <= board.Count && currentPos + yHop * 4 >= 0)
        {
            int nextMove = currentPos + xHop + yHop * 4;
            if (board[nextMove].GetComponent<Square>().Piece == null)
            {
                toReturn.Add(nextMove);
                toReturn.AddRange(MoveInLine(board, nextMove, xHop, yHop));
            }
            else if (board[nextMove].GetComponent<Square>().Piece.Team != Team)
            {
                toReturn.Add(nextMove);
            }
        }

        return toReturn;

    }

    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        var allMoves = new List<int>();
        var rightMoves = MoveInLine(board, currentPos, 1, 0);
        allMoves.AddRange(rightMoves);
        var leftMoves = MoveInLine(board, currentPos, -1, 0);
        allMoves.AddRange(leftMoves);

        var upMoves = MoveInLine(board, currentPos, 0, 1);
        allMoves.AddRange(upMoves);
        var downMoves = MoveInLine(board, currentPos, 0, -1);
        allMoves.AddRange(downMoves);

        return allMoves;



        //return new List<int>();
        //int x = currentPos % 4;
        //int y = currentPos / 4;
        //List<int> validMoves = new List<int>();

        //// Movement logic
        //if(y + direction <= 7 && y + direction >= 0)
        //{
        //    // One move forward
        //    int index = GameManager.Instance.GetBoardIndex(x, y + 1 * direction);
        //    if(board[index].GetComponent<Square>().Piece == null)
        //    {
        //        validMoves.Add(index);

        //        // Two moves forward possible if never moved
        //        if (startingPosition)
        //        {
        //            index = GameManager.Instance.GetBoardIndex(x, y + 2 * direction);
        //            if (board[index].GetComponent<Square>().Piece == null)
        //            {
        //                validMoves.Add(index);
        //            }
        //            if ((y != 1 && team == Affiliation.Black) || (y != 6 && team == Affiliation.White))
        //            {
        //                // An important warning as logic does not test boundaries for second move
        //                Debug.LogWarning("Pawn is not starting on second rank.  Is this intentional?");
        //            }
        //        }
        //    }
        //}

        //// Capture logic
        //if(x != 0 && y != 7 && y != 0){
        //    int index = GameManager.Instance.GetBoardIndex(x - 1, y + 1 * direction);

        //    // if(an enemy piece is found)
        //    if(board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team){
        //        validMoves.Add(index);
        //    }
        //}
        //if (x != 7 && y != 7 && y != 0)
        //{
        //    int index = GameManager.Instance.GetBoardIndex(x + 1, y + 1 * direction);

        //    // if(an enemy piece is found)
        //    if (board[index].GetComponent<Square>().Piece != null && board[index].GetComponent<Square>().Piece.Team != team)
        //    {
        //        validMoves.Add(index);
        //    }
        //}

        //// En Passant Logic
        //if(enPassant > 0)
        //{
        //    validMoves.Add(passantBoardIndex);
        //}

        //return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {

        //return new List<ChessCommand>();

        List<ChessCommand> moves = new List<ChessCommand>();

        if (board[to].GetComponent<Square>().Piece != null)
        {
            moves.Add(new CaptureCommand(to));
        }

        moves.Add(new MoveCommand(this, from, to));

        int ystart = from / 4;
        int yend = to / 4;

        // TODO: Add promotion command and logic

        return moves;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
