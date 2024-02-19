using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float inputX;
    private float inputZ;
    private float inputRotation; 

    public float moveSpeed = 0.5f;
    public float rotationSpeed = 90f;

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputRotation = Input.GetAxis("Rotation");
        inputZ = Input.GetAxis("Vertical");

        if (inputRotation != 0)
        {
            RotateCamera();
        }
        if (inputZ != 0)
        {
            MoveCameraFB();
        }
        if (inputX != 0)
        {
            MoveCameraLR();
        }
    }
    private void MoveCameraFB()
    {
        transform.position += transform.forward * inputZ * Time.deltaTime * moveSpeed;
    }
    private void MoveCameraLR()
    {
        transform.position += transform.right * inputX * Time.deltaTime * moveSpeed;
    }
    private void RotateCamera()
    {
        transform.Rotate(new Vector3(0f, inputRotation * Time.deltaTime * rotationSpeed, 0f));
    }
}
