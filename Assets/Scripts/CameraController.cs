using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    void Update()
    {
        // Camera movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Camera rotation
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationInput = 1f;
        }

        transform.Rotate(Vector3.up, rotationInput * rotateSpeed * Time.deltaTime);
    }
}
