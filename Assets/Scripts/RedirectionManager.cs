using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectionManager : MonoBehaviour
{
    public GameObject environment;
    public GameObject player;
    private Vector3 oldPlayerPos;
    private Vector3 movement;
    public float movementThreshold = 0.01f;

    // The relative speed of the environment moving in the opposite direction. 
    // If speedGain = 1, the environment moves the same speed as the player,
    // making the virtual speed of the bike twice as the physical speed.
    public float speedGain = 1f;

    // The rotation offset of the environment relative to the player.
    // If angleGain = 0.5, the environment rotates around the player for 0.5*movement.magnitude degrees.
    public float angleGain = 0.5f;

    private void Start(){
        oldPlayerPos = player.transform.position;
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            // Calculate user movement between frames
            movement = player.transform.position - oldPlayerPos;
            // Ignore movement along y-axis
            movement[1] = 0.0f;
            if(movement.magnitude > movementThreshold){
                // Move the city in the opposite direction of the bike's movement
                if (environment != null)
                {
                    if(angleGain > 0){
                        // environment.transform.RotateAround(transform.position, Vector3.up, -angleGain * horizontalInput * rotationSpeed * Time.deltaTime);
                        // environment.transform.Rotate(Vector3.up, -0.2f*angleGain * speedGain * speed * Time.deltaTime);
                        environment.transform.RotateAround(player.transform.position, Vector3.up, -angleGain*movement.magnitude);
                    }
                    if(speedGain > 0){
                        environment.transform.Translate(-speedGain * movement, Space.World);
                    }
                }
                oldPlayerPos = player.transform.position;
            }
        }
    }
}
