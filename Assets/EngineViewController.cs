using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EngineViewController : MonoBehaviour
{
    private Vector3 startScale;
    public ParticleSystem trailParticle;

    private void Awake()
    {
        startScale = trailParticle.gameObject.transform.localScale;
    }

    public void IncreaseTrailEffectOnStart()
    {
        trailParticle.transform.localScale *= 4;
        DOTween.To(() => trailParticle.transform.localScale, x => trailParticle.transform.localScale = x, startScale, 4f).SetEase(Ease.OutBack);
    }
}