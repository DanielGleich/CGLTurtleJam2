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
    TrashManager trashmananger;
    GameObject roombaObject;
    [SerializeField] private float rotateCooldown;
    float cooldown;

    [SerializeField] GameObject Grid_Cell;

    void Start()
    {
        trashmananger = GameObject.Find("Trashmanager").GetComponent<TrashManager>();
        roombaObject = GameObject.FindGameObjectWithTag("Player");
    }
    private void Awake()
    {
    }
    private void Update()
    {

        if (trashmananger.trashLeft <=0 && Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel++;
            SceneManager.LoadScene("Level " + currentLevel.ToString());
        }
        if (rotatingDirection == "none")
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && cooldown <= 0f && rotatingDirection!="right")
            {
                roombaObject.GetComponent<Movement>().PauseForLevelRotation(false);
                rotatingDirection = "left";
                cooldown = rotateCooldown;
            }
            if (Input.GetKeyDown(KeyCode.RightShift) && cooldown <= 0f && rotatingDirection != "left")
            {
                roombaObject.GetComponent<Movement>().PauseForLevelRotation(true);
                rotatingDirection = "right";
                cooldown = rotateCooldown;
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
        cooldown -= Time.deltaTime;
    }
}
