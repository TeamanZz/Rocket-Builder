using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMovement : MonoBehaviour
{
    // public float verticalForce;
    public float constantVelocity;
    public float rotationDamping;
    public int sideSpeed;

    private Rigidbody spaceRB;
    private Quaternion needAngle;
    public Joystick joystick;

    public Transform shuttleModel;

    private void Awake()
    {
        spaceRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        spaceRB.velocity = new Vector3(joystick.Horizontal * sideSpeed, constantVelocity, 0);

        // Vector3 direction = new Vector3(joystick.Horizontal * sideSpeed, constantVelocity, 0);
        // spaceRB.MovePosition(transform.position + new Vector3(joystick.Horizontal * sideSpeed, transform.position.y, 0) * Time.fixedDeltaTime);
        Heeling();

        // Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, -joystick.Horizontal * rotationDamping) * Time.fixedDeltaTime);
        // spaceRB.MoveRotation(spaceRB.rotation * deltaRotation);
    }

    private void Heeling()
    {
        if (joystick.Horizontal != 0)
        {
            needAngle = Quaternion.Euler(0, 180 + joystick.Horizontal * -30, 0);
        }

        if (joystick.Horizontal == 0)
        {
            needAngle = shuttleModel.rotation;
        }
        shuttleModel.rotation = Quaternion.Slerp(shuttleModel.rotation, needAngle, Time.deltaTime * rotationDamping);

    }
}