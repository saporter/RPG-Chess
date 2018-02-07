using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Square : MonoBehaviour {
    [System.Serializable]
    public class SquareEvent : UnityEvent {}
    [SerializeField]
    public SquareEvent OnClick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseUp()
    {
        Debug.Log("Mouse clicked " + transform.position);
        OnClick.Invoke();
    }
}
