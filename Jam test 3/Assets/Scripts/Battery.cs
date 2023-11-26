using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Battery : MonoBehaviour
{
    Movement roomba_mov;
    TextMeshProUGUI tmg;
    Transform batteryImage;
    void Start()
    {
        roomba_mov = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        tmg = GetComponent<TextMeshProUGUI>();
        batteryImage = gameObject.transform.parent.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!roomba_mov.useEnergy)
        {
            batteryImage.position = new Vector3(100, 100, 100);
        }
        else {
            if (roomba_mov.energy < 0) roomba_mov.energy = 0;

            Debug.Log("Object:" + roomba_mov);
            Debug.Log("Battery: " + roomba_mov.energy.ToString());
            tmg.text = "" + roomba_mov.energy.ToString();
        }
    }
}

