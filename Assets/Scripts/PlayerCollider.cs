using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public GameObject camera;
    public GameObject circleCenter;
    public int signArrow = 0; //0, show nothing. 1, turn left. 2, turn right. 3, go straight. 4, no further.
    public bool isHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.transform.position.x, circleCenter.transform.position.y, camera.transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Building"){
            isHit = true;
        }
        else if (collision.gameObject.name.Contains("Highlight")){
            if(collision.gameObject.tag == "HighlightLeft"){
                signArrow = 1;
            }
            else if(collision.gameObject.tag == "HighlightRight"){
                signArrow = 2;
            }
            else if(collision.gameObject.tag == "HighlightStraight"){
                signArrow = 3;
            }
            else{
                signArrow = 0;
            }
        }
        else{
            signArrow = 4;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Building"){
            isHit = false;
        }
    }
}
