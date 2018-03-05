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

    //------ Make Pieces -------
    void MakePiece(GameObject PieceMaker)
    {
        var maker = PieceMaker.GetComponent<MakePieceAtSquare>();
        CmdMakePiece(maker.name, maker.Location, maker.IsWhite);
    }

    [Command]
    public void CmdMakePiece(string stringID, int location, bool isWhite)
    {
        RpcMakePiece(stringID, location, isWhite);
    }

    [ClientRpc]
    void RpcMakePiece(string stringID, int location, bool isWhite)
    {
        GameEventSystem.Instance.PromotionEvent.Invoke(location, (isWhite ? "White" : "Black") + "Network");
        GameEventSystem.Instance.MakePieceEvent.Invoke(stringID);
    }
    //-------------------------------------------------------------------------------------------------------

    //-------- Movement --------
    private void SquareClicked(GameObject square)
    {
        CmdSquareClicked(Library.GetIndex(GameManager.Instance.Board, square.GetComponent<Square>()));
    }

    [Command]
    private void CmdSquareClicked(int squareIndex)
    {
        RpcSquareClicked(squareIndex);
    }

    [ClientRpc]
    private void RpcSquareClicked(int squareIndex)
    {
        GameManager.Instance.SquareClicked(squareIndex);
    }
    //-------------------------------------------------------------------------------------------------------

    //----- Scene Loading ------
    private void LoadScene(string sceneName)
    {
        CmdLoadScene(sceneName);
    }

    [Command]
    private void CmdLoadScene(string sceneName)
    {
        if (sceneName.Contains("Setup"))
        {
            RpcClearBoard();
        }
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    [ClientRpc]
    private void RpcClearBoard()
    {
        GameManager.Instance.ResetBoard(new List<GameObject>(0));
    }
    //-------------------------------------------------------------------------------------------------------


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
