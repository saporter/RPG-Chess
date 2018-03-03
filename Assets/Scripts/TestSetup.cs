using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

/*
 * This class (and corresponding scene GameObject) is meant to contain all test pieces and setup for demonstration purposes.  
 * It also demonstrates how the GameManager can be interacted with to setup and start (or reset) a game --> see coment marked with ** 
 */ 
public class TestSetup : MonoBehaviour {
    public GameObject BoardSquares;

    public GameObject WhitePawn;
    public GameObject WhiteKing;
    public GameObject WhiteQueen;
    public GameObject WhiteBishop;
    public GameObject WhiteRook;
    public GameObject WhiteKnight;
    public GameObject BlackPawn;
    public GameObject BlackKing;
    public GameObject BlackQueen;
    public GameObject BlackBishop;
    public GameObject BlackRook;
    public GameObject BlackKnight;

	// Use this for initialization
	void Start () {
        GameManager gameManager = GameManager.Instance;
        List<GameObject> board = new List<GameObject>();
        foreach (Outline h in BoardSquares.transform.GetComponentsInChildren<Outline>())
        {
            board.Add(h.gameObject);
        }


        int x = 0;
        int y = 6;
        IChessPiece piece;
        GameObject go;

        // Place pawns
        for (int i = 0; i < 4; ++i){
            x = i;
            y = 6;
            go = Instantiate(WhitePawn);
            piece = go.GetComponent<IChessPiece>();
            piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
            board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;
        }

        for (int i = 0; i < 4; ++i)
        {
            x = i;
            y = 1;
            go = Instantiate(BlackPawn);
            piece = go.GetComponent<IChessPiece>();
            piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
            board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;
        }

        // Kings
        x = 2;
        y = 7;
        go = Instantiate(WhiteKing);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        x = 2;
        y = 0;
        go = Instantiate(BlackKing);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        // Queen
        x = 1;
        y = 7;
        go = Instantiate(WhiteQueen);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        x = 1;
        y = 0;
        go = Instantiate(BlackQueen);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        // Bishop
        x = 0;
        y = 7;
        go = Instantiate(WhiteBishop);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        x = 0;
        y = 0;
        go = Instantiate(BlackBishop);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        // Bishop
        x = 3;
        y = 7;
        go = Instantiate(WhiteRook);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        x = 3;
        y = 0;
        go = Instantiate(BlackRook);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        // Knight
        x = 3;
        y = 5;
        go = Instantiate(WhiteKnight);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;

        x = 3;
        y = 2;
        go = Instantiate(BlackKnight);
        piece = go.GetComponent<IChessPiece>();
        piece.gameObject.transform.position = board[Library.GetBoardIndex(x, y)].GetComponent<Square>().transform.position;
        board[Library.GetBoardIndex(x, y)].GetComponent<Square>().Piece = piece;


        // --- ** ----
        // This is how to setup up the game manager
        // --- ** ----
        gameManager.ResetBoard(board);
        gameManager.StartGame();
	}
	
}
