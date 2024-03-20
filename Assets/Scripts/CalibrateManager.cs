using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateManager : MonoBehaviour
{
    public GameObject controller; // Reference to the controller GameObject
    public Transform environmentToCalibrate; // Reference to the environment you want to calibrate
    private float trigger;

    private void Update()
    {
        RaycastHit hit;
        if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0f){
            if(Physics.Raycast(transform.position, transform.forward, out hit, 100)){
                GameObject obj = hit.collider.gameObject;
                if(obj.tag == "Button"){
                    Vector3 newPosition = environmentToCalibrate.position;
                    newPosition.y = controller.transform.position.y;
                    environmentToCalibrate.position = newPosition;
                    obj.SetActive(false);
                }
            }
        }
    }
}
