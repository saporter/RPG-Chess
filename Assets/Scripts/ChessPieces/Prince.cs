using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prince : MonoBehaviour, IChessPiece {
    [SerializeField]
    Affiliation team;
    [SerializeField]
    GameObject KingPrefab;

    private AudioForPieces audioPlayer;
    private int myLocation;

    void Start()
    {
        audioPlayer = GetComponent<AudioForPieces>();
        GameEventSystem.Instance.MakePieceEvent.AddListener(NewKing);
        myLocation = Library.GetIndex(GameManager.Instance.Board, this);
    }

    public Affiliation Team
    {
        get
        {
            return team;
        }
    }

    public List<int> AvailableMoves(List<GameObject> board, int currentPos)
    {
        int x = currentPos % 4;
        int y = currentPos / 4;
        List<int> validMoves = new List<int>();
        int index = -1;

        if (x != 0)
        {
            // Left
            index = Library.GetBoardIndex(x - 1, y);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }

            // Up and Left
            if (y != 0)
            {
                index = Library.GetBoardIndex(x - 1, y - 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
            // Down and Left
            if (y != 7)
            {
                index = Library.GetBoardIndex(x - 1, y + 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
        }
        if (x != 3)
        {
            index = Library.GetBoardIndex(x + 1, y);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }

            // Up and Right
            if (y != 0)
            {
                index = Library.GetBoardIndex(x + 1, y - 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }

            // Down and Right
            if (y != 7)
            {
                index = Library.GetBoardIndex(x + 1, y + 1);
                if (emptyOrOpponent(board, index))
                {
                    validMoves.Add(index);
                }
            }
        }
        if (y != 0)
        {
            // Up
            index = Library.GetBoardIndex(x, y - 1);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }
        }
        if (y != 7)
        {
            // Down
            index = Library.GetBoardIndex(x, y + 1);
            if (emptyOrOpponent(board, index))
            {
                validMoves.Add(index);
            }
        }

        audioPlayer.SE_PickUp();
        return validMoves;
    }

    public List<ChessCommand> Moved(List<GameObject> board, int from, int to)
    {
        List<ChessCommand> moves = new List<ChessCommand>();

        if (board[to].GetComponent<Square>().Piece != null)
        {
            moves.Add(new CaptureCommand(to));
            audioPlayer.SE_Capture();
        }
        moves.Add(new MoveCommand(this, from, to));
        myLocation = to;

        return moves;
    }

    public void OnDeath()
    {
        Debug.Log(name + " Captured");
    }

    private void NewKing(string pieceType)
    {
        
        if ((pieceType.Contains("White") && team == Affiliation.Black) 
            || (pieceType.Contains("Black") && team == Affiliation.White))
            return;

        if(pieceType.Contains("PrincePromotion"))
        {
            Debug.Log("Promoting " + team + " Prince");
            transform.position = transform.position + new Vector3(1000f, 0f, 0f);  // Move self out of the way...

            // Make the King
            GameObject king = Instantiate(KingPrefab);
            Square square = GameManager.Instance.Board[myLocation].GetComponent<Square>();
            king.transform.position = square.transform.position;
            king.transform.SetParent(square.transform.parent);
            square.Piece = king.GetComponent<IChessPiece>();

            Destroy(gameObject);    // ...until self is destroyed

        }
    }

    private bool emptyOrOpponent(List<GameObject> board, int index)
    {
        return board[index].GetComponent<Square>().Piece == null || board[index].GetComponent<Square>().Piece.Team != team;
    }

    private void OnDestroy()
    {
        if(GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.MakePieceEvent.RemoveListener(NewKing);
        }
    }
}
