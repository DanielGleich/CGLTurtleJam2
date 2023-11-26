using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battery : MonoBehaviour
{
    Movement roomba_mov;
    TextMeshProUGUI tmg;
    void Start()
    {
        roomba_mov = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        tmg = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roomba_mov.energy < 0) roomba_mov.energy = 0;
        {
            
        }
        tmg.text = "" + roomba_mov.energy.ToString();
    }
}

