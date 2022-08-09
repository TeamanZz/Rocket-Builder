using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float movementSpeed = 0.5f;
    public float movementSpeedIncrease = 0.1f;
    private float trueMovementSpeed;
    public bool canMove = false;
    private Vector3 startContainerPosition;

    private void Awake()
    {
        trueMovementSpeed = movementSpeed;
    }

    private void Start()
    {
        startContainerPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            transform.position += new Vector3(0, -1 * movementSpeed, 0);
        }
        else
        {
            return;
        }
    }

    public void ReturnContainerToStartPosition()
    {
        transform.position = startContainerPosition;
        movementSpeed = trueMovementSpeed;
        canMove = false;
    }

    public void SlowlyStopMoving()
    {
        canMove = false;
        transform.DOMoveY(transform.position.y - 10, 2);
    }

    public void EnableCurrentContainerMovement()
    {
        movementSpeed += (movementSpeedIncrease * GameStateHandler.Instance.countOfEngines);
        canMove = true;
    }
}