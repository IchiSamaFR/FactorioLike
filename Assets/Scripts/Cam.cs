using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public string grabKey = "e";
    GameObject hitObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Check();
    }

    void Check()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 4))
        {
            //  Indicateur si l'objet peut être bougé
            if (hit.transform.gameObject.tag == "Movable" || hitObject != null)
            {
                HitMarker.instance.Change(new Color(180, 0, 0));
            } 
            else
            {
                HitMarker.instance.Change(new Color(255, 255, 255));
            }

            //  Mouvement de l'objet
            if (hit.transform.gameObject.tag == "Movable" && Input.GetKeyDown(grabKey))
            {
                /*
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                */
                hitObject = hit.transform.gameObject;
            }
        }
        else
        {
            HitMarker.instance.Change(new Color(255, 255, 255));
        }
    }
}
