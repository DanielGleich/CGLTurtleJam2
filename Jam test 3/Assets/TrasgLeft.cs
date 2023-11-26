using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrasgLeft : MonoBehaviour
{
    TrashManager trashman;
    TextMeshProUGUI tmg;
    void Start()
    {
        trashman = GameObject.FindGameObjectWithTag("Trashmanager").GetComponent<TrashManager>();
        tmg = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Object:" + trashman.trashLeft);
        Debug.Log("Trash left: " + trashman.trashLeft.ToString());
        tmg.text = "Trash Left: " + trashman.trashLeft.ToString();
    }
}
