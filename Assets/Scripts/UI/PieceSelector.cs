using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelector : MonoBehaviour {

    public int BoardIndex;
    public Affiliation Team;
    public string ChooserType;
    public GameObject Chooser;
    public Transform ChooserLocation;

    public void Selected()
    {
        GameEventSystem.Instance.PromotionEvent.Invoke(BoardIndex, (Team == Affiliation.White ? "White" : "Black") + ChooserType);
        Chooser.transform.position = ChooserLocation.position;
    }
}
