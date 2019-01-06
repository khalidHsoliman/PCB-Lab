﻿using UnityEngine;
using System.Collections; 
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public GameObject ErrorButton; 
    public GameObject UIGamePaused;
    public GameObject TimerObject; 

    public Text ErrorText; 
     
    public FirstPersonController FPSController;

    public bool enableDrill = false; 

    private GameObject _player;
    private Timer _timer;

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

        if(_timer.finished)
        {
            ShowErrorMessage("This is taking too much time!");

            ShowErrorButton();

            TimerReset();
        }
    }


    // Timer helper Functions 

    public void TimerRun(float time = 10.0f)
    {
        TimerObject.SetActive(true);

        _timer.TimeAmt = time;
        _timer.isRunning = true; 
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

    public void ResetObject(GameObject gameObject)
    {
        if (gameObject.GetComponent<Interaction>()) 
            gameObject.GetComponent<Interaction>().enabled = true;

        if (gameObject.tag == "PCB")
        {
            gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Metallic", 1.0f);
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
        yield return new WaitForSeconds(2.0f);
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
        }
    }
}