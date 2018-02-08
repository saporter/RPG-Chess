using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityStandardAssets.CrossPlatformInput;

/*
 * This is GameManager is mostly built as a test environment for now to manage an 8x4 chess board.
 * Player can a move a random piece up, down, left or right.  Movable squares will highlight on mouse rollover.
 * */
public class GameManager : MonoBehaviour {
    public GameObject Highlights;
    public GameObject TestPiece;        // **This is a temporary var for demonstration and will be deleted soon

    private List<GameObject> board;
    private int selectedIndex;          // Currently selected piece that is about to move
    private Affiliation currentTurn;    // The player's whos turn it currently is

    /*
     * Maps the x and y parameters (denoting a place on the board) to the single dimensional index used by the List<> array
     * Static so that it can be used by Chess Piece implementations for convenience
     */
    public static int GetBoardIndex(int x, int y)
    {
        return 4 * y + x;
    }

    // Use this for initialization
	void Start () 
    {
        board = new List<GameObject>();
        foreach(Outline h in Highlights.transform.GetComponentsInChildren<Outline>())
        {
            board.Add(h.gameObject);
            h.GetComponent<Square>().OnClick.AddListener(SquareClicked);
        }
        currentTurn = Affiliation.White;
        selectedIndex = -1;

        // The rest of this is for demonstration only.  Will be deleted soon.
        int testx = 2;
        int testy = 4;
        IChessPiece piece = TestPiece.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[GetBoardIndex(testx, testy)].GetComponent<Square>().transform.position;
        board[GetBoardIndex(testx, testy)].GetComponent<Square>().Piece = piece;
                                          
        AllOff();
	}

    private void SquareClicked(Square square)
    {
        if (square.GetComponent<Outline>().enabled)
        { 
            
            if(selectedIndex < 0){
                Debug.LogError("A piece has not been selected yet.  All outlines should be off.  Did you miss a call to AllOff() somewhere?");
            }

            // Execute the moves of previously selected piece to the new square
            List<ChessCommand> actions = board[selectedIndex].GetComponent<Square>().Piece.Moved(board, selectedIndex, getIndex(square));
            foreach(ChessCommand action in actions)
            {
                action.Execute(board);
            }

            // Change turns
            selectedIndex = -1;
            currentTurn = currentTurn == Affiliation.White ? Affiliation.Black : Affiliation.White;
        } else if (square.Piece != null)  
        { 
            // highlight valid moves
            AllOff();
            selectedIndex = getIndex(square);
            List<int> validMoves = square.Piece.AvailableMoves(board, selectedIndex);
            foreach (int i in validMoves)
            {
                board[i].GetComponent<Outline>().enabled = true;
            }

            return; // Do not call AllOff() before exiting method
        }

        AllOff();
    }

    private void AllOff()
    {
        foreach(GameObject go in board)
        {
            go.GetComponent<Outline>().enabled = false;
        }
    }

    /*
     * Determines what index this square corresponds to
     * */
    private int getIndex(Square square)
    {
        for (int i = 0; i < board.Count; ++i)
        {
            if(board[i].GetComponent<Square>() == square) { return i; }
        }
        Debug.LogWarning("Square not found on board");
        return -1;
    }
}
