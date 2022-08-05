using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float movementSpeed;
    void FixedUpdate()
    {
        transform.position += new Vector3(0, -1 * movementSpeed, 0);
    }
}