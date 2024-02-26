using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSignFollow : MonoBehaviour
{
    public Transform targetObject; // bike
    public float rotationSpeed = 5f;
    public Vector3 distanceFromBike; // distance from the bike's front

    void Start(){
        distanceFromBike = transform.position - targetObject.position;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        // Calculate the target position based on the bike's position and distanceFromBike
        Vector3 targetPosition = targetObject.position + distanceFromBike;

        // Set the arrow's position to the target position
        transform.position = targetPosition;
    }
}
