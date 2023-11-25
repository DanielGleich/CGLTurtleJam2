using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTracker : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI textmesh;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        textmesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
      
     //   textmesh.text = "Level: " + script_grid_script.currentLevel.ToString();
    }
}
