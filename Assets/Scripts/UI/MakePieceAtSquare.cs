using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MakePieceAtSquare : NetworkBehaviour {
    [SerializeField]
    GameObject WhitePiece;
    [SerializeField]
    GameObject BlackPiece;

    [SyncVar]
    private int Location = -1;
    [SyncVar]
    private bool IsWhite;

    public void MakePiece()
    {
        CmdMakePiece();
    }

    [Command]
    private void CmdMakePiece()
    {
        RpcMakePiece();
        RpcPromotionEvent();
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PromotionEvent.AddListener(promotedLocation);
        }
    }

    private void promotedLocation(int atLocation, string type)
    {
        Location = atLocation;
        IsWhite = type.Contains("White");
    }

    [ClientRpc]
    private void RpcMakePiece()
    {
        List<GameObject> board = GameManager.Instance.Board;
        GameObject prefab = IsWhite ? WhitePiece : BlackPiece;

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
        piece.transform.SetParent(square.transform.parent);
        square.Piece = piece.GetComponent<IChessPiece>();

        if(square.Piece == null)
        {
            Debug.LogError("Prefab created does not implement interface IChessPiece");
            return;
        }

        GameManager.Instance.PieceAddedEvent.Invoke(Location, IsWhite ? "White" : "Black");
    }

    [ClientRpc]
    private void RpcPromotionEvent()
    {
        GameManager.Instance.PromotionEvent.Invoke(-1, "Off");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PromotionEvent.RemoveListener(promotedLocation);  // A good habit to get into
        }
    }
}
