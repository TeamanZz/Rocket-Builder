using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpaceShipMovement : MonoBehaviour
{
    public float defaultConstantVelocity = 6;
    public float defaultSideSpeed = 7;
    public float rotationDamping = 7;

    public float constantVelocity;
    public float sideSpeed;
    public Joystick joystick;
    public Transform shuttleModel;

    public Rigidbody spaceRB { get; private set; }
    private Quaternion startAngle;
    private Quaternion needAngle;

    [field: SerializeField] public Transform spawnEnemyPosition { get; private set; }

    [SerializeField] private Transform endEnemiesPosition;
    public static SpaceShipMovement Instance;

    private Tween velocityTween;
    private Tween sideSpeedTween;

    public bool playerCanControl = true;

    private void Awake()
    {
        Instance = this;
        spaceRB = GetComponent<Rigidbody>();
        startAngle = shuttleModel.rotation;
    }

    public void SetNewValues(float newMoveSpeedValue)
    {
        constantVelocity = defaultConstantVelocity + newMoveSpeedValue;
        sideSpeed = defaultSideSpeed + (newMoveSpeedValue * 1.1f);
    }

    public void SetZeroVariables()
    {
        velocityTween = DOTween.To(() => constantVelocity, x => constantVelocity = x, 0, 2f).SetEase(Ease.OutBack);
        sideSpeedTween = DOTween.To(() => sideSpeed, x => sideSpeed = x, 0, 2f).SetEase(Ease.OutBack);
    }

    private void FixedUpdate()
    {
        spaceRB.velocity = new Vector3(joystick.Horizontal * sideSpeed, constantVelocity, 0);
        if (!playerCanControl)
            return;
        Heeling();
    }

    private void Heeling()
    {
        if (joystick.Horizontal != 0)
        {
            needAngle = Quaternion.Euler(0, shuttleModel.rotation.eulerAngles.y, -joystick.Horizontal * 30);
            shuttleModel.rotation = Quaternion.Slerp(shuttleModel.rotation, needAngle, Time.deltaTime * rotationDamping);
        }

        if (joystick.Horizontal == 0)
        {
            needAngle = startAngle;
            shuttleModel.rotation = Quaternion.Slerp(shuttleModel.rotation, needAngle, Time.deltaTime * rotationDamping);
        }
    }
}