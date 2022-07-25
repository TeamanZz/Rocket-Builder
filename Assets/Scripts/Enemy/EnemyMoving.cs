using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyMoving : MonoBehaviour
{
    [Header("EnemySettings")]
    [SerializeField] private int enemySpeed;

    [SerializeField] private float enemySideSpeed;
    [SerializeField] private bool canMove;
    [SerializeField] private GameObject fireParticle;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject healthBarPrefab;

    [Header("EnemyTransforms")]
    [SerializeField] private Rigidbody enemyRb;

    [SerializeField] public Rigidbody playerRb;
    [SerializeField] private Vector3 playerPosition;


    private void Start()
    {
        playerRb = SpaceShipMovement.Instance.spaceRB;
        StartCoroutine(EnemyMove());
        StartCoroutine(AttackToPlayer());
        StartCoroutine(ScaleObject());
    }

    private void Update()
    {
        playerPosition = playerRb.gameObject.transform.position;
    }

    private void FixedUpdate()
    {

        if (canMove)
        {
            enemyRb.velocity = playerRb.velocity;
        }
        else
        {
            enemyRb.AddForce(Vector3.up * Time.fixedDeltaTime * enemySpeed);
        }
    }

    public IEnumerator ScaleObject()
    {
        transform.localScale = Vector3.one * 0.1f;
        yield return new WaitForSeconds(3f);
        transform.DOScale(1, 1f);
    }

    public IEnumerator EnemyMove()
    {
        canMove = true;
        yield return new WaitForSeconds(0.3f);
        canMove = false;
        yield return new WaitForSeconds(0.4f);
        yield return EnemyMove();
    }

    public IEnumerator AttackToPlayer()
    {
        yield return new WaitForSeconds(6f);
        fireParticle.SetActive(true);
        transform.DOMove(playerPosition, 1f);
        Destroy(gameObject, 9f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DamageToPlayer();
        }
    }

    public void DamageToPlayer()
    {
        StopCoroutine(EnemyMove());
        canMove = false;
        playerRb.gameObject.GetComponent<EnemyBase>().DescreaseHealth(5f);
        Instantiate(deathParticles, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
        Destroy(gameObject);
    }
}
