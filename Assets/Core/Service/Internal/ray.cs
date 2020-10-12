using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray : MonoBehaviour
{
    // Start is called before the first frame update

    int layerMask = 1 << 8;
    public string focusLayer;

    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer(focusLayer);
        //layerMask = ~(1 << LayerMask.NameToLayer(focusLayer));  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float distance;
    public Vector3 diraction;
    void FixedUpdate()
    {
        //diration();
        between();
    }


    public Transform source;
    public Transform destination;
    void between() {

        RaycastHit hit;
        Vector3 fromPosition = source.transform.position;
        Vector3 toPosition = destination.transform.position;
        Vector3 direction = toPosition - fromPosition;


        if (Physics.Raycast(source.transform.position, direction, out hit, distance, layerMask))
        {
            //print("ray just hit the gameobject: " + hit.collider.gameObject.name);
            Debug.DrawRay(source.transform.position, direction * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(source.transform.position , direction * distance , Color.white);
            Debug.Log("Did not Hit");
        }
    }

    void diration() {
        // Bit shift the index of the layer (8) to get a bit mask
        //int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(diraction), out hit, distance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(diraction) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(diraction) * distance, Color.white);
            Debug.Log("Did not Hit");
        }
    }

}
