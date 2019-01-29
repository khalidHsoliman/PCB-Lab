using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private bool isClicked = false;
   
	
	// Update is called once per frame
	void Update () {
        if (isClicked)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    private void OnMouseUp()
    {
        isClicked = !isClicked;

        GameManager.gm.enableSolder = !GameManager.gm.enableSolder;

    }
}
