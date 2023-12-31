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

    //SOUND MANAGEMENT
    [SerializeField] SoundManager powerSoundManager;
    [SerializeField] SoundManager movementSoundManager;
    [SerializeField] SoundManager eatSoundManager;

    //ENERGY MANAGEMENT
    public bool useEnergy = true;
    public int energy = 15;

    //STATES
    public bool isMoving = false;
    public bool isMovementPaused = false;
    public bool isLevelRotating = false;
    public bool isBlocked = false;
    public bool isEverythingTidy = false;

    levelManager levelman;
    SpriteRenderer pauseIndicator;

    //DIMENSION INFORMATION
    public Vector3 nextStepTarget = Vector3.zero;
    public Vector2 moveDirection;
    Vector3 velocity;

    TrashManager trashManager;

    void Start()
    {
        pauseIndicator = GameObject.FindGameObjectWithTag("PauseIndicator").GetComponent<SpriteRenderer>();
        trashManager = GameObject.FindGameObjectWithTag("Trashmanager").GetComponent<TrashManager>();
        levelman = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        InitArrowDirection();
        StartCoroutine(MovePhase());
        powerSoundManager.PlayTurnOnSound();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Destroy(this.gameObject);
            isMovementPaused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        pauseIndicator.enabled = isMovementPaused;

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

            if (trashManager.trashLeft <= 0)
            {
               if(levelman.currentLevel<10) levelman.currentLevel++;
                SceneManager.LoadScene(levelman.currentLevel.ToString() + " LEVEL");
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

   public IEnumerator MovePhase() {
        CheckNextStepForBlockage();
        if (!isBlocked && energy > 0)
        {
            if (useEnergy)
            {
                energy--;
                if (energy <= 0)
                {
                    Die();
                }
            }
            movementSoundManager.PlayVariation();
            isMoving = true;
            SetNextStep();
            yield return new WaitForSeconds(movePhaseTime);
            
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
        powerSoundManager.PlayTurnOffSound();
    
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

    public void PlayEatSound()
    {
        eatSoundManager.PlayVariation();
    }

}
