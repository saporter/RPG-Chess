using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AWSDController : MonoBehaviour {
    public float speed = 1f;
	
	// Update is called once per frame
	void FixedUpdate () {
        float x = CrossPlatformInputManager.GetAxis("Horizontal");
        float z = CrossPlatformInputManager.GetAxis("Vertical");
        transform.position += new Vector3(x * speed, 0f, z * speed);
	}
}
