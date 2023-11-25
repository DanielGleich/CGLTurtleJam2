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
        //CheckNextStepForBlockage();
        Debug.DrawLine(transform.position, moveDirection * gridStepSize * 2, Color.red);
        if (Input.GetKeyDown(KeyCode.Space)) {
            isMovementPaused = !isMovementPaused;
        }

        if (!isMoving && !isMovementPaused && !isBlocked && ( (useEnergy && energy > 0 ) || !useEnergy)) { 
            StartCoroutine(PrepNextStep());
            
        }
        transform.position = Vector3.SmoothDamp(transform.position, nextStepTarget, ref velocity, smoothTime, speed);       
    }

    IEnumerator PrepNextStep() {
        isMoving = true;
        nextStepTarget = new Vector3(transform.position.x + (moveDirection.x * gridStepSize), transform.position.y + (moveDirection.y * gridStepSize), transform.position.z);
        yield return new WaitForSeconds(gridStepSpeed);
        if (useEnergy)
            energy--;
        isMoving = false;
    }

    //void CheckNextStepForBlockage() {
    //    Debug.Log(moveDirection * gridStepSize);
    //    //Debug.Log(Physics2D.Raycast(GetComponent<Transform>().position, moveDirection * gridStepSize * 2));
    //    isBlocked = Physics2D.Raycast(GetComponent<Transform>().position, new Vector2(moveDirection.x * gridStepSize, moveDirection.y * gridStepSize));
    //}
}
