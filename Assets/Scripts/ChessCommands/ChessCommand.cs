using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract Chess Command. Will implement move, capture, or other creative game logic in these sub classes.  
 * See IChessPiece for where creation - and GameManager for where invocation - typically occurs.
 * */
public abstract class ChessCommand {

    // Command logic
    public abstract void Execute(List<GameObject> board);
}
