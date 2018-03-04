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

    private bool listenForClick = true;

    private void Start()
    {
        GameEventSystem.Instance.PromotionEvent.AddListener(pauseClicking);
        listenForClick = true;
    }

    private void pauseClicking(int location, string type)
    {
        listenForClick = type.Contains("Off");
    }

    private void OnMouseUp()
    {
        if (listenForClick)
        {
            OnClick.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        if (GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.PromotionEvent.RemoveListener(pauseClicking);  // A good habit to get into
        }
    }
}
