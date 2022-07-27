using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanetsMovement : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;

    public float rotateValue;
    private void OnEnable()
    {
        rotateValue = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        transform.Rotate(0, 0, 30 * Time.deltaTime);
    }
}