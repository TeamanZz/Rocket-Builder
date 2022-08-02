using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HumanRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 rotateValue = new Vector3(0, 0, Random.Range(-3600, 3600f));
        transform.DORotate(rotateValue, 60, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
}