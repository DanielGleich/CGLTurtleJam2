using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelHud : MonoBehaviour
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
        tmg.text = "Level: " + roomba_mov.currentLevel.ToString();
    }
}
