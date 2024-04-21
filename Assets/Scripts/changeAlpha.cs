using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeAlpha : MonoBehaviour
{
    public float alpha;
    public CircleRedirection circleRedirection;
    public Transform circleCenter;
    public PlayerCollider playerCollider;
    private Bounds objectBounds;

    // Start is called before the first frame update
    void Start()
    {
        // Renderer renderer = GetComponent<Renderer>();
        // objectBounds = renderer.bounds;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            circleRedirection.alpha = alpha;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
