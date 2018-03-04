using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoveToggle : MonoBehaviour {
    public Slider slider;
    public Image handle;
    public Text text;


	// Use this for initialization
	void Start () {
        GameEventSystem.Instance.TurnChanged.AddListener(turnChange);
        turnChange();

	}
	
    private void turnChange()
    {
        if(GameManager.Instance.CurrentTurn == Affiliation.White)
        {
            handle.color = Color.white;
            slider.value = 0f;
            text.text = "White";
        }else
        {
            handle.color = Color.black;
            slider.value = 1f; 
            text.text = "Black";
        }
    }
}
