using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

	public void ReturnToSetup()
    {
        GameManager.Instance.ResetBoard(new List<GameObject>(0));
        SceneManager.LoadSceneAsync("Setup");
    }
}
