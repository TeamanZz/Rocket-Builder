using System;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float aimDistance;
    

    private void FixedUpdate()
    {
        if (EnemyManager.Instance.enemiesContainer.Count != 0)
        {
            for (int i = 0; i < EnemyManager.Instance.enemiesContainer.Count; i++)
            {
                var supposeTarget = EnemyManager.Instance.enemiesContainer[i];
                if (aimDistance >= supposeTarget.transform.position.y - transform.position.y)
                {
                    target = supposeTarget.transform;
                }
                else
                {
                    DismissTarget();
                }
            }
        }

        if (target != null)
        {
            if (aimDistance >= (target.position.y - transform.position.y))
            {
                LookAtTarget();
            }
        }
        else if(target == null || target.position.y < transform.position.y)
        {
            DismissTarget();
        }
    }

    private void LookAtTarget()
    {
        Vector3 difference = target.position - objectToRotate.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        objectToRotate.transform.rotation = Quaternion.Euler(0.0f, 180f, 90 - rotationZ);
    }

    private void DismissTarget()
    {
        target = null;
        objectToRotate.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
