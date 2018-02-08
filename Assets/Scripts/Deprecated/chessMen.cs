using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class chessMen : GameManager 
{
    public enum piece
    {
        pawn,
        knight,
        bishop,
        rook,
        queen,
        king
    };
    //string getCoords;
    Outline outline;
    public piece thisPiece;
    public Vector2 myCoords;
    Vector2 newCoords;
    public GameObject[] squares;

    int newX;
    int newY;
    string newsquare;
    GameObject chosen;

	// Use this for initialization
	void Start () 
    {
        if (thisPiece == piece.rook)
            Debug.Log("yes");
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnMouseOver()
    {
        ShowAvailableMoves();
    }

    void OnMouseOut()
    {
        chosen.GetComponent<Outline>().enabled = false;
    }

    void ShowAvailableMoves()
    {
        if (thisPiece == piece.pawn)
        {
            newX = (int)myCoords.x;
            newY = (int)myCoords.y+1;
            newsquare = (newX +","+ newY).ToString();
            chosen = GameObject.Find(newsquare);
            Debug.Log(newsquare);
            chosen.GetComponent<Outline>().enabled = true;

            newX = (int)myCoords.x;
            newY = (int)myCoords.y+2;
            newsquare = (newX +","+ newY).ToString();
            chosen = GameObject.Find(newsquare);
            Debug.Log(newsquare);
            chosen.GetComponent<Outline>().enabled = true;
        }
    }

    void MakeMove()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        string temp;
        string[] getCoords;
        if (other.gameObject.tag == "square")
        {
            Debug.Log("I'm touching you!");
            temp = other.gameObject.name;
            getCoords = temp.Split(',');
            myCoords = new Vector2(float.Parse(getCoords[0]), float.Parse(getCoords[1]));
        }
    }
}
