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
    public GameObject MoveCameraTo;
    public GameObject MoveSprayTo;
    public GameObject MoveShablonaTo; 
    public GameObject Spray;
    public GameObject LightMaterial;
    public GameObject Tapes;
    public GameObject MaterialAfterWash;
    public GameObject Layout;
    public GameObject Shablona;
    public GameObject SmokeEffect; 


    public FirstPersonController FirstPersonController; 

    public Material newMaterial;
    public Texture2D cursorTexture;

    public Text Width;
    public Text Height;
    public RawImage PCB;

    public AudioClip SpraySFX;
    public AudioClip ShakeSFX;
    public AudioClip DuctSFX;
    public AudioClip WaterSFX; 

    private AudioSource audioSource;

    private Light myLight; 

    private Vector3[] sprayPos;
    private Vector3 newPos;
    private Vector3 oldObjectPos;
    private Vector3 oldCameraPos;
    private Quaternion[] sprayRot;
    private Quaternion newRot;
    private Quaternion oldCameraRot;
    private Quaternion oldObjectRot; 
    
    
    private float width  = 1.0f;
    private float height = 1.0f;
    private float new_width  = 100.0f;
    private float new_height = 100.0f;

    private bool isRotating = false;
    private bool  paperOn = false; 
    private bool  acidOn  = false;
    private bool  PCBOn   = false;
    private bool  ductOn  = false; 

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

        if (Tapes)
            Tapes.SetActive(false);

        if (LightMaterial)
            LightMaterial.SetActive(false);

        if (MaterialAfterWash)
            MaterialAfterWash.SetActive(false);

        if (Layout)
            Layout.SetActive(false); 

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

        else if (gameObject.tag == "Duct")
            DuctShablona();

        else if (gameObject.tag == "Bucket")
            MaterialOnShablona();

        else if (gameObject.tag == "Ink")
            PrintPCB();
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
        newPos = new Vector3(0, 1.0f, 0);
        newRot = Quaternion.Euler(90.0f, 0, 0);

        paper.GetComponent<Interaction>().enabled = false; 

        if (newParent)
        {
            paper.transform.SetParent(newParent.transform);
            paper.transform.localPosition = newPos;
            paper.transform.localRotation = newRot;

            paperOn = true; 
        }
    }

    private void PrintLayout(GameObject iron)
    {
        if (paperOn)
        {
            iron.GetComponent<Interaction>().enabled = false;

            iron.transform.position = new Vector3(-9.0f, 2.7f, -2.0f);

            isRotating = true;
            StartCoroutine("Rotate", iron);
            StartCoroutine("Smoke"); 
            GameManager.gm.TimerRun(10.0f, iron);
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
            newPos = new Vector3(PCB.transform.position.x, PCB.transform.position.y - 0.5f, PCB.transform.position.z + 2.0f);

            PCB.GetComponent<Interaction>().enabled = false;

            PCB.transform.position = newPos;

            GameManager.gm.TimerRun(5.0f, PCB);

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

        Camera.main.transform.position = MoveCameraTo.transform.position;
        Camera.main.transform.rotation = MoveCameraTo.transform.rotation;

        StartCoroutine("ZoomIn");

        GameManager.gm.enableDrill = true;

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);


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

        GameManager.gm.enableDrill = false;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);


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

        PCB.transform.position = new Vector3(PCB.transform.position.x + 2, PCB.transform.position.y, PCB.transform.position.z);
        PCB.transform.Rotate(180, 0, 0);

        PCBOn = true; 

    }

    private void PlaceShablona(GameObject Shablona)
    {
        Shablona.GetComponent<Interaction>().enabled = false;

        if (MoveShablonaTo)
        {
            Shablona.transform.position = MoveShablonaTo.transform.position;
            Shablona.transform.rotation = MoveShablonaTo.transform.rotation;
        }
            //new Vector3(-16.27f, 2.78f, 16.31f);
        
        if(audioSource)
        {
            if (WaterSFX)
            {
                audioSource.PlayOneShot(WaterSFX);

                if(MaterialAfterWash)
                    MaterialAfterWash.SetActive(true);

                if(newMaterial)
                    Shablona.transform.GetChild(1).GetComponent<Renderer>().material = newMaterial;

                GameManager.gm.TimerRun(5.0f); 
            }
            
        }

        PCBOn = true; 
    }

    private void DuctShablona()
    {
        Tapes.SetActive(true);

        StartCoroutine("Duct");

        ductOn = true;
    }

    private void MaterialOnShablona()
    {
        if (ductOn)
            LightMaterial.SetActive(true);

        else
            GameManager.gm.ShowErrorMessage("You must put the duct tape first!"); 
    }

    private void PrintPCB()
    {
        Layout.SetActive(true);

        if (Shablona)
            Shablona.GetComponent<Interaction>().enabled = true; 
    }

    public void turnLightOn()
    {
        myLight = gameObject.GetComponentInChildren<Light>();
        if(myLight)
            myLight.intensity = 1;

        if (PCBOn)
        {
            GameManager.gm.TimerRun(8.0f);
        }

        else
            GameManager.gm.ShowErrorMessage("You should put the PCB first!");
    }

    public void turnLightOFF()
    {
        myLight = gameObject.GetComponentInChildren<Light>();
        if(myLight)
            myLight.intensity = 0;
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

    IEnumerator Smoke()
    {
        yield return new WaitForSeconds(10.0f);

        Instantiate(SmokeEffect, new Vector3(-8.3f, 2.7f, -1.36f), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
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

    IEnumerator Duct()
    {
        for (int i = 0; i < Tapes.gameObject.transform.childCount ; i++)
        {
            GameObject tape = Tapes.gameObject.transform.GetChild(i).gameObject;

            if (audioSource)
                audioSource.PlayOneShot(DuctSFX);

            yield return new WaitForSeconds(0.75f);

            tape.SetActive(true);

            yield return new WaitForSeconds(0.75f); 
        }

    }
}
