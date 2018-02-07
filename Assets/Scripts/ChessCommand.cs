using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract Chess Command. Will implement move, capture, or other creative game logic in these sub classes.  
 * See IChessPiece for where invocation typically occurs.
 * */
public abstract class ChessCommand {
    // Where this command should happen
    public abstract int Location { get; }

    // Command logic
    public abstract void Execute(List<GameObject> board);
}
