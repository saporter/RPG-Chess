using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Setup : MonoBehaviour {
    public GameObject BoardSquares;
    public GameObject WhiteBoardSetup;
    public GameObject BlackBoardSetup;
    public GameObject WhiteKingPrefab;
    public GameObject BlackKingPrefab;
    public GameObject DefaultIconPrefab;

    private List<GameObject> WhiteSlots;
    private List<GameObject> BlackSlots;

	// Use this for initialization
	void Start () {
        // Create a new board and place Kings
        GameObject squares = Instantiate(BoardSquares);
        List<GameObject> board = squares.GetChildren();
        GameObject king = Instantiate(WhiteKingPrefab);
        king.transform.position = board[30].GetComponent<Square>().transform.position;
        board[30].GetComponent<Square>().Piece = king.GetComponent<IChessPiece>();

        king = Instantiate(BlackKingPrefab);
        king.transform.position = board[2].GetComponent<Square>().transform.position;
        board[2].GetComponent<Square>().Piece = king.GetComponent<IChessPiece>();

        // Setup board in GameManager and listen for pieces added
        GameManager.Instance.ResetBoard(board);
        GameManager.Instance.PieceAddedEvent.AddListener(pieceAdded);
        squares.transform.SetParent(GameManager.Instance.transform);

        // Some slot UI references and setup
        WhiteSlots = WhiteBoardSetup.GetChildren();
        BlackSlots = BlackBoardSetup.GetChildren();
        pieceAdded(30, "White");
        pieceAdded(2, "Black");
	}

    private void pieceAdded(int location, string type)
    {
        GameObject slot = type.Contains("White") ? WhiteSlots.GetSlot(location) : BlackSlots.GetSlot(location);
        if(slot != null)
        {
            var Piece = GameManager.Instance.Board[location].GetComponent<Square>().Piece;
            GameObject iconPrefab = DefaultIconPrefab;

            if (Piece.gameObject.GetComponent<IconUI>() != null)
            {
                if(Piece.gameObject.GetComponent<IconUI>().ImagePrefab == null)
                {
                    Debug.LogWarning("IconUI component found on Chess Piece but Image Prefab is null");
                }
                else
                {
                    iconPrefab = Piece.gameObject.GetComponent<IconUI>().ImagePrefab;
                }
            }

            slot.DestroyChildren();
            Instantiate(iconPrefab).transform.SetParent(slot.transform);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PromotionEvent.RemoveListener(pieceAdded);  // A good habit to get into
        }
    }
}

public static class HelperMethods
{
    public static List<GameObject> GetChildren(this GameObject go)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in go.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }

    public static void DestroyChildren(this GameObject go)
    {
        foreach (Transform tran in go.transform)
        {
            GameObject.Destroy(tran.gameObject);
        }

    }

    public static GameObject GetSlot(this List<GameObject> slots, int location)
    {
        foreach(GameObject slot in slots)
        {
            if(slot.GetComponent<PieceSelector>().BoardIndex == location)
            {
                return slot;
            }
        }

        return null;
    }
}
