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
        if (Physics.Raycast(camera.transform.position, -Vector3.up, out hit, raycastDistance))
        {
            GameObject obj = hit.collider.gameObject;
            if(obj.CompareTag("HighlightLeft")){
                signArrow = 1;
            }
            else if(obj.CompareTag("HighlightRight")){
                signArrow = 2;
            }
            else if(obj.CompareTag("HighlightStraight")){
                signArrow = 3;
            }
            else if(obj.name.Contains("Highlight")){
                signArrow = 0;
            }
            else{
                signArrow = 4;
            }
        }
        else {
            signArrow = 4;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            isHit = false;
        }
    }
}
