using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class grid_script : MonoBehaviour
{
    public static int currentLevel=1;
    string rotatingDirection = "none";
    int rotationCounter = 0;


    [SerializeField] GameObject Grid_Cell;

    void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel++;
        }
        if (rotatingDirection == "none")
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                rotatingDirection = "left";
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                rotatingDirection = "right";
            }
        }
        if (rotatingDirection == "left")
        {
           if(rotationCounter<90)
            {
                transform.Rotate(0, 0, 1);
                rotationCounter++;
            }
            else
            {
                rotationCounter = 0;
                rotatingDirection = "none";
            }
        }

        if (rotatingDirection == "right")
        {
            if (rotationCounter < 90)
            {
                transform.Rotate(0, 0, -1);
                rotationCounter++;
            }
            else
            {
                rotationCounter = 0;
                rotatingDirection = "none";
            }
        }
    }
}
