using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    GameObject item { 
        get {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop for " + gameObject.name);
        if(item == null)
        {
            Debug.Log("resetting parent for " + DragHandler.ItemBeingDragged.name);
            DragHandler.ItemBeingDragged.transform.SetParent(transform);
        }
    }
}
