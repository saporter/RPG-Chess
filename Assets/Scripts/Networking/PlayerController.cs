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
    }

    [Command]
    public void CmdMakePiece()
    {
        RpcMakePiece();
    }

    [ClientRpc]
    void RpcMakePiece()
    {
        if(!isLocalPlayer)
        {
            return;
        }
    }
}
