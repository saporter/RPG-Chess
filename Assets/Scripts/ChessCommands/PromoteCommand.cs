using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This command works closely with the UI system.  See button scripts in UI folder for those 
 * buttons (and parent panels) that listen for the PawnPromotion event.
 * 
 * PromotionCommand takes a string arument to work with different promotion types.  This allows
 * different pop-up windows to be displayed that are customized to each promoted piece.
 * This way, since special pieces promote to different piece types, you can customize the pop-up
 * window however you like in the editor then attach a script that listens for the right event (called with the 
 * corresponding string parameter - see Pawn for an example).  
 */ 
public class PromoteCommand : ChessCommand
{
    private int location;
    private string pieceType;

    public PromoteCommand(int atLocation, string promotedPieceType)
    {
        location = atLocation;
        pieceType = promotedPieceType;
    }

    public override void Execute(List<GameObject> board)
    {
        GameManager.Instance.PromotionEvent.Invoke(location, pieceType);
    }
}
