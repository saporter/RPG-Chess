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
    public GameObject TestPiece;        // This is a temporary var for demonstration and will be deleted soon
    private Square testPieceLocation;   // This is a temporary var for demonstration and will be deleted soon

    private List<GameObject> board;
    private int testx, testy;           // These are temporary vars for demonstration and will be deleted soon

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

        // The rest of this is for demonstration only.  Will be deleted soon.
        testx = 2;
        testy = 4;
        testPieceLocation = board[GetBoardIndex(testx, testy)].GetComponent<Square>();
        TestPiece.transform.position = testPieceLocation.transform.position;
        testPieceLocation.Piece = TestPiece.GetComponent<IChessPiece>();

        AllOff();
	}


	
	// Update is called once per frame
    void Update () {
        
        if (CrossPlatformInputManager.GetButtonUp("Horizontal"))
        {
            float input = CrossPlatformInputManager.GetAxis("Horizontal");
            if (input > 0 && testx < 3)
            {
                testx++;
            }
            else if (input < 0 && testx > 0)
            {
                testx--;
            }
        }

        if (CrossPlatformInputManager.GetButtonUp("Vertical"))
        {
            float input = CrossPlatformInputManager.GetAxis("Vertical");
            if (input < 0 && testy < 7)
            {
                testy++;
            }
            else if (input > 0 && testy > 0)
            {
                testy--;
            }
        }

        testPieceLocation.Piece = null;
        testPieceLocation = board[GetBoardIndex(testx, testy)].GetComponent<Square>();
        TestPiece.transform.position = testPieceLocation.transform.position;
        testPieceLocation.Piece = TestPiece.GetComponent<IChessPiece>();
	}

    public void Off()
    {
        AllOff();
    }

    private void SquareClicked(Square square)
    {
        AllOff();

        // TODO: Need to add more user input logic.  For now, all this does is highlight available moves
        // ..

        // highlight valid moves
        if (square.Piece != null)
        {
            List<int> validMoves = square.Piece.AvailableMoves(board, getIndex(square));
            foreach (int i in validMoves)
            {
                board[i].GetComponent<Outline>().enabled = true;
            }
        }
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
