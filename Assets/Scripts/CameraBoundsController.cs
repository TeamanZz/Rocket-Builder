using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundsController : MonoBehaviour
{
    public float boundValue;
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
        DoParralax();
        StopTrackingSpaceShipPosition();
    }

    private void DoParralax()
    {
        cineXValue = (transform.position.x - -5) / (5 + 5);
        transposer.m_ScreenX = Mathf.Clamp(Mathf.Lerp(transposer.m_ScreenX, cineXValue, Time.deltaTime), 0.45f, 0.55f);
    }

    private void StopTrackingSpaceShipPosition()
    {
        if ((transform.position.x > boundValue || transform.position.x < -boundValue) && componentAdded == false)
        {
            componentAdded = true;
            cineCamera.gameObject.AddComponent<LockCameraZ>().m_ZPosition = cineCamera.transform.position.x;
        }
        else if (((transform.position.x < boundValue && transform.position.x > -boundValue) || (transform.position.x > -boundValue && transform.position.x < boundValue)) && componentAdded == true)
        {
            componentAdded = false;
            Destroy(cineCamera.gameObject.GetComponent<LockCameraZ>());
        }
    }
}