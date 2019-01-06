using UnityEngine;
using UnityEngine.UI;

public class DisplayUI : MonoBehaviour
{
    public Text myText;

    public string myString;
    public float fadeTime;
    public bool displayInfo;



    void Start()
    {
        // myText = GameObject.Find("Text").GetComponent<Text>();
        myText.color = Color.clear;
    }

    void Update()
    {
        FadeText();
    }

    void OnMouseOver()
    {
        displayInfo = true;
    }
    void OnMouseExit()

    {
        displayInfo = false;
    }

    void FadeText()

    {
        if (displayInfo)
        { 
            myText.text = myString;
            myText.color = Color.Lerp(myText.color, Color.black, fadeTime * Time.deltaTime);
        }
        else
        {
            myText.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
        }
    }
}