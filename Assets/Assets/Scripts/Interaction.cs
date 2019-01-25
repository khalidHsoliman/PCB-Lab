using UnityEngine;

public class Interaction : MonoBehaviour {

    public Interaction[] anotherObjectsInteraction; 
  //  public GameObject highlightedObject;
  //  public Color highlightingColor;

    public float waitTime = 0.05f;
    public float offset   = 0.5f;

    private Color originalColor;
    private Color lerpedColor; 
    private Vector3 oldPos;
    private Vector3 newPos;
    private Quaternion oldRot;

    public bool isClicked = false; 

    private void Start()
    {
     //   if(highlightedObject)
     //       originalColor = highlightedObject.GetComponent<MeshRenderer>().sharedMaterial.color;

        oldPos = gameObject.transform.position;
        oldRot = gameObject.transform.rotation;
        newPos = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
    }

    private void Update()
    {
        if (isClicked)
        {
            gameObject.transform.position = newPos;

            // flashing
           // if (highlightedObject)
           // {
           //     lerpedColor = Color.Lerp(originalColor, highlightingColor, Mathf.PingPong(Time.time, waitTime));
           //     highlightedObject.GetComponent<MeshRenderer>().sharedMaterial.color = lerpedColor;
           // }
        }

        else
        {
            gameObject.transform.position = oldPos;
            gameObject.transform.rotation = oldRot; 
            
            // stop flashing 
            //if (highlightedObject)
            //    highlightedObject.GetComponent<MeshRenderer>().material.color = originalColor;
        }
    }

    private void OnMouseUp()
    {
        if (anotherObjectsInteraction.Length != 0)
        {
            for (int i = 0; i < anotherObjectsInteraction.Length; i++)
            {
                if (anotherObjectsInteraction[i].isClicked)
                {
                    if (gameObject.GetComponent<DoAction>())
                    {
                        gameObject.GetComponent<DoAction>().doAction(anotherObjectsInteraction[i].gameObject);
                    }

                    anotherObjectsInteraction[i].isClicked = false;
                }
            }

            if (!gameObject.CompareTag("Copper"))
                isClicked = !isClicked; 
        }

        else
            isClicked = !isClicked;
      
    }
}
