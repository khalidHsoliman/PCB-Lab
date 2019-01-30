using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour {

    public GameObject drillEffect;
    public GameObject blank; 
    public AudioClip  drillSFX;
    
    private AudioSource AudioSource;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    private GameObject effect;
    private Vector3 instPos; 

    // Use this for initialization
    void Start () {
        AudioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.gm.enableDrill)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit, 1000.0f))
                {
                    if(hit.transform.tag == "Copper")
                    {
                        instPos = hit.point;

                        instPos = new Vector3(instPos.x + 0.005f, instPos.y + 0.005f, instPos.z);

                        float vol = Random.Range(volLowRange, volHighRange);
                        AudioSource.PlayOneShot(drillSFX, vol);
                
                        effect = Instantiate(drillEffect, instPos, Quaternion.identity) as GameObject;
                        Destroy(effect, 3.5f);

                        StartCoroutine("instHole");
                    }
                }
            }
        }
	}

    IEnumerator instHole()
    {
        yield return new WaitForSeconds(1.5f);

        Instantiate(blank, instPos, Quaternion.identity);
    }
}
