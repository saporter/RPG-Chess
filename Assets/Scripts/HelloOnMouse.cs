using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloOnMouse : MonoBehaviour {

    private void OnMouseOver()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().HighlightTest();

    }

    private void OnMouseExit()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Off();
    }
}
