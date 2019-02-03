using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solder : MonoBehaviour {

    public GameObject SolderEffect;
    public GameObject BurnedSolderHole; 
    public GameObject SolderHole;

    private GameObject effect;
    private GameObject hole; 

    private Vector3 instPos;

    private bool isheld   = false;
    private bool isburned = false;
    
    private float burnTime = 1.5f;
    private float SolderTime = 0.0f;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0) && GameManager.gm.enableSolder && GameManager.gm.isRotated)
            isheld = true;
        else 
            isheld = false; 

        if (Input.GetMouseButton(0) && GameManager.gm.enableSolder && !GameManager.gm.isRotated)
            GameManager.gm.ShowErrorMessage("You Need To Rotate the PCB Before start Soldering!");

        if(isheld)
        {
            SolderTime += Time.deltaTime;

            if (SolderTime > burnTime)
                isburned = true;

            else
                isburned = false;
        }

        else if (!isheld && GameManager.gm.enableSolder && SolderTime > 0.0f)
        {
            SolderTime = 0.0f;

            if (isburned)
                hole = BurnedSolderHole;
            else
                hole = SolderHole; 

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Copper")
                {
                    instPos = hit.point;

                    instPos = new Vector3(instPos.x + 0.065f, instPos.y - 0.005f, instPos.z);

                    effect = Instantiate(SolderEffect, instPos, Quaternion.Euler(-90.0f, 0.0f, 0.0f)) as GameObject;
                    Destroy(effect, 1.5f);

                    StartCoroutine("instHole");
                }
            }

        }
    }

    IEnumerator instHole()
    {
        yield return new WaitForSeconds(0.7f);

        Instantiate(hole, instPos, Quaternion.identity);
    }
}
