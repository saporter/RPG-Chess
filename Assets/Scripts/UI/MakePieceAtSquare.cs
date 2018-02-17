using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePieceAtSquare : MonoBehaviour {
    [SerializeField]
    GameObject WhitePiece;
    [SerializeField]
    GameObject BlackPiece;

    private int Location = -1;
    private bool IsWhite;

    public void MakePiece()
    {
        MakePiece(GameManager.Instance.Board, IsWhite ? WhitePiece : BlackPiece);
        GameManager.Instance.PromotionEvent.Invoke(-1, "Off");
    }

    private void Start()
    {
        GameManager.Instance.PromotionEvent.AddListener(promotedLocation);
    }

    private void promotedLocation(int atLocation, string type)
    {
        Location = atLocation;
        IsWhite = type.Contains("White");
    }

    private void MakePiece(List<GameObject> board, GameObject prefab)
    {
        if(Location < 0)
        {
            Debug.LogError("Location is less than zero.  MakePiece() does not know where to place created piece.  Are you sure the UI is displaying correctly?");
            return;
        }

        if (board[Location].GetComponent<Square>().Piece != null)
        {
            Destroy(board[Location].GetComponent<Square>().Piece.gameObject);
        }

        GameObject piece = Instantiate(prefab);
        Square square = board[Location].GetComponent<Square>();
        piece.transform.position = square.transform.position;
        square.Piece = piece.GetComponent<IChessPiece>();

        if(square.Piece == null)
        {
            Debug.LogError("Prefab created does not implement interface IChessPiece");
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PromotionEvent.RemoveListener(promotedLocation);  // A good habit to get into
        }
    }
}
