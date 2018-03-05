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
}
