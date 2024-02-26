using UnityEngine;

public class BikeController : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 80f;
    public GameObject environment;

    // The relative speed of the environment moving in the opposite direction. 
    // If speedGain = 1, the environment moves the same speed as the bike,
    // making the virtual speed of the bike twice as the physical speed.
    public float speedGain = 1f;

    // The rotation offset of the environment relative to the bike.
    // If angleGain = 2, the environment rotates 2 degrees more than the bike.
    public float angleGain = 0f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Rotate the bike based on left and right arrow keys
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

        // Move the bike forward based on up arrow key
        if (verticalInput > 0)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // Move the city in the opposite direction of the bike's movement
            if (environment != null)
            {
                environment.transform.Translate(-speedGain*transform.forward * speed * Time.deltaTime, Space.World);
            }
        }
    }
}
