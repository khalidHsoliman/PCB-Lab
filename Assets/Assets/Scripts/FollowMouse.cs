using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    private bool isClicked = false;

    private Vector3    oldPos;
    private Quaternion oldRot;
    
	// Use this for initialization
	void Start () {
        oldPos = transform.position;
        oldRot = transform.rotation; 
	}
	
	// Update is called once per frame
	void Update () {
		if(isClicked)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        else
        {
            transform.position = oldPos;
            transform.rotation = oldRot; 
        }
	}

    private void OnMouseUp()
    {
        isClicked = !isClicked;     
    }
}
