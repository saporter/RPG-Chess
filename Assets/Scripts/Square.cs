using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Square : MonoBehaviour {
    // Event system for managing clicks
    [System.Serializable]
    public class SquareEvent : UnityEvent<Square> {}
    [SerializeField]
    public SquareEvent OnClick;

    // The piece located at this square
    public IChessPiece Piece;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseUp()
    {
        OnClick.Invoke(this);
    }
}
