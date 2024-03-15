using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteArrow : MonoBehaviour
{
    public Transform targetObject; // bike/player
    public Transform destination;
    public float rotationSpeed = 5f;
    public float distanceFromBike = 3f; // distance from the bike's front

    // Update is called once per frame
    void LateUpdate()
    {
        // Calculate the target position based on the bike's position and distanceFromBike
        Vector3 targetPosition = targetObject.position + targetObject.forward * distanceFromBike;

        // Set the arrow's position to the target position
        transform.position = targetPosition;

        // Calculate the direction to the destination
        Vector3 toDestination = (destination.position - transform.position).normalized;

        // Calculate the target rotation based on the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(toDestination, targetObject.up);

        // Smoothly interpolate between the current rotation and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
