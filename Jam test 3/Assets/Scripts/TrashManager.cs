using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public List<GameObject> trashList;
    public int trashLeft;
    void Start()
    {
        trashList = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Trash"))
        {
            trashList.Add(obj);
        }
        trashLeft = trashList.Count;
    }

    public void RemoveOneTrash(GameObject obj)
    {
        trashList.Remove(obj);
        trashLeft--;
    }
}
