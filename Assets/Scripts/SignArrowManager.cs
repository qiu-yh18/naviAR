using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignArrowManager : MonoBehaviour
{
    public Camera mainCamera;
    public Material straightMaterial;
    public Material leftMaterial;
    public Material rightMaterial;
    public Material noMaterial;
    public GameObject highlights;
    private Image signArrowImage;
    private bool isAboveHighlight;
    // Start is called before the first frame update
    void Start()
    {
        signArrowImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        isAboveHighlight = false;
        foreach (Transform highlight in highlights.transform){
            Vector3 cameraPositionXZ = new Vector3(mainCamera.transform.position.x, 0f, mainCamera.transform.position.z);
            Bounds highlightBounds = highlight.GetComponent<Collider>().bounds;
            if (highlightBounds.Contains(cameraPositionXZ))
            {
                isAboveHighlight = true;
                if(highlight.tag == "HighlightStraight"){
                    signArrowImage.material = straightMaterial;
                }
                else if(highlight.tag == "HighlightLeft"){
                    signArrowImage.material = leftMaterial;
                }
                else{
                    signArrowImage.material = rightMaterial;
                }
            }
        }
        if(!isAboveHighlight){
            signArrowImage.material = noMaterial;
        }
    }
}
