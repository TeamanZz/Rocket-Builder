using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 rotateValue = new Vector3(Random.Range(-3600, 3600f), Random.Range(-3600, 3600f), Random.Range(-3600, 3600f));
        Debug.Log(rotateValue);
        transform.DORotate(rotateValue, 60, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }

}
