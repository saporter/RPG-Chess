using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {
    public GameObject Highlights;
    public GameObject TestPiece;

    private GameObject[,] board;
    private int testx, testy;

    // Use this for initialization
	void Start () {
        board = new GameObject[8,8];
        //x = 0; y = 0;

        int i = 0;
        int j = 0;
        foreach(Outline h in Highlights.transform.GetComponentsInChildren<Outline>())
        {
            board[i, j] = h.gameObject;
            //Debug.Log(h.name);
            h.enabled = false;
            i = ++i % 8;
            j = i == 0 ? ++j % 8 : j;
        }

        testx = 4;
        testy = 4;
        TestPiece.transform.position = board[testx, testy].transform.position;
	}
	
	// Update is called once per frame
    //private float timer = 0f;
    //private Outline previous;
    //private int x, y;
    void Update () {
        //if(Time.time - timer > .25f){
        //    timer = Time.time;

        //    if(previous != null){
        //        previous.enabled = false;
        //    }
        //    previous = board[x, y].GetComponent<Outline>();
        //    previous.enabled = true;
        //    x = ++x % 8;
        //    y = x == 0 ? ++y % 8 : y;
        //}


        if (CrossPlatformInputManager.GetButtonUp("Horizontal"))
        {
            float input = CrossPlatformInputManager.GetAxis("Horizontal");
            if (input > 0 && testx < 7)
            {
                testx++;
            }
            else if (input < 0 && testx > 0)
            {
                testx--;
            }
        }

        if (CrossPlatformInputManager.GetButtonUp("Vertical"))
        {
            float input = CrossPlatformInputManager.GetAxis("Vertical");
            if (input < 0 && testy < 7)
            {
                testy++;
            }
            else if (input > 0 && testy > 0)
            {
                testy--;
            }
        }

        TestPiece.transform.position = board[testx, testy].transform.position;

	}

    public void HighlightTest()
    {
        AllOff();
        if (testx != 0)
        {
            board[testx - 1, testy].GetComponent<Outline>().enabled = true;
        }
        if (testx != 7)
        {
            board[testx + 1, testy].GetComponent<Outline>().enabled = true;
        }
        if (testy != 0)
        {
            board[testx, testy - 1].GetComponent<Outline>().enabled = true;
        }
        if (testy != 7)
        {
            board[testx, testy + 1].GetComponent<Outline>().enabled = true;
        }
    }

    public void Off()
    {
        AllOff();
    }

    private void CheckMouse()
    {
        AllOff();
        if (testx != 0)
        {
            board[testx - 1, testy].GetComponent<Outline>().enabled = true;
        }
        if (testx != 7)
        {
            board[testx + 1, testy].GetComponent<Outline>().enabled = true;
        }
        if (testy != 0)
        {
            board[testx, testy - 1].GetComponent<Outline>().enabled = true;
        }
        if (testy != 7)
        {
            board[testx, testy + 1].GetComponent<Outline>().enabled = true;
        }
    }

    private void AllOff()
    {
        foreach(GameObject go in board)
        {
            go.GetComponent<Outline>().enabled = false;
        }
    }
}
