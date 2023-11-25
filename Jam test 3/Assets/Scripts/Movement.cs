using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using System;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

enum Direction { UP, RIGHT, LEFT, DOWN };
public class Movement : MonoBehaviour
{
    [SerializeField] float gridStepSize = 50f;
    [SerializeField] float gridStepSpeed = 1f;
    [SerializeField] float smoothTime = .3f;
    [SerializeField] float speed = 10f;
    [SerializeField] Direction spawnDirection;

    public bool useEnergy = true;
    public int energy = 15;

    bool isMoving = false;
    bool isMovementPaused = false;
    bool isLevelRotating = false;
    bool isBlocked = false;

    Vector3 nextStepTarget = Vector3.zero;
    Vector2 moveDirection;
    Vector3 velocity;

    void Start()
    {
        StartCoroutine(PrepNextStep());
        int tempX = 0;
        int tempY = 0;
        switch (spawnDirection) {
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

    // Update is called once per frame
    void Update()
    {
        CheckNextStepForBlockage();
        if (Input.GetKeyDown(KeyCode.Space)) {
            isMovementPaused = !isMovementPaused;
        }

        if (!isMoving && !isMovementPaused && !isBlocked && !isLevelRotating &&( (useEnergy && energy > 0 ) || !useEnergy)) { 
            StartCoroutine(PrepNextStep());
            
        }
        if (!isLevelRotating)
        {
            transform.position = Vector3.SmoothDamp(transform.position, nextStepTarget, ref velocity, smoothTime, speed);       
        }
    }

    IEnumerator PrepNextStep() {
        isMoving = true;
        nextStepTarget = new Vector3(transform.position.x + (moveDirection.x * gridStepSize), transform.position.y + (moveDirection.y * gridStepSize), transform.position.z);
        yield return new WaitForSeconds(gridStepSpeed);
        if (useEnergy)
            energy--;
        isMoving = false;
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

    public void PauseForLevelRotation( bool clockwiseRotation ) {
        isLevelRotating = true;
        StopCoroutine(PrepNextStep());
        GameObject currentCell = GetCurrentCell();


        gameObject.transform.Rotate(0, 0, gameObject.transform.rotation.z + (clockwiseRotation ? 90 : -90));
        gameObject.transform.SetParent(currentCell.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Grid").transform, true);

        StartCoroutine(waitForLevelRotation(1.2f));
        
    }

    public void UnpauseForLevelRotation() {

    }

    GameObject GetCurrentCell() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, moveDirection);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.gameObject.tag == "GridCell") {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    IEnumerator waitForLevelRotation(float t) { 
        yield return new WaitForSeconds(t);
        gameObject.transform.parent = null;
        isLevelRotating = false;
    }
}
