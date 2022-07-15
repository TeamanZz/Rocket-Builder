using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMovement : MonoBehaviour
{
    public float constantVelocity;
    public float rotationDamping;
    public int sideSpeed;
    public Joystick joystick;
    public Transform shuttleModel;

    private Rigidbody spaceRB;
    private Quaternion startAngle;
    private Quaternion needAngle;

    private void Awake()
    {
        spaceRB = GetComponent<Rigidbody>();
        startAngle = shuttleModel.rotation;
    }

    private void FixedUpdate()
    {
        spaceRB.velocity = new Vector3(joystick.Horizontal * sideSpeed, constantVelocity, 0);
        Heeling();
    }

    private void Heeling()
    {
        if (joystick.Horizontal != 0)
        {
            needAngle = Quaternion.Euler(0, 180 + joystick.Horizontal * -15, joystick.Horizontal * 30);
            shuttleModel.rotation = Quaternion.Slerp(shuttleModel.rotation, needAngle, Time.deltaTime * rotationDamping);
        }

        if (joystick.Horizontal == 0)
        {
            needAngle = startAngle;
            shuttleModel.rotation = Quaternion.Slerp(shuttleModel.rotation, needAngle, Time.deltaTime * rotationDamping);
        }
    }
}