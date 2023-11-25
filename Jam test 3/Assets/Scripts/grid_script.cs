using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class grid_script : MonoBehaviour
{
    public Vector2[] gridSize;
    public static int currentLevel=1;
    string rotatingDirection = "none";
    int rotationCounter = 0;
    float cellSize = 1;


    [SerializeField] GameObject Grid_Cell;

    void Start()
    {
        CreateGrid();
        CenterCameraOnObject();
        DontDestroyOnLoad(gameObject);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel++;
            CreateGrid();
            CenterCameraOnObject();
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

    void CenterCameraOnObject()
    {

        foreach (Transform child in transform)
        {
            child.gameObject.transform.position -= new Vector3((gridSize[currentLevel].x * cellSize) / 2 - cellSize/2, (gridSize[currentLevel].y * cellSize) / 2 - cellSize/2, child.gameObject.transform.position.z);
        }

       // Vector3 targetCenter = transform.position;
        //Camera.main.transform.position = new Vector3(targetCenter.x, targetCenter.y, transform.position.z);
    }
    void CreateGrid()
    {
        DeleteChildren();
       if(SceneManager.GetActiveScene().name != "Level " + currentLevel) SceneManager.LoadScene("Level " + currentLevel);
        cellSize = (10f / gridSize[currentLevel].x);
        

        for (int x = 0; x < gridSize[currentLevel].x; x++)
        {
            for (int y = 0; y < gridSize[currentLevel].y; y++)
            {
                Vector3 cellPosition = new Vector3(x*cellSize, y*cellSize, 0);
                GameObject cell = Instantiate(Grid_Cell, cellPosition, Quaternion.identity);
                cell.transform.parent = transform;
                cell.transform.localScale = new Vector3(cellSize, cellSize, 1);

            }
        }
    }
    void DeleteChildren()
    {
        // Iterate through all children and destroy them
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
