using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using System;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System.Linq;
using System.Transactions;
using UnityEngine.SceneManagement;

enum Direction { UP, RIGHT, LEFT, DOWN };
public class Movement : MonoBehaviour
{
    //MOVEMENTPARAMETERS
    [SerializeField] float gridStepSize = 50f;
    [SerializeField] float smoothTime = .3f;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] Direction spawnDirection;

    [SerializeField] float movePhaseTime = .5f;
    [SerializeField] float waitPhaseTime = .5f;

    //ENERGY MANAGEMENT
    public bool useEnergy = true;
    public int energy = 15;

    //STATES
    public bool isMoving = false;
    public bool isMovementPaused = false;
    public bool isLevelRotating = false;
    public bool isBlocked = false;
    public bool isEverythingTidy = false;

    public int currentLevel = 1;

    //DIMENSION INFORMATION
    public Vector3 nextStepTarget = Vector3.zero;
    public Vector2 moveDirection;
    Vector3 velocity;

    TrashManager trashManager;

    void Start()
    {
        trashManager = GameObject.FindGameObjectWithTag("Trashmanager").GetComponent<TrashManager>();
        InitArrowDirection();
        StartCoroutine(MovePhase());
    }

    void Update()
    {
        // PAUSE PLAYER MOVEMENT
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(trashManager.trashLeft > 0)
            {
                isMovementPaused = !isMovementPaused;

                if (!isMovementPaused)
                {
                    StartCoroutine(MovePhase());
                }
            }
            if (energy <= 0 && trashManager.trashLeft > 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (trashManager.trashLeft <= 0)
            {
                currentLevel++;
                SceneManager.LoadScene(currentLevel.ToString() + " LEVEL");
            }
        }

        // Roomba does not move when room is rotating
        if (!isLevelRotating && isMoving && !isMovementPaused && !isBlocked && !isEverythingTidy && ((useEnergy && energy > 0) || !useEnergy))
        {
            transform.position = Vector3.SmoothDamp(transform.position, nextStepTarget, ref velocity, smoothTime, movementSpeed);       
        }
    }

    void CheckNextStepForBlockage()
    {
        Vector2 origin = GetComponent<Transform>().position;
        isBlocked = false;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, moveDirection, gridStepSize);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.gameObject.layer == 31) { 
                isBlocked = true;
            }
        }
    }

    public void InitiateRotation( bool clockwiseRotation ) {
        isLevelRotating = true;

        gameObject.transform.Rotate(0, 0, gameObject.transform.rotation.z + (clockwiseRotation ? 90 : -90));
        CenterRoomba();
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Grid").transform);
    }

    IEnumerator MovePhase() {
        CheckNextStepForBlockage();
        isMoving = true;
        SetNextStep();
        yield return new WaitForSeconds(movePhaseTime);
        if (useEnergy && energy > 0) { 
            energy--;
            if (energy <= 0)
            {
                Die();
            }
        }
        CheckLevelEndCondition();
        if (!isMovementPaused)
            StartCoroutine(WaitPhase());
    }

    IEnumerator WaitPhase()
    {
        isMoving = false;
        yield return new WaitForSeconds(waitPhaseTime);
        StartCoroutine(MovePhase());

    }

    void CheckLevelEndCondition()
    {
        if (trashManager.trashLeft == 0)
        {
            isEverythingTidy = true;
        }
    }

    public void FinishRotation()
    {
        isLevelRotating = false;
        SetNextStep();
    }


    void SetNextStep()
    {
        nextStepTarget = new Vector3(transform.position.x + (moveDirection.x * gridStepSize), transform.position.y + (moveDirection.y * gridStepSize), transform.position.z);
    }

    GameObject GetCurrentCell() {
    //Returns cells that are in front of the roomba
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, moveDirection);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.gameObject.tag == "GridCell") {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    void Die() {
    
    }


    void InitArrowDirection()
    {
        int tempX = 0;
        int tempY = 0;
        switch (spawnDirection)
        {
            case Direction.UP:
                tempX = 0;
                tempY = 1;
                transform.Rotate(0, 0, 90);
                break;

            case Direction.RIGHT:
                tempX = 1;
                tempY = 0;
                transform.Rotate(0, 0, 0);
                break;

            case Direction.DOWN:
                tempX = 0;
                tempY = -1;
                transform.Rotate(0, 0, 270);
                break;

            case Direction.LEFT:
                tempX = -1;
                tempY = 0;
                transform.Rotate(0, 0, 180);
                break;
        }
        moveDirection = new Vector2(tempX, tempY);
    }

    void CenterRoomba() {
        GameObject currentCell = GetCurrentCell();
        gameObject.transform.SetParent(currentCell.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.SetParent(null);
    }
}
