using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script used on selected components. This technique adds the script to clicked on units instead of enabling a pre-existing script and destroys on DeselectAll().
public class SelectedComponent : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;

    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if(Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
                agent.SetDestination(hit.point);
        }
    }
}