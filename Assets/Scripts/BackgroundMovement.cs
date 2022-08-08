using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float movementSpeed;
    public bool canMove = false;
    private Vector3 startContainerPosition;

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
        canMove = false;
    }

    public void SlowlyStopMoving()
    {
        canMove = false;
        transform.DOMoveY(transform.position.y - 10, 2);
    }

    public void EnableCurrentContainerMovement()
    {
        canMove = true;
    }
}