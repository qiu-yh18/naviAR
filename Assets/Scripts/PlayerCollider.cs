using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public GameObject camera;
    public GameObject circleCenter;
    public int signArrow = 0; //0, show nothing. 1, turn left. 2, turn right. 3, go straight. 4, no further.
    public bool isHit;
    private float raycastDistance = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.transform.position.x, circleCenter.transform.position.y, camera.transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, raycastDistance))
        {
            GameObject obj = hit.collider.gameObject;
            // Check if the object hit by the ray is tagged as "Highlight"
            if (obj.tag == "Highlight")
            {   
                if(obj.tag == "HighlightLeft"){
                    signArrow = 1;
                }
                else if(obj.tag == "HighlightRight"){
                    signArrow = 2;
                }
                else if(obj.tag == "HighlightStraight"){
                    signArrow = 3;
                }
                else{
                    signArrow = 0;
                }
            }
        }
        else {
            signArrow = 4;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Building"){
            isHit = true;
        }
        // else if (collision.gameObject.name.Contains("Highlight")){
        //     if(collision.gameObject.tag == "HighlightLeft"){
        //         signArrow = 1;
        //     }
        //     else if(collision.gameObject.tag == "HighlightRight"){
        //         signArrow = 2;
        //     }
        //     else if(collision.gameObject.tag == "HighlightStraight"){
        //         signArrow = 3;
        //     }
        //     else{
        //         signArrow = 0;
        //     }
        // }
        // else{
        //     signArrow = 4;
        // }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Building"){
            isHit = false;
        }
    }
}
