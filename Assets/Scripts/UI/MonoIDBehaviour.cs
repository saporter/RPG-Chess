using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoIDBehaviour : MonoBehaviour {
    private static int Counter = 0;
    private int objectID;

    public int ObjectID { get { return objectID; } }

    protected virtual void Awake()
    {
        objectID = Counter++;
    }
}
