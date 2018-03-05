using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GameEventSystem : Singleton<GameEventSystem> {
    private bool manualDestroy = false;

    private void Awake()
    {
        if (Instance != this)
        {
            manualDestroy = true;       // Do not call Singleton OnDestroy()
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Event system options
    [System.Serializable]
    public class GameEvent : UnityEvent { }             // An event that does not require arguments
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
    [System.Serializable]
    public class NetworkIDEvent : UnityEvent<NetworkInstanceId> { }
    [System.Serializable]
    public class LocationEvent : UnityEvent<int, string> { }    // An event that occurs at a specific location on the board


    [SerializeField]
    public GameEvent TurnChanged;
    [SerializeField]
    public LocationEvent PromotionEvent;
    [SerializeField]
    public LocationEvent PieceAddedEvent;
    [SerializeField]
    public GameObjectEvent OnClick;
    [SerializeField]
    public GameObjectEvent SelectedPieceEvent;
    [SerializeField]
    public NetworkIDEvent MakePieceEvent;
    [SerializeField]
    public StringEvent LoadNewSceneEvent;

    public override void OnDestroy()
    {
        if (!manualDestroy)
            base.OnDestroy();
    }
}
