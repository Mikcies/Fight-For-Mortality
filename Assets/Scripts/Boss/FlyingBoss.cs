using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingBoss : BossBase
{
    [SerializeField]
    GameObject heartObject;
    [SerializeField]
    Transform heartSpawnPoint;
    [Header("Tornado Settings")]
    [SerializeField]
    float tornadoForce;
    [SerializeField]
    GameObject tornadoPrefab;
    [SerializeField]
    Transform shootPoint;

    [Header("HomingProjectile")]
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    float fireRate = 1f;
    [SerializeField]
    int projectilesPerAttack = 3;
    private float fireTimer = 0f;

    [Header("FeatherStorm Settings")]
    [SerializeField]
    GameObject featherPrefab;
    [SerializeField]
    float featherSpawnRate = 0.2f;
    [SerializeField]
    int feathersPerWave = 10;
    [SerializeField]
    float spawnAreaWidth = 15f;
    [SerializeField]
    float spawnHeight = 10f;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Transform targetPosition;
    [SerializeField]
    float moveSpeed = 5f;
    Vector2 originalPosition;

    [Header("Movement Settings")]
    [SerializeField]
    Transform pointA;
    [SerializeField]
    Transform pointB;
    [SerializeField]
    float movementSpeed = 5f;
    private bool movingToPointB = true;



    protected override void InitializeAttacks()
    {
        phase1Attacks.Add(Fall);
        phase1Attacks.Add(HomingProjectile);
        phase1Attacks.Add(Tornado);
        phase1Attacks.Add(MoveBetweenPoints);

        phase2Attacks.Add(FeatherFall);
        phase2Attacks.Add(HomingProjectile);
        phase2Attacks.Add(Tornado);
        phase2Attacks.Add(MoveBetweenPoints);
        phase2Attacks.Add(FeatherFall2phase);
    }
    private void Start()
    {
        base.Start();
        originalPosition = transform.position;
    }
    private void ChargeAttack()
    {
    }
    private void HomingProjectile()
    {
        isAttacking = true;
        if (firePoint == null || projectilePrefab == null)
        {
            return;
        }

        for (int i = 0; i < projectilesPerAttack; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            if (projectile != null)
            {
                homingBall homingScript = projectile.GetComponent<homingBall>();
                if (homingScript != null)
                {
                    homingScript.InitializeTarget(); 
                }
            }
        }
        StartCoroutine(EndAttackAfterTime(1.5f));
    }
    private void Tornado()
    {
        isAttacking = true;
        GameObject tornado = Instantiate(tornadoPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody2D rb = tornado.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(-shootPoint.right * tornadoForce, ForceMode2D.Impulse);
        }
        StartCoroutine(EndAttackAfterTime(1.5f));
    }
    private void FeatherFall()
    {
        isAttacking = true;
        for (int i = 0; i < feathersPerWave; i++)
        {
            float randomX = Random.Range(spawnPoint.position.x - spawnAreaWidth / 2f, spawnPoint.position.x + spawnAreaWidth / 2f);
            Vector3 spawnPosition = new Vector3(randomX, spawnPoint.position.y, 0f);

            Quaternion rotation = Quaternion.Euler(0, 0, 180);

            GameObject feather = Instantiate(featherPrefab, spawnPosition, rotation);
        }
        StartCoroutine(EndAttackAfterTime(1.5f));
    }
    private void Fall()
    {
        if (targetPosition == null)
        {
            return;
        }
        StartCoroutine(Falle());
    }

    private IEnumerator Falle()
    {
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            Vector3 direction = (targetPosition.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null; 
        }

        transform.position = targetPosition.position;
        if (transform.position == targetPosition.position)
        {
            FeatherFall();
            yield return StartCoroutine(ReturnToOriginalPosition()); 
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            Vector2 direction = (originalPosition - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
            yield return null; 
        }

        transform.position = originalPosition;
    }


    private void FeatherFall2phase()
    {
        int Feather = Random.Range(10, 15);
        if (featherPrefab == null)
        {
            return;
        }
        for (int i = 0; i < Feather; i++)
        {
            float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

            GameObject feather = Instantiate(featherPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private void MoveBetweenPoints()
    {
        Transform targetPoint = movingToPointB ? pointB : pointA;
        if (targetPoint == null || pointA == null || pointB == null)
        {
            return;
        }

        StartCoroutine(MoveToTarget(targetPoint));

        movingToPointB = !movingToPointB;
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        while (Vector2.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
            yield return null;
        }
    }
    protected override void HandleDeath()
    {
        base.HandleDeath();
    }
    private IEnumerator EndAttackAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && isDead)
        {
            animator.enabled = false;
            Instantiate(heartObject, heartSpawnPoint);
        }
    }
}
