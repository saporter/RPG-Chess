using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPiece : MonoBehaviour, IChessPiece {
    [SerializeField]
    Affiliation PieceColor;

    /*
     * Set in editor via SerializedField above
     */
    public Affiliation Team
    {
        get
        {
            return PieceColor;
        }
    }

    // We don't actually need to implement this IChessPiece Property.  Because it is named "gameObject", the inherited MonoBehavior implemenation satisfies the IChessPiece requirement
    // public GameObject gameObject { get { return this.gameObject; } }

    /*
     * This test piece can move up, down, left and right
     * */
    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        int x = currentPos % 4;
        int y = currentPos / 4;
        List<int> validMoves = new List<int>();

        if (x != 0)
        {
            validMoves.Add(Library.GetBoardIndex(x - 1, y));
        }
        if (x != 3)
        {
            validMoves.Add(Library.GetBoardIndex(x + 1, y));
        }
        if (y != 0)
        {
            validMoves.Add(Library.GetBoardIndex(x, y - 1));
        }
        if (y != 7)
        {
            validMoves.Add(Library.GetBoardIndex(x, y + 1));
        }
        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        List<ChessCommand> moves = new List<ChessCommand>();
        moves.Add(new MoveCommand(this, from, to));
        return moves;
    }

    public void OnDeath()
    {
        Debug.Log(name + " Captured");
    }
}
