using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Simple script to orbit camera during Main Menu sequence. Adds "animation" in conjuction with the unique skybox.
public class OrbitCamera : MonoBehaviour
{
    public float rotationAmount;
    public float movementTime;

    public Quaternion newRotation;

    void Start()
    {
        newRotation = transform.rotation;
    }

    void Update() 
    {
        newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }
}