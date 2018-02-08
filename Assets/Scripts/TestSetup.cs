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

	// Use this for initialization
	void Start () {
        List<GameObject> board = new List<GameObject>();
        foreach (Outline h in BoardSquares.transform.GetComponentsInChildren<Outline>())
        {
            board.Add(h.gameObject);
        }

        // Put the test piece somewhere
        int testx = 2;
        int testy = 4;
        IChessPiece piece = TestPiece.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[GameManager.GetBoardIndex(testx, testy)].GetComponent<Square>().transform.position;
        board[GameManager.GetBoardIndex(testx, testy)].GetComponent<Square>().Piece = piece;

        // --- ** ----
        // This is how to setup up the game manager
        // --- ** ----
        gameManager.ResetBoard(board);
	}
	
}
