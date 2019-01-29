using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solder : MonoBehaviour {

    public GameObject SolderEffect;
    public GameObject SolderHole;    

    private GameObject effect;
    private Vector3 instPos;


    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButtonDown(0) && GameManager.gm.enableSolder)
            {
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

        Instantiate(SolderHole, instPos, Quaternion.identity);
    }
}
