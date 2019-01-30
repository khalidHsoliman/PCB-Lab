using UnityEngine;
using System.Collections; 
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public GameObject ErrorButton;
    public GameObject RestartButton; 
    public GameObject UIGamePaused;
    public GameObject TimerObject;
    public GameObject IronDmg;
    public GameObject AcidDmg;

    public GameObject[] gameobjectsToReset;

    public Material IronDmgMat;
    public Material AcidDmgMat; 

    public Material newPcbMaterial;
    public Material newPcbeMaterial;


    public Text ErrorText; 
     
    public FirstPersonController FPSController;

    public bool enableDrill  = false;
    public bool enableSolder = false; 

    private GameObject _player;
    private Timer _timer;

    private bool isIron = false;
    private bool isAcid = false;

    private void Awake()
    {
        if (gm == null)
            gm = this.GetComponent<GameManager>();

        setupDefaults();
    }

    // game loop
    void Update()
    {

        // if ESC pressed then pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0f)
            {
                UIGamePaused.SetActive(true);
                Time.timeScale = 0f;
                FPSController.GetComponent<FirstPersonController>().enabled = false;
            }
            else
            {
                FPSController.GetComponent<FirstPersonController>().enabled = true;
                Time.timeScale = 1f;
                UIGamePaused.SetActive(false);
            }
        }

        if (_timer)
        {
            if (_timer.finished)
            {
                ShowErrorMessage("This is taking too much time!");

                ShowErrorButton();

                TimerReset();
            }
        }
    }


    // Timer helper Functions 

    public void TimerRun(float time = 10.0f, GameObject obj = null)
    {
        TimerObject.SetActive(true);

        _timer.TimeAmt = time;
        _timer.isRunning = true; 

        if(obj)
        {
            if (obj.tag == "Iron")
                isIron = true;

            else if (obj.tag == "PCB")
                isAcid = true;
        }
    }

    public void TimerReset()
    {
        _timer.Reset();

        TimerObject.SetActive(false);
    }

    public void TimerStop()
    {
        _timer.isRunning = false; 
    }

    public void TimerStart()
    {
        _timer.isRunning = true; 
    }

    public bool TimerFinished()
    {
        return _timer.finished;
    }

    // Error 
    
    public void ShowErrorMessage(string Message)
    {
        ErrorText.text = Message; 
        StartCoroutine("ShowMessage");
    }

    public void ShowErrorButton()
    {
        StartCoroutine("ShowButton"); 
    }

    public void DamagePCB()
    {
        for (int i = 0; i < gameobjectsToReset.Length; i++)
        {
            if (gameobjectsToReset[i])
            {
                gameobjectsToReset[i].GetComponent<Interaction>().enabled = true;
            }
        }

        if(isAcid)
        {
            AcidDmg.transform.GetChild(1).GetComponent<Renderer>().material = AcidDmgMat; 
        }

        if(isIron)
        {
            IronDmg.transform.GetChild(1).GetComponent<Renderer>().material = IronDmgMat; 
        }
    }

    public void ResetObject(GameObject gameObject)
    {
        if (gameObject.GetComponent<Interaction>()) 
            gameObject.GetComponent<Interaction>().enabled = true;

        if (gameObject.tag == "PCB")
        {
            gameObject.transform.GetChild(1).GetComponent<Renderer>().material = newPcbMaterial;
        }

        if (gameObject.tag == "PCBE")
        {
            gameObject.transform.GetChild(1).GetComponent<Renderer>().material = newPcbeMaterial;
        }
    }

    // Pause menu UI 

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
        {
            Application.Quit();
        }


    //

    void setupDefaults()
    {
        if(FPSController)
            FPSController.GetComponent<FirstPersonController>().enabled = true;

        if (TimerObject)
        {
            _timer = TimerObject.GetComponent<Timer>();
            TimerObject.SetActive(false);
        }

        if (ErrorText)
            ErrorText.enabled = false;

        if (ErrorButton)
            ErrorButton.SetActive(false); 

        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
            Debug.LogError("Player not found in Game Manager");
    }


    // coroutines 

    IEnumerator ShowMessage()
    {
        ErrorText.enabled = true;
        yield return new WaitForSeconds(3.0f);
        ErrorText.enabled = false; 
    }

    IEnumerator ShowButton()
    {
        ErrorButton.SetActive(true);

        while (ErrorButton.GetComponent<Image>().color.b >= 0 && ErrorButton.activeSelf)
        {
            ErrorButton.GetComponent<Image>().color -= new Color(0.0f, 0.01f, 0.01f, 0.0f);
            yield return new WaitForSeconds(0.05f);
        }

        if (ErrorButton.activeSelf)
        {
            ErrorButton.SetActive(false);
            ShowErrorMessage("PCB was damaged, Restart the procedure");
            DamagePCB();

            StartCoroutine("ShowRestartButton");
        }
    }

    IEnumerator ShowRestartButton()
    {
        yield return new WaitForSeconds(1.0f);

        RestartButton.SetActive(true); 
    }
}