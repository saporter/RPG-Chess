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
            validMoves.Add(GameManager.GetBoardIndex(x - 1, y));
        }
        if (x != 3)
        {
            validMoves.Add(GameManager.GetBoardIndex(x + 1, y));
        }
        if (y != 0)
        {
            validMoves.Add(GameManager.GetBoardIndex(x, y - 1));
        }
        if (y != 7)
        {
            validMoves.Add(GameManager.GetBoardIndex(x, y + 1));
        }
        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        throw new System.NotImplementedException();
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
