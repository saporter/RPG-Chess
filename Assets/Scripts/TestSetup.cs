using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

/*
 * This class (and corresponding scene GameObject) is meant to contain all test pieces and setup for demonstration purposes.  
 * It also demonstrates how the GameManager can be interacted with to setup and start (or reset) a game --> see coment marked with ** 
 */ 
public class TestSetup : MonoBehaviour {
    public GameManager gameManager;
    public GameObject BoardSquares;
    public GameObject TestPiece;
    public GameObject WhitePawn;
    public GameObject BlackPawn;

	// Use this for initialization
	void Start () {
        List<GameObject> board = new List<GameObject>();
        foreach (Outline h in BoardSquares.transform.GetComponentsInChildren<Outline>())
        {
            board.Add(h.gameObject);
        }

        // Put the test piece somewhere
        int x = 2;
        int y = 4;
        IChessPiece piece = TestPiece.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        // Place pawn
        x = 0;
        y = 6;
        piece = WhitePawn.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        for (int i = 0; i < 4; ++i){
            x = i;
            y = 6;
            GameObject go = Instantiate(WhitePawn);
            piece = go.GetComponent<IChessPiece>();
            piece.gameObject.transform.position = board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
            board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;
        }

        for (int i = 0; i < 4; ++i)
        {
            x = i;
            y = 1;
            GameObject go = Instantiate(BlackPawn);
            piece = go.GetComponent<IChessPiece>();
            piece.gameObject.transform.position = board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
            board[gameManager.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;
        }

        // --- ** ----
        // This is how to setup up the game manager
        // --- ** ----
        gameManager.ResetBoard(board);
	}
	
}
