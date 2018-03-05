using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    [SerializeField]
    Affiliation Team;

	// Use this for initialization
	void Awake() {
        DontDestroyOnLoad(gameObject);
	}

    public override void OnStartLocalPlayer()
    {
        // sloppy way to assign player side
        if(isServer)
        {
            Team = Affiliation.White;
        }else{
            Team = Affiliation.Black;
        }

        GameManager.Instance.LocalPlayer = this;
        GameEventSystem.Instance.SelectedPieceEvent.AddListener(MakePiece);
        GameEventSystem.Instance.OnClick.AddListener(SquareClicked);
        GameEventSystem.Instance.LoadNewSceneEvent.AddListener(LoadScene);

        //GameObject.Find("Setup").GetComponent<Setup>().BeginSetup();
    }

    void MakePiece(GameObject PieceMaker)
    {
        var maker = PieceMaker.GetComponent<MakePieceAtSquare>();
        CmdMakePiece(maker.ObjectID, maker.Location, maker.IsWhite);
    }

    private void SquareClicked(GameObject square)
    {
        CmdSquareClicked(Library.GetIndex(GameManager.Instance.Board, square.GetComponent<Square>()));
    }

    private void LoadScene(string sceneName)
    {
        CmdLoadScene(sceneName);
    }

    [Command]
    public void CmdMakePiece(int MakerID, int location, bool isWhite)
    {
        RpcMakePiece(MakerID, location, isWhite);
    }

    [Command]
    private void CmdSquareClicked(int squareIndex)
    {
        GameManager.Instance.RpcSquareClicked(squareIndex);
    }

    [Command]
    private void CmdLoadScene(string sceneName)
    {
        if (sceneName.Contains("Setup"))
        {
            GameManager.Instance.RpcEmptyBoard();
        }
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    [ClientRpc]
    void RpcMakePiece(int MakerID, int location, bool isWhite)
    {
        GameEventSystem.Instance.PromotionEvent.Invoke(location, (isWhite ? "White" : "Black") + "Network");
        GameEventSystem.Instance.MakePieceEvent.Invoke(MakerID);
    }

    private void OnDestroy()
    {
        if(GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.SelectedPieceEvent.RemoveListener(MakePiece);
            GameEventSystem.Instance.OnClick.RemoveListener(SquareClicked);
            GameEventSystem.Instance.LoadNewSceneEvent.AddListener(CmdLoadScene);
        }
    }
}
