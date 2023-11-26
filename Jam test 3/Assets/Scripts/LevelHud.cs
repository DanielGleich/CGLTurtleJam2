using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelHud : MonoBehaviour
{
    levelManager levelman;
    TextMeshProUGUI tmg;
    void Start()
    {
        levelman = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        tmg = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Object:" + levelman);
        Debug.Log("Level: " + levelman.currentLevel.ToString());
        tmg.text = "Level: " + levelman.currentLevel.ToString();
    }
}
