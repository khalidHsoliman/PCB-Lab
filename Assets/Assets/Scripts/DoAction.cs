using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson; 

public class DoAction : MonoBehaviour {

    public GameObject resizePanel;
    public GameObject DrillPanel;
    public GameObject DetailsPanel; 
    public GameObject newParent;
    public GameObject Water;
    public GameObject moveCameraTo;
    public GameObject MoveSprayTo;
    public GameObject Spray;

    public FirstPersonController FirstPersonController; 

    public Material newMaterial;
    public Text Width;
    public Text Height;
    public RawImage PCB;

    public AudioClip SpraySFX;
    public AudioClip ShakeSFX;

    private AudioSource audioSource;

    private Light light; 

    private Vector3[] sprayPos;
    private Vector3 newPos;
    private Vector3 oldObjectPos;
    private Vector3 oldCameraPos;
    private Quaternion[] sprayRot;
    private Quaternion oldCameraRot;
    private Quaternion oldObjectRot; 
    
    private float width  = 1.0f;
    private float height = 1.0f;
    private float new_width  = 100.0f;
    private float new_height = 100.0f;

    private bool  paperOn = false; 
    private bool  isRotating = false;
    private bool  acidOn = false;
    private bool  PCBOn = false; 

    // private Material oldMaterial;

