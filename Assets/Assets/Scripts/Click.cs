using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {

    public DoAction DoAction;

    private bool clicked = false; 

    private void OnMouseUp()
    {
        if (!clicked)
            DoAction.GetComponent<DoAction>().turnLightOn();

        else
            DoAction.GetComponent<DoAction>().turnLightOFF();

        clicked = !clicked;     
    }
}
