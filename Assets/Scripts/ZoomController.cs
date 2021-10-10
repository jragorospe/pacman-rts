using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Camera does not actually move closer to the screen. This allows for expansion for air units that are higher than the camera. We simply change the field of view.
public class ZoomController : MonoBehaviour
{
    public Camera cam;

    public float minFOV;
    public float maxFOV;
    public float zoomRate;
    
    public float startingFOV;
    public float currentFOV;

    public bool isEnabled = true;

    void Start()
    {
        cam = GetComponent<Camera>();
        startingFOV = 25;
        cam.fieldOfView = startingFOV;
    }

    void Update()
    {
        HandleMouseInput(); 
        HandleKeyboardInput();
    }

    void HandleMouseInput()
    {
        currentFOV = cam.fieldOfView;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        currentFOV -= scroll * zoomRate * Time.deltaTime; //Try to smooth it out with Time.deltaTime but it will all depend on the mouse you use.
        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);
        cam.fieldOfView = currentFOV;
    }

    void HandleKeyboardInput()
    {
        //RTSs have a standard FOV and camera orientation. This resets it to that.
        if(Input.GetKey(KeyCode.R)) 
            cam.fieldOfView = 25;        
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
