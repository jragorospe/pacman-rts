using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float normalSpeed;
    public float fastSpeed;
    public float panSpeed;
    public float movementTime;
    public float rotationAmount;

    public bool isEnabled = true;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;


    void Start()
    {
        //Set position and rotation to current presets.
        newPosition = transform.position;
        newRotation = transform.rotation;
    }

    void Update() 
    {
        //Allows for pause handling.
        if(isEnabled)
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }
    }
    
    void HandleMouseInput()
    {
        //Checks for initial click and if the button is still held down.
        if(Input.GetMouseButtonDown(2))
            rotateStartPosition = Input.mousePosition;
        //Finds the difference in position and calculates the direction in 3 Dimensional space. Handles vertical and horizontal translation.
        if(Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5.0f));
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(Vector3.right * (difference.y / 35.0f));
        }
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            panSpeed = fastSpeed;
        else
            panSpeed = normalSpeed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            newPosition += (transform.forward * panSpeed);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            newPosition += (transform.forward * -panSpeed);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            newPosition += (transform.right * panSpeed);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            newPosition += (transform.right * -panSpeed);

        if(Input.GetKey(KeyCode.Q))
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        if(Input.GetKey(KeyCode.E))
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);

        //Reset to original FOV since RTSs have an optimal FOV.
        if(Input.GetKey(KeyCode.R))
            newRotation = Quaternion.Euler(new Vector3(0, 45, 0));

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }

    public void DisableInput()
    {
        isEnabled = false;
    }

    public void EnableInput()
    {
        isEnabled = true;
    }
}
