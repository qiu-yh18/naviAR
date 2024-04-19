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
    }
}
