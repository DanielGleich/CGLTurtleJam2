using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

enum RotatingDirection {NONE, LEFT, RIGHT };

public class grid_script : MonoBehaviour
{
    public static int currentLevel = 1;

    [SerializeField] RotatingDirection lastRotationInput = RotatingDirection.NONE;
    [SerializeField] RotatingDirection currentInputProcess = RotatingDirection.NONE;
    [SerializeField] int rotationCounter = 0;

    GameObject roombaObject;
    TrashManager trashmanager;
    Movement roombaMovement;

    void Start()
    {
        trashmanager = GameObject.Find("Trashmanager").GetComponent<TrashManager>();
        roombaObject = GameObject.FindGameObjectWithTag("Player");
        roombaMovement = roombaObject.GetComponent<Movement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            lastRotationInput = RotatingDirection.LEFT;

        if (Input.GetKeyDown(KeyCode.RightShift))
            lastRotationInput = RotatingDirection.RIGHT;


        if (lastRotationInput != RotatingDirection.NONE && currentInputProcess == RotatingDirection.NONE && !roombaMovement.isMoving) { 
            currentInputProcess = lastRotationInput;
            roombaMovement.InitiateRotation(currentInputProcess == RotatingDirection.RIGHT);
        }

        if (trashmanager.trashLeft <=0 && Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel++;
            SceneManager.LoadScene("Level " + currentLevel.ToString());
        }
        
        if (currentInputProcess != RotatingDirection.NONE && roombaMovement.isLevelRotating)
        {
            RotateGrid();
        }
    }

    void RotateGrid() {
        int rotateAngle = currentInputProcess == RotatingDirection.LEFT ? 1 : -1;

        if (rotationCounter < 90)
        {
            transform.Rotate(0, 0, rotateAngle);
            rotationCounter++;
        }
        else
        {
            rotationCounter = 0;
            lastRotationInput = RotatingDirection.NONE;
            currentInputProcess = RotatingDirection.NONE;
            roombaMovement.FinishRotation();
        }
    }
}
