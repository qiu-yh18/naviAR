using UnityEngine;

public class BikeController : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 80f;
    public GameObject city;
    public float speedGain = 0f;

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
            if (city != null)
            {
                city.transform.Translate(-speedGain*transform.forward * speed * Time.deltaTime, Space.World);
            }
        }
    }
}
