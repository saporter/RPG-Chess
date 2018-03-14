using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Reset : MonoBehaviour {

	public void ReturnToSetup()
    {
        GameEventSystem.Instance.LoadNewSceneEvent.Invoke("Setup");
    }
}
