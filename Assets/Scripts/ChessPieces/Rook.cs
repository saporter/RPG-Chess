using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour, IChessPiece
{
    private int direction;

    [SerializeField]
    Affiliation team; // White is at the bottom of the screen.  Black on top.


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
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        List<ChessCommand> moves = new List<ChessCommand>();

        if (board[to].GetComponent<Square>().Piece != null)
        {
            moves.Add(new CaptureCommand(to));
        }

        moves.Add(new MoveCommand(this, from, to));

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
