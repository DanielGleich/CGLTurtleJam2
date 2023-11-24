using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using System;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using Unity.VisualScripting;

public class Movement : MonoBehaviour
{
    [SerializeField] float gridStepSize = 50f;
    [SerializeField] float gridStepSpeed = 1f;
    [SerializeField] float smoothTime = .3f;
    [SerializeField] float speed = 10f;

    public bool useEnergy = true;
    public int energy = 15;

    bool isMoving = false;
    bool isMovementPaused = false;

    Vector3 nextStepTarget = Vector3.zero;
    Vector3 velocity;

    void Start()
    {
        StartCoroutine(MoveStep());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            isMovementPaused = !isMovementPaused;
        }

        if (!isMoving && !isMovementPaused && ( (useEnergy && energy > 0 ) || !useEnergy)) { 
            StartCoroutine(MoveStep());
            
        }
        transform.position = Vector3.SmoothDamp(transform.position, nextStepTarget, ref velocity, smoothTime, speed);       
    }

    IEnumerator MoveStep() {
        isMoving = true;
        nextStepTarget = new Vector3(transform.position.x + gridStepSize, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(gridStepSpeed);
        if (useEnergy)
            energy--;
        isMoving = false;
    }
}
