using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Show and hides a UI GameObject with a CanvasGroup
 */ 
public class PopUpWindow : MonoBehaviour {
    [SerializeField]
    string PopUpType = "";

	// Use this for initialization
	private void Start () 
    {
        GameEventSystem.Instance.PromotionEvent.AddListener(popUpListener);
        toggleCanvasGroup(0f, false, false);

        // I hate this but it solves an unusual bug in the editor whenever the scene is reloaded
        var mask = transform.GetComponentInChildren<Mask>();
        if(mask != null)
        {
            mask.gameObject.SetActive(false);
            mask.gameObject.SetActive(true);
        }
	}

    private void popUpListener(int location, string type)
    {
        if(type.Contains(PopUpType))
        {
            toggleCanvasGroup(1f, true, true);
            moveWindow(location);
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

    /*
     * A lazy implementation that may not work with all screen resolutions
     */ 
    private void moveWindow(int location)
    {
        RectTransform rt = GetComponent<RectTransform>();
        float x = 30f + (location % 4) * 45f; 
        float y = (location / 4) > 0 ? -220f : -6.5f; 
        rt.anchoredPosition = new Vector3(x, y);
    }

    private void OnDestroy()
    {
        if (GameEventSystem.Instance != null)
        {
            GameEventSystem.Instance.PromotionEvent.RemoveListener(popUpListener);  // A good habit to get into
        }
    }
}
