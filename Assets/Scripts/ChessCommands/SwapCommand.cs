using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCommand : ChessCommand {
    private IChessPiece movingPiece;
    public int MoveFrom { get { return moveFrom; } }
    private int moveFrom;
    public int MoveTo { get { return moveTo; } }
    private int moveTo;

    public SwapCommand(int from, int to)
    {
        moveFrom = from;
        moveTo = to;
    }

    public override void Execute(List<GameObject> board)
    {
        var pieceA = board[moveFrom].GetComponent<Square>().Piece;
        var pieceB = board[moveTo].GetComponent<Square>().Piece;

        board[moveFrom].GetComponent<Square>().Piece = pieceB;
        board[moveTo].GetComponent<Square>().Piece = pieceA;

        if(pieceA!=null)
            pieceA.gameObject.transform.position = board[moveTo].GetComponent<Square>().transform.position;

        if(pieceB!=null)
            pieceB.gameObject.transform.position = board[moveFrom].GetComponent<Square>().transform.position;
    }

}
