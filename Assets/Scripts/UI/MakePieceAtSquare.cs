﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MakePieceAtSquare : MonoBehaviour {
    [SerializeField]
    GameObject WhitePiece;
    [SerializeField]
    GameObject BlackPiece;

    private int location = -1;
    private bool isWhite;

    public int Location { get { return location; } }
    public bool IsWhite { get { return isWhite; } }

    public void MakePiece()
    {
        GameEventSystem.Instance.SelectedPieceEvent.Invoke(gameObject);
    }

    private void Start()
    {
        if (GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.PromotionEvent.AddListener(promotedLocation);
            GameEventSystem.Instance.MakePieceEvent.AddListener(MakePieceListener);
        }
    }

    private void MakePieceListener(string stringID)
    {
        if (stringID == name)
        {
            MakePiece(GameManager.Instance.Board, isWhite ? WhitePiece : BlackPiece);
            GameEventSystem.Instance.PromotionEvent.Invoke(-1, "Off");
        }
    }

    private void promotedLocation(int atLocation, string type)
    {
        location = atLocation;
        isWhite = type.Contains("White");
    }

    private void MakePiece(List<GameObject> board, GameObject prefab)
    {
        if(location < 0)
        {
            Debug.LogError("Location is less than zero.  MakePiece() does not know where to place created piece.  " +
                           "Are you sure the UI is displaying correctly?" +
                           "Does another MakePieceAtSquare UI element have the same name as this one? (add a trailing space to the other element if so)");
            return;
        }

        if (board[location].GetComponent<Square>().Piece != null)
        {
            Destroy(board[location].GetComponent<Square>().Piece.gameObject);
        }

        GameObject piece = Instantiate(prefab);
        Square square = board[location].GetComponent<Square>();
        piece.transform.position = square.transform.position;
        piece.transform.SetParent(square.transform.parent);
        square.Piece = piece.GetComponent<IChessPiece>();

        if(square.Piece == null)
        {
            Debug.LogError("Prefab created does not implement interface IChessPiece");
            return;
        }

        GameEventSystem.Instance.PieceAddedEvent.Invoke(location, isWhite ? "White" : "Black");
    }

    private void OnDestroy()
    {
        if (GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.PromotionEvent.RemoveListener(promotedLocation);  // A good habit to get into
        }
    }
}
