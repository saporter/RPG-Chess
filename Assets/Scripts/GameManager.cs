using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using cakeslice;

/*
 * This is GameManager is mostly built as a test environment for now to manage an 8x4 chess board.
 * Player can a move a random piece up, down, left or right.  Movable squares will highlight on mouse rollover.
 * 
 * GameManager is a Singleton.  To access instance, call GameManager.Instance - see TestPiece.cs for an example
 * */
public class GameManager : Singleton<GameManager> {
    protected GameManager() { } // guarantees this will always be a singleton because this prevents the use of the constructor

    // Event system for managing clicks
    [System.Serializable]
    public class GameEvent : UnityEvent { }
    [SerializeField]
    public GameEvent TurnChanged;

    public Affiliation CurrentTurn;

    private List<GameObject> board;
    private int selectedIndex;          // Currently selected piece that is about to move

    /*
     * Maps the x and y parameters (denoting a place on the board) to the single dimensional index used by the List<> array
     * Static so that it can be used by Chess Piece implementations for convenience
     */
    public int GetBoardIndex(int x, int y)
    {
        return 4 * y + x;
    }

    /*
     * The old board will be destroyed, and the new board will be used
     */ 
    public void ResetBoard(List<GameObject> newBoard)
    {
        // Delete old board if one exists
        if(board != null){
            foreach(GameObject go in board){
                Destroy(go);
            }
            board.Clear();
        }

        // Add GameManager as listener to onClick events
        board = newBoard;
        foreach(GameObject go in board)
        {
            go.GetComponent<Square>().OnClick.AddListener(SquareClicked);
        }

        // Start the game with White as current player
        CurrentTurn = Affiliation.White;
        selectedIndex = -1;
        allOff();
    }

    private void SquareClicked(Square square)
    {
        if (square.GetComponent<Outline>().enabled)
        { 
            
            if(selectedIndex < 0){
                Debug.LogError("A piece has not been selected yet.  All outlines should be off.  Did you miss a call to allOff() somewhere?");
            }

            // Execute the moves of previously selected piece to the new square
            List<ChessCommand> actions = board[selectedIndex].GetComponent<Square>().Piece.Moved(board, selectedIndex, getIndex(square));
            foreach(ChessCommand action in actions)
            {
                action.Execute(board);
            }

            // Change turns
            selectedIndex = -1;
            CurrentTurn = CurrentTurn == Affiliation.White ? Affiliation.Black : Affiliation.White;
            TurnChanged.Invoke();
        } else if (square.Piece != null)  
        { 
            // highlight valid moves
            allOff();
            selectedIndex = getIndex(square);
            List<int> validMoves = square.Piece.AvailableMoves(board, selectedIndex);
            foreach (int i in validMoves)
            {
                board[i].GetComponent<Outline>().enabled = true;
            }

            return; // Do not call AllOff() before exiting method
        }

        allOff();
    }

    private void allOff()
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
