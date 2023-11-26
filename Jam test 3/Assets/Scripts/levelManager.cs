using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    public int currentLevel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (currentLevel < 10) currentLevel++;
            SceneManager.LoadScene(currentLevel.ToString() + " LEVEL");
        }
    }
}
