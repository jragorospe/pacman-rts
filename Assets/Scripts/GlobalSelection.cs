using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSelection : MonoBehaviour
{
    SelectedDictionary SelectedTable;
    RaycastHit hit;

    bool dragSelect;
    int groundMask;
    int selectableMask;

    //Collider Variables
    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

    //The Corners of our 2D Selection Box
    Vector2[] corners;

    //The Vertices of our Mesh Collider
    Vector3[] verts;
    Vector3[] vecs;

    // Start is called before the first frame update
    void Start()
    {
        SelectedTable = GetComponent<SelectedDictionary>();
        dragSelect = false;
        groundMask = LayerMask.GetMask("Ground");
        selectableMask = LayerMask.GetMask("Unit");
    }

    void Update() 
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            p1 = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            if((p1 - Input.mousePosition).magnitude > 40)
                dragSelect = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Single Click
            if(dragSelect == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(p1);

                if(Physics.Raycast(ray, out hit, 50000.0f, selectableMask))
                {
                    //Inclusive Select
                    if (Input.GetKey(KeyCode.LeftControl))
                        SelectedTable.AddSelected(hit.transform.gameObject);
                    else //Exclusive Select
                        StartCoroutine(ClearAllExcept(hit.transform.gameObject));
                }
                else //We didn't select anything
                {
                    if (Input.GetKey(KeyCode.LeftControl)) 
                    {}
                    else
                        SelectedTable.DeselectAll();
                }
            }
            else //Drag Select / Marquee Select
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                p2 = Input.mousePosition;
                corners = GetBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, 50000.0f, groundMask))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //Generate the Mesh
                selectionMesh = GenerateSelectionMesh(verts,vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftControl))
                    SelectedTable.DeselectAll();

               Destroy(selectionBox, 0.02f);

            }//End Marquee Select
            dragSelect = false;
        }
       
    }

    private void OnGUI()
    {
        if(dragSelect == true)
        {
            var rect = UIHelper.GetScreenRect(p1, Input.mousePosition);
            UIHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            UIHelper.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //Create a Bounding Box (4 Corners in Order) from the Start and End mouse position
    Vector2[] GetBoundingBox(Vector2 p1,Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //If p1 is to the left of p2
        {
            if (p1.y > p2.y) // If p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //If p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //If p1 is to the right of p2
        {
            if (p1.y > p2.y) //If p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //If p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }

    //Generate a mesh from the 4 bottom points
    Mesh GenerateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        //Map the Triangles of our Cube. We connect corners of our cube to make triangles in order to convert the cube into a mesh.
        //This is an arbitrary sequence the connects maps all triangles of the cube.
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        for(int i = 0; i < 4; i++)
            verts[i] = corners[i];

        for(int j = 4; j < 8; j++)
            verts[j] = corners[j - 4] + vecs[j - 4];

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        SelectedTable.AddSelected(other.gameObject);
    }

    //Since DeselectAll() calls onDestroy() for the SelectedComponent() script, onDestroy() takes priority and thus causes bugs in the
    //double click since it takes a frame to register. We simply add a 1 frame delay and deselect works perfectly.
    IEnumerator ClearAllExcept(GameObject go)
    {
        SelectedTable.DeselectAll();
        yield return 0;
        SelectedTable.AddSelected(go);
    }
}