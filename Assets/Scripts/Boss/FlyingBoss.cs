using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingBoss : BossBase
{
    [SerializeField]
    Sprite deadOnGround;

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
        phase1Attacks.Add(FeatherFall);
        phase1Attacks.Add(HomingProjectile);
        phase1Attacks.Add(Tornado);
        phase1Attacks.Add(MoveBetweenPoints);

        phase2Attacks.Add(FeatherFall);
        phase2Attacks.Add(HomingProjectile);
        phase2Attacks.Add(Tornado);
        phase2Attacks.Add(MoveBetweenPoints);
        phase2Attacks.Add(FeatherFall2phase);
    }

    private void ChargeAttack()
    {
    }
    private void HomingProjectile()
    {

        if (firePoint == null || projectilePrefab == null)
        {
            return;
        }

        for (int i = 0; i < projectilesPerAttack; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            if (projectile != null)
            {
                homingFeather homingScript = projectile.GetComponent<homingFeather>();
                if (homingScript != null)
                {
                    homingScript.InitializeTarget(); 
                }
            }
        }
    }
    private void Tornado()
    {
        GameObject tornado = Instantiate(tornadoPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody2D rb = tornado.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(-shootPoint.right * tornadoForce, ForceMode2D.Impulse);
        }
    }
    private void FeatherFall()
    {
        if (featherPrefab == null || spawnPoint == null)
        {
            return;
        }

        for (int i = 0; i < feathersPerWave; i++)
        {
            float randomX = Random.Range(spawnPoint.position.x - spawnAreaWidth / 2f, spawnPoint.position.x + spawnAreaWidth / 2f);
            Vector3 spawnPosition = new Vector3(randomX, spawnPoint.position.y, 0f); // Y zůstává stejný

            Quaternion rotation = Quaternion.Euler(0, 0, 180);

            GameObject feather = Instantiate(featherPrefab, spawnPosition, rotation);
        }
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
        rb.gravityScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && isDead)
        {
            spriteRenderer.sprite = deadOnGround;
            animator.enabled = false;
        }
    }
}
