using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceTurnsToggle : MonoBehaviour {

	public void Toggle()
    {
        GameManager.Instance.EnforceTurns = !GameManager.Instance.EnforceTurns;
    }
}
