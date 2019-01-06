using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson; 

public class DetectCollision : MonoBehaviour {

    public GameObject DetailsButton;
    public FirstPersonController FPSController; 

    private void Start()
    {
        if(DetailsButton)
            DetailsButton.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        FPSController.GetComponent<FirstPersonController>().enabled = false; 

        if (other.gameObject.CompareTag("Player"))
        {
            if (DetailsButton)
                DetailsButton.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        FPSController.GetComponent<FirstPersonController>().enabled = true; 

        if (other.gameObject.CompareTag("Player"))
        {
            if (DetailsButton)
                DetailsButton.SetActive(false);
        }
    }

}
