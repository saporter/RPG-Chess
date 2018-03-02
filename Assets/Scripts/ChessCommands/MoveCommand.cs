using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ChessCommand {
    private IChessPiece movingPiece;
    public int MoveFrom { get { return moveFrom; } }
    private int moveFrom;
    public int MoveTo { get { return moveTo; } }
    private int moveTo;
    private AudioForPieces audioForPieces;

    public MoveCommand(IChessPiece piece, int from, int to)
    {
        movingPiece = piece;
        moveFrom = from;
        moveTo = to;

        audioForPieces = piece.gameObject.GetComponent<AudioForPieces>();
        if (audioForPieces)
            audioForPieces.SE_Move();
        
        
    }

    public override void Execute(List<GameObject> board)
    {
        board[moveFrom].GetComponent<Square>().Piece = null;
        board[moveTo].GetComponent<Square>().Piece = movingPiece;
        movingPiece.gameObject.transform.position = board[moveTo].GetComponent<Square>().transform.position;
    }

}
