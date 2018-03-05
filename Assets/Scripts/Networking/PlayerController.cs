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
        GameEventSystem.Instance.LoadNewSceneEvent.AddListener(CmdLoadScene);
    }

    void MakePiece(GameObject PieceMaker)
    {
        var maker = PieceMaker.GetComponent<MakePieceAtSquare>();
        CmdMakePiece(maker.netId, maker.Location, maker.IsWhite);
    }

    [Command]
    public void CmdMakePiece(NetworkInstanceId MakerID, int location, bool isWhite)
    {
        RpcMakePiece(MakerID, location, isWhite);
    }

    [ClientRpc]
    void RpcMakePiece(NetworkInstanceId MakerID, int location, bool isWhite)
    {
        GameEventSystem.Instance.PromotionEvent.Invoke(location, (isWhite ? "White" : "Black") + "Network");
        GameEventSystem.Instance.MakePieceEvent.Invoke(MakerID);
    }

    private void SquareClicked(GameObject square)
    {
        CmdSquareClicked(Library.GetIndex(GameManager.Instance.Board, square.GetComponent<Square>()));
    }

    [Command]
    private void CmdSquareClicked(int squareIndex)
    {
        GameManager.Instance.RpcSquareClicked(squareIndex);
    }

    private void OnDestroy()
    {
        if(GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.SelectedPieceEvent.RemoveListener(MakePiece);
            GameEventSystem.Instance.OnClick.RemoveListener(SquareClicked);
        }
    }

    [Command]
    private void CmdLoadScene(string sceneName)
    {
        if(sceneName.Contains("Setup"))
        {
            GameManager.Instance.ResetBoard(new List<GameObject>(0));
        }
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }
}
