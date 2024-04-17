using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignArrowManager : MonoBehaviour
{
    public PlayerCollider player; //0, show nothing. 1, turn left. 2, turn right. 3, go straight. 4, no further.
    public Material straightMaterial;
    public Material leftMaterial;
    public Material rightMaterial;
    public Material noMaterial;
    public GameObject highlights;
    private Image signArrowImage;
    // private bool isAboveHighlight;
    // Start is called before the first frame update
    void Start()
    {
        signArrowImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.signArrow == 0){
            signArrowImage.material = null;
        }
        else if(player.signArrow == 1){
            signArrowImage.material = leftMaterial;
        }
        else if(player.signArrow == 2){
            signArrowImage.material = rightMaterial;
        }
        else if(player.signArrow == 3){
            signArrowImage.material = straightMaterial;
        }
        else{
            signArrowImage.material = noMaterial;
        }
        
        // isAboveHighlight = false;
        // foreach (Transform highlight in highlights.transform){
        //     Vector3 cameraPositionXZ = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
        //     Bounds highlightBounds = highlight.GetComponent<Collider>().bounds;
        //     if (highlightBounds.Contains(cameraPositionXZ))
        //     {
        //         isAboveHighlight = true;
        //         if(highlight.tag == "HighlightStraight"){
        //             signArrowImage.material = straightMaterial;
        //         }
        //         else if(highlight.tag == "HighlightLeft"){
        //             signArrowImage.material = leftMaterial;
        //         }
        //         else{
        //             signArrowImage.material = rightMaterial;
        //         }
        //     }
        // }
        // if(!isAboveHighlight){
        //     signArrowImage.material = noMaterial;
        // }
    }
}
