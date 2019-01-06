using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// A timer
/// </summary>
public class Timer : MonoBehaviour {

    Image fillImg;
    float timeAmt = 10.0f;
    float time;

    public Text timeText;
    public bool isRunning = false;
    public bool finished = false; 

    public float TimeAmt
    {
        get
        {
            return timeAmt;
        }

        set
        {
            timeAmt = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0.0 && isRunning)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / TimeAmt;
            timeText.text = time.ToString("F");
        }
        else
        {
            timeText.text = "0.00";
            isRunning = false;

            if (time <= 0.0f)
                finished = true; 
        }
    }

    public void Reset()
    {
        fillImg = this.GetComponent<Image>();
        time = TimeAmt;

        finished = false; 
    }
}