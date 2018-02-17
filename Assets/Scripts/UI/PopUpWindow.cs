using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpWindow : MonoBehaviour {
    [SerializeField]
    string PopUpType = "";

	// Use this for initialization
	private void Start () 
    {
        GameManager.Instance.PromotionEvent.AddListener(popUpListener);
        toggleCanvasGroup(0f, false, false);
	}

    private void popUpListener(int location, string type)
    {
        if(type.Contains(PopUpType))
        {
            toggleCanvasGroup(1f, true, true);
        }else
        {
            toggleCanvasGroup(0f, false, false);
        }
    }

    private void toggleCanvasGroup(float alpha, bool interactable, bool blocksRaycasts)
    {
        CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = alpha;
        cg.interactable = interactable;
        cg.blocksRaycasts = blocksRaycasts;
    }


    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PromotionEvent.RemoveListener(popUpListener);  // A good habit to get into
        }
    }
}
