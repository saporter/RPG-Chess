using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Setup : MonoBehaviour {
    public GameObject BoardSquares;

	// Use this for initialization
	void Start () {
        GameObject squares = Instantiate(BoardSquares);
        List<GameObject> board = new List<GameObject>();
        foreach (Outline h in squares.transform.GetComponentsInChildren<Outline>())
        {
            board.Add(h.gameObject);
        }

        GameManager.Instance.ResetBoard(board);
        squares.transform.SetParent(GameManager.Instance.transform);
	}
	
}
