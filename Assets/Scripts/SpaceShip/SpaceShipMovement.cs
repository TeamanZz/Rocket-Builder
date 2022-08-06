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

    public static SpaceShipMovement Instance;

    private Tween velocityTween;
    private Tween sideSpeedTween;

    public bool playerCanControl = true;

    public float rocketTrueSpeed;
    public float newSpeed = 5;
    public float rotateZMultiplier = 5;
    public float rotateYMultiplier = 5;

    private bool fingerDown = false;

    private void Awake()
    {
        Instance = this;
        spaceRB = GetComponent<Rigidbody>();
        startAngle = shuttleModel.rotation;
    }

    public void SetNewValues(float newMoveSpeedValue)
    {
        sideSpeed = defaultSideSpeed + (newMoveSpeedValue * 1.1f);
        transform.DOScale(new Vector3(2.2f, 1.8f, 2), 0.5f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
        var velocitySeq = DOTween.Sequence();
        velocitySeq.Append(DOTween.To(() => constantVelocity, x => constantVelocity = x, defaultConstantVelocity * 2 + newMoveSpeedValue, 1f).SetEase(Ease.InBack));
        velocitySeq.Append(DOTween.To(() => constantVelocity, x => constantVelocity = x, defaultConstantVelocity + newMoveSpeedValue, 2f));
        rocketTrueSpeed = defaultConstantVelocity + newMoveSpeedValue;
    }

    public float GetTrueSpeed()
    {
        return rocketTrueSpeed;
    }

    public void SetZeroVariables()
    {
        velocityTween = DOTween.To(() => constantVelocity, x => constantVelocity = x, 0, 2f).SetEase(Ease.OutBack);
        sideSpeedTween = DOTween.To(() => sideSpeed, x => sideSpeed = x, 0, 2f).SetEase(Ease.OutBack);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            fingerDown = true;
        else
            fingerDown = false;
    }

    private void FixedUpdate()
    {
        spaceRB.velocity += new Vector3(0, constantVelocity - spaceRB.velocity.y, 0);
        if (!playerCanControl)
            return;

        var direction = Vector3.zero;
        if (fingerDown)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 24;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            var delta = mousePos - transform.position;
            var movementVector = delta.normalized * Time.deltaTime * newSpeed;
            spaceRB.MovePosition(transform.position + new Vector3(movementVector.x, 0, 0));

            Heeling(delta);
        }
        else
        {
            HeelingBack();
        }
    }

    private void Heeling(Vector3 delta)
    {
        needAngle = Quaternion.Euler(0, -delta.x * rotateYMultiplier, -delta.x * rotateZMultiplier);
        shuttleModel.rotation = Quaternion.Slerp(shuttleModel.rotation, needAngle, Time.deltaTime * rotationDamping);
    }

    private void HeelingBack()
    {
        needAngle = startAngle;
        shuttleModel.rotation = Quaternion.Slerp(shuttleModel.rotation, needAngle, Time.deltaTime * rotationDamping);
    }
}