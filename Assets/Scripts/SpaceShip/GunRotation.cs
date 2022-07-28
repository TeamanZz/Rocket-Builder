using System;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Transform objectToRotate;
    public int speed;
    private void FixedUpdate()
    {
        Vector3 relativePos = target.transform.position - objectToRotate.position;
        Quaternion LookAtRotation = Quaternion.LookRotation(relativePos);

        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(objectToRotate.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y + 55, objectToRotate.rotation.eulerAngles.z);
        objectToRotate.rotation = Quaternion.Slerp(objectToRotate.rotation, LookAtRotationOnly_Y, 4 * Time.deltaTime);
    }

    /*private void LookAtTarget()
    {
        Vector3 direction = target.position - objectToRotate.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        objectToRotate.rotation = Quaternion.Euler(0,180,rotation.z * 10);
    }*/
}
