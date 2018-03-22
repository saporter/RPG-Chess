using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using cakeslice;

public class Setup : MonoBehaviour {
    public GameObject BoardSquares;
    public GameObject WhiteBoardSetup;
    public GameObject BlackBoardSetup;
    public GameObject WhiteKingPrefab;
    public GameObject WhitePawnPrefab;
    public GameObject BlackKingPrefab;
    public GameObject BlackPawnPrefab;
    public GameObject DefaultIconPrefab;
    public GameObject PlayButton;

    private List<GameObject> WhiteSlots;
    private List<GameObject> BlackSlots;

    public void LoadMainLevel()
    {
        GameEventSystem.Instance.LoadNewSceneEvent.Invoke("Main");

    }

    private void Start()
    {
        StartCoroutine(WaitForPlayerReady());
        //BeginSetup();
    }

    IEnumerator WaitForPlayerReady()
    {
        bool flag = true;
        while (flag)
        {
            foreach(UnityEngine.Networking.PlayerController player in ClientScene.localPlayers)
            {
                if(player.unetView != null && player.unetView.isLocalPlayer)
                {
                    flag = false;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
        BeginSetup();
    }

    // Use this for initialization
    public void BeginSetup () {
        // Create a new board and place Kings
        GameObject squares = Instantiate(BoardSquares);
        List<GameObject> board = squares.GetChildren();
        GameObject king = Instantiate(WhiteKingPrefab);
        king.transform.position = board[30].GetComponent<Square>().transform.position;
        king.transform.SetParent(squares.transform);
        board[30].GetComponent<Square>().Piece = king.GetComponent<IChessPiece>();

        king = Instantiate(BlackKingPrefab);
        king.transform.position = board[2].GetComponent<Square>().transform.position;
        king.transform.SetParent(squares.transform);
        board[2].GetComponent<Square>().Piece = king.GetComponent<IChessPiece>();

        // Place pawns
        for (int i = 0; i < 4; ++i)
        {
            // Black pawns
            GameObject pawn = Instantiate(BlackPawnPrefab);
            pawn.transform.position = board[4 + i].GetComponent<Square>().transform.position;
            pawn.transform.SetParent(squares.transform);
            board[4 + i].GetComponent<Square>().Piece = pawn.GetComponent<IChessPiece>();

            // White pawns
            pawn = Instantiate(WhitePawnPrefab);
            pawn.transform.position = board[24 + i].GetComponent<Square>().transform.position;
            pawn.transform.SetParent(squares.transform);
            board[24 + i].GetComponent<Square>().Piece = pawn.GetComponent<IChessPiece>();
        }

        // Setup board in GameManager and listen for pieces added
        GameManager.Instance.ResetBoard(board);
        GameEventSystem.Instance.PieceAddedEvent.AddListener(pieceAdded);
        squares.transform.SetParent(GameManager.Instance.transform);

        // Some slot UI references and setup
        BlackSlots = BlackBoardSetup.GetChildren();
        WhiteSlots = WhiteBoardSetup.GetChildren();
        pieceAdded(4, "Black"); pieceAdded(24, "White");    // Pawns
        pieceAdded(5, "Black"); pieceAdded(25, "White");
        pieceAdded(6, "Black"); pieceAdded(26, "White");
        pieceAdded(7, "Black"); pieceAdded(27, "White");
        pieceAdded(2, "Black"); pieceAdded(30, "White");    // Kings

        // Hide play button
        PlayButton.GetComponent<CanvasGroup>().alpha = 0f;
        PlayButton.GetComponent<CanvasGroup>().interactable = false;
        PlayButton.GetComponent<CanvasGroup>().blocksRaycasts = false;

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

        if(ReadyToPlay())
        {
            PlayButton.GetComponent<CanvasGroup>().alpha = 1f;
            PlayButton.GetComponent<CanvasGroup>().interactable = true;
            PlayButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    private bool ReadyToPlay()
    {
        bool ready = true;

        foreach(GameObject slot in WhiteSlots)
        {
            if(slot.transform.childCount == 0)
            {
                ready = false;
            }
        }

        foreach (GameObject slot in BlackSlots)
        {
            if (slot.transform.childCount == 0)
            {
                ready = false;
            }
        }

        return ready;
    }

    private void OnDestroy()
    {
        if (GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.PromotionEvent.RemoveListener(pieceAdded);  // A good habit to get into
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
