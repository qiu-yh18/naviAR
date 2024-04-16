using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeAlpha : MonoBehaviour
{
    public float alpha;
    public CircleRedirection circleRedirection;
    public Transform circleCenter;
    public Transform player;
    private Bounds objectBounds;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        objectBounds = renderer.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, circleCenter.position.y, player.position.z);
        if (objectBounds.Contains(playerPos)){
            circleRedirection.alpha = alpha;
        }
    }
}
