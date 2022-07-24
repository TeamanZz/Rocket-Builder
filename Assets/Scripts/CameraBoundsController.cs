using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundsController : MonoBehaviour
{
    public float boundReachLeft;
    public float boundReachRight;
    public float leftBound;
    public float rightBound;
    public CinemachineVirtualCamera cineCamera;

    private float cineXValue;
    private bool componentAdded = false;
    private CinemachineComposer transposer;

    private void Awake()
    {
        transposer = cineCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        // DoParralax();
        // StopTrackingSpaceShipPosition();
        // Debug.Log(transform.position.x);
    }

    private void DoParralax()
    {
        cineXValue = (transform.position.x - -5) / (5 + 5);
        transposer.m_ScreenX = Mathf.Clamp(Mathf.Lerp(transposer.m_ScreenX, cineXValue, Time.deltaTime), leftBound, rightBound);
    }

    private void StopTrackingSpaceShipPosition()
    {
        if ((transform.position.x > boundReachRight || transform.position.x < boundReachLeft) && componentAdded == false)
        {
            componentAdded = true;
            cineCamera.gameObject.AddComponent<LockCameraZ>().m_ZPosition = cineCamera.transform.position.x;
        }
        else if (((transform.position.x < boundReachRight && transform.position.x > boundReachLeft) || (transform.position.x > boundReachLeft && transform.position.x < boundReachRight)) && componentAdded == true)
        {
            componentAdded = false;
            Destroy(cineCamera.gameObject.GetComponent<LockCameraZ>());
        }
    }
}