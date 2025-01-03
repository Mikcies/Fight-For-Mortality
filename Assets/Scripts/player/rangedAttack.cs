﻿using UnityEngine;

public class rangedAttack : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    float projectileSpeed = 10f;

    private Transform playerTransform;
    Parry parry;
    private void Start()
    {
        parry = GetComponent<Parry>();
        playerTransform = GetComponent<Transform>();
    }
    void Update()
    {
        if (!parry.isParrying)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0)) 
            {
            Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (playerTransform.localScale.x < 0)
        {
            rb.velocity = -firePoint.right * projectileSpeed;
        }
        else
        {
            rb.velocity = firePoint.right * projectileSpeed;
        }

    }

}
