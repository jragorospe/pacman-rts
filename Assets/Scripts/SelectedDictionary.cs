using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used dictionary since it allowed navigation through data structure with having to interate through the entire thing after every 
//select and deselect. Every object has a unique ID which allows for this type of navigation.
public class SelectedDictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            go.AddComponent<SelectedComponent>();
            Debug.Log("Added " + id + " to Dictionary.");
        }
    }

    public void Deselect(int id)
    {
        Destroy(selectedTable[id].GetComponent<SelectedComponent>());
        selectedTable.Remove(id);
    }

    public void DeselectAll()
    {
        foreach(KeyValuePair<int,GameObject> pair in selectedTable)
        {
            if(pair.Value != null)
                Destroy(selectedTable[pair.Key].GetComponent<SelectedComponent>());
        }
        selectedTable.Clear();
    }
}