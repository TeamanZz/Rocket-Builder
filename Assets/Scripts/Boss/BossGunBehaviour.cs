using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossGunBehaviour : MonoBehaviour
{
    public List<BossGun> bossGuns = new List<BossGun>();
    public BossTargetGun bossTargetGun;
    [SerializeField] private int timeBetweenBossAttacks;
    private int countOfAttackingGuns;

    private void Start()
    {
        StartCoroutine(HowMuchGunsWillAttack());
    }

    private IEnumerator HowMuchGunsWillAttack()
    {
        yield return new WaitForSeconds(timeBetweenBossAttacks);
        countOfAttackingGuns = Random.Range(1, bossGuns.Count + 2);
        Debug.Log(countOfAttackingGuns);
        if (countOfAttackingGuns > bossGuns.Count)
        {
            BossTargetAttack();
        }
        else
        {
            BossAttack();   
        }
        yield return HowMuchGunsWillAttack();
    }
    
    private void BossAttack()
    {
        for (int i = 0; i < countOfAttackingGuns; i++)
        {
            bossGuns[i].StartCoroutine(bossGuns[i].OnceBossShot());
        }
    }

    public void BossTargetAttack()
    {
        bossTargetGun.StartCoroutine(bossTargetGun.OnceBossShot());
    }
}
