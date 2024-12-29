using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingBoss : BossBase
{
    //public GameObject fireballPrefab;
    //public GameObject flameWavePrefab;

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

    [Header("Charge Attack Settings")]
    [SerializeField]
    Transform target;
    [SerializeField]
    float chargeDuration = 1f;
    [SerializeField]
    float chargeSpeed = 10f;
    private bool isCharging = false;



    protected override void InitializeAttacks()
    {
        phase1Attacks.Add(FeatherFall);
        phase1Attacks.Add(HomingProjectile);
        phase1Attacks.Add(Tornado);


        phase2Attacks.Add(ChargeAttack);
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
        Debug.Log("Called FeatherFall");
        if (featherPrefab == null)
        {
            return;
        }

        for (int i = 0; i < feathersPerWave; i++)
        {
            float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

            GameObject feather = Instantiate(featherPrefab, spawnPosition, Quaternion.identity);

        }
    }
}
