using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using cakeslice;

/*
 * This is GameManager is mostly built as a test environment for now to manage an 8x4 chess board.
 * Player can a move a random piece up, down, left or right.  Movable squares will highlight on mouse rollover.
 * 
 * GameManager is a Singleton.  To access instance, call GameManager.Instance - see TestPiece.cs for an example
 * */
public class GameManager : NetworkSingleton
{
    protected GameManager() { } // guarantees this will always be a singleton because this prevents the use of the constructor

    public Affiliation CurrentTurn;
    public PlayerController LocalPlayer;

    private List<GameObject> board;
    private int selectedIndex;          // Currently selected piece that is about to move
    private bool playing = true;
    private bool manualDestroy = false;

    // For debug and testing.  Remove for build
    private bool enforceTurns = true;
    public bool EnforceTurns { get { return enforceTurns; } set { enforceTurns = value; Debug.Log("Turn enforcement now: " + enforceTurns); } }

    public List<GameObject> Board
    {
        get
        {
            return board;
        }
    }

    private void Awake()
    {
        if(Instance != this)
        {
            manualDestroy = true;       // Do not call Singleton OnDestroy()
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        playing = false;
        SceneManager.sceneLoaded += sceneLoaded;
    }

    ///*
    // * Look in Library class for this helper method
    // */
    //public int GetBoardIndex(int x, int y)
    //{
    //    return 0;
    //}


    /*
     * The old board will be destroyed, and the new board will be used
     */
    public void ResetBoard(List<GameObject> newBoard)
    {
        // Delete old board if one exists
        if (board != null)
        {
            foreach (GameObject go in board)
            {
                if (go.GetComponent<Square>().Piece != null)
                {
                    Destroy(go.GetComponent<Square>().Piece.gameObject);
                }
                Destroy(go);
            }
            board.Clear();
        }

        board = newBoard;

        // Start the game with White as current player
        CurrentTurn = Affiliation.White;
        selectedIndex = -1;
        playing = false;
        allOff();
    }

    [ClientRpc]
    public void RpcSquareClicked(int squareIndex)
    {
        if (!playing)
            return;
        
        Square square = board[squareIndex].GetComponent<Square>();

        if (square.GetComponent<Outline>().enabled)
        {

            if (selectedIndex < 0)
            {
                Debug.LogError("A piece has not been selected yet.  All outlines should be off.  Did you miss a call to allOff() somewhere?");
            }

            // Execute the moves of previously selected piece to the new square
            List<ChessCommand> actions = board[selectedIndex].GetComponent<Square>().Piece.Moved(board, selectedIndex, squareIndex);

            foreach (ChessCommand action in actions)
            {
                action.Execute(board);
            }

            // Change turns
            selectedIndex = -1;
            CurrentTurn = CurrentTurn == Affiliation.White ? Affiliation.Black : Affiliation.White;
            GameEventSystem.Instance.TurnChanged.Invoke();
        }
        else if (square.Piece != null)
        {

            allOff();

            // check turn
            if (CurrentTurn != square.Piece.Team)
            {
                // I don't know how to surround the "if" porton of this statement with a #build tag of some sort to remove for builds
                if (EnforceTurns)
                    return;
            }

            // highlight valid moves
            selectedIndex = squareIndex;
            List<int> validMoves = square.Piece.AvailableMoves(board, selectedIndex);
            foreach (int i in validMoves)
            {
                board[i].GetComponent<Outline>().enabled = true;
            }

            return; // Do not call AllOff() before exiting method
        }

        allOff();
    }

    [ClientRpc]
    public void RpcEmptyBoard()
    {
        ResetBoard(new List<GameObject>(0));
    }

    private void sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        allOff();
        playing = true;
    }

    private void allOff()
    {
        if (board == null)
            return;
        
        foreach (GameObject go in board)
        {
            go.GetComponent<Outline>().enabled = false;
        }
    }

    public override void OnDestroy()
    {
        if(!manualDestroy)
            base.OnDestroy();
        SceneManager.sceneLoaded -= sceneLoaded;
    }
}