    private void Start()
    {
        if (Spray)
        {
            oldObjectPos = Spray.transform.position;
            oldObjectRot = Spray.transform.rotation;
        }


        // oldMaterial = gameObject.GetComponent<Renderer>().material;
        if (resizePanel)
            resizePanel.SetActive(false);

        if (DrillPanel)
            DrillPanel.SetActive(false);

        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // invoked doAction() function 

    public void doAction(GameObject gameObject)
    {
        if (gameObject.tag == "Cutter")
            ShowResizePanel();

        else if (gameObject.tag == "Cleaner")
            ChangeMaterial();

        else if (gameObject.tag == "Paper")
            PutPaper(gameObject);

        else if (gameObject.tag == "Iron")
            PrintLayout(gameObject);

        else if (gameObject.tag == "Acid")
            ChangeWaterColor();

        else if (gameObject.tag == "PCB")
            PutPCBonAcid(gameObject);

        else if (gameObject.tag == "Drill")
            StartDrill();

        else if (gameObject.tag == "Spray")
            StartSpray();

        else if (gameObject.tag == "PCBE")
            PlacePCB(gameObject);

        else if (gameObject.tag == "Shablona")
            PlaceShablona(gameObject);
    }

    // functions to be called inside the doAction to explicitly define that action or called by UI 

    public void Resize()
    {
        gameObject.transform.localScale = new Vector3(width, gameObject.transform.localScale.y, height);
    }

    private void ShowResizePanel()
    {
        if(resizePanel)
            resizePanel.SetActive(true);
    }
  
    private void ChangeMaterial()
    {
        if(newMaterial)
            gameObject.GetComponent<Renderer>().material = newMaterial;
    }

    private void PutPaper(GameObject paper)
    {
        newPos = new Vector3(0, 0.5f, 0);

        paper.GetComponent<Interaction>().enabled = false; 

        if (newParent)
        {
            paper.transform.SetParent(newParent.transform);
            paper.transform.localPosition = newPos;

            paperOn = true; 
        }
    }

    private void PrintLayout(GameObject iron)
    {
        if (paperOn)
        {
            iron.GetComponent<Interaction>().enabled = false;

            iron.transform.position = new Vector3(-5.0f, 2.7f, -2.0f);

            isRotating = true;
            StartCoroutine("Rotate", iron);

            GameManager.gm.TimerRun(10.0f);
        }

        else
        {
            GameManager.gm.ShowErrorMessage("Place the PCB Layout First!");
        }
    }

    private void ChangeWaterColor()
    {
        Water.GetComponent<Renderer>().material.SetColor("_TintColor", new Color32(34, 62, 34, 255));

        acidOn = true; 
    }

    private void PutPCBonAcid(GameObject PCB)
    {
        if (acidOn)
        {
            newPos = new Vector3(-11.5f, 2.342f, -2.5f);

            PCB.GetComponent<Interaction>().enabled = false;

            PCB.transform.position = newPos;

            GameManager.gm.TimerRun(10.0f);

        }

        else
        {
            GameManager.gm.ShowErrorMessage("Put the Acid on water First!");
        }
    }

    private void StartDrill()
    {

        oldCameraPos = Camera.main.transform.position;
        oldCameraRot = Camera.main.transform.rotation;

        FirstPersonController.GetComponent<FirstPersonController>().enabled = false;

        Camera.main.transform.position = moveCameraTo.transform.position;
        Camera.main.transform.rotation = moveCameraTo.transform.rotation;

        StartCoroutine("ZoomIn");

        GameManager.gm.enableDrill = true; 

        if (DrillPanel)
            DrillPanel.SetActive(true);

        if (DetailsPanel)
            DetailsPanel.SetActive(false);
    }

    public void StopDrill()
    {
        FirstPersonController.GetComponent<FirstPersonController>().enabled = true;

        Camera.main.transform.position = oldCameraPos;
        Camera.main.transform.rotation = oldCameraRot;

        StartCoroutine("ZoomOut");

        GameManager.gm.enableDrill = true;

        if (DrillPanel)
            DrillPanel.SetActive(false);

    }

    private void StartSpray()
    {
        Spray.GetComponent<Interaction>().enabled = false; 

        StartCoroutine("Spraying");
    }

    private void PlacePCB(GameObject PCB)
    {
        PCB.GetComponent<Interaction>().enabled = false;

        PCB.transform.position = new Vector3(-15.714f, 2.681f, 15.524f);
        PCB.transform.Rotate(180, 0, 0);

        PCBOn = true; 

    }

    private void PlaceShablona(GameObject Shablona)
    {
        Shablona.GetComponent<Interaction>().enabled = false;

        Shablona.transform.position = new Vector3(-16.27f, 2.78f, 16.31f);

        PCBOn = true; 
    }
    public void turnLightOn()
    {
        light = gameObject.GetComponentInChildren<Light>();
        if(light)
            light.intensity = 1;

        if (PCBOn)
        {
            GameManager.gm.TimerRun(10.0f);
        }

        else
            GameManager.gm.ShowErrorMessage("You should put the PCB first!");
    }

    public void turnLightOFF()
    {
        light = gameObject.GetComponentInChildren<Light>();
        if(light)
            light.intensity = 0;
    }

    // helper functions 

    public void getWidth()
    {
        if (float.TryParse(Width.text, out width))
        {
            width = checkBoundries(width);
            new_width = width;
            width /= 100.0f;
            PCB.rectTransform.sizeDelta = new Vector2(new_width, new_height);
        }
    }

    public void getHeight()
    {
        if (float.TryParse(Height.text, out height))
        {
            height = checkBoundries(height);
            new_height = height;
            height /= 100.0f;
            PCB.rectTransform.sizeDelta = new Vector2(new_width, new_height);
        }
    }

    private float checkBoundries(float num)
    {
        if (num > 100.0f)
            num = 100.0f;
        else if (num < 20.0f)
            num = 20.0f;

        return num; 
    }

    IEnumerator Rotate(GameObject gameObject)
    {
        while (isRotating)
        {
            if ((int)gameObject.transform.rotation.eulerAngles.z == 344)
            {
                isRotating = false;
            }
            else
            {
                gameObject.transform.Rotate(Vector3.forward, -5.0f);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    IEnumerator ZoomIn()
    {
        while(Camera.main.fieldOfView > 15)
        {
            Camera.main.fieldOfView -= 1.0f;
            yield return new WaitForSeconds(0.01f); 
        }
    }

    IEnumerator ZoomOut()
    {
        while (Camera.main.fieldOfView < 60)
        {
            Camera.main.fieldOfView += 1.0f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Spraying()
    {
        Spray.transform.position = MoveSprayTo.transform.position;
        Spray.transform.rotation = MoveSprayTo.transform.rotation; 

        for (int i = 0; i < 3; i++)
        {
            if (audioSource)
               audioSource.PlayOneShot(ShakeSFX);

            yield return new WaitForSeconds(1.5f);

            if (audioSource)
                audioSource.PlayOneShot(SpraySFX);

            yield return new WaitForSeconds(1.5f);

            gameObject.GetComponent<Renderer>().material.color -= new Color32(50, 20, 0, 0);


        }

        Spray.transform.rotation = oldObjectRot;
        Spray.transform.position = oldObjectPos;

        Spray.GetComponent<Interaction>().enabled = true;

    }
}
