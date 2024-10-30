using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int maxHealth;
    private int currentHealth;
    SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;

    // New variables for shooting
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform firePoint; // Point from which the bullet will be fired
    public float shootInterval = 5f; // Time interval between shots

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ShootRoutine()); // Start the shooting routine
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        // animator.SetTrigger("Die");

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        Destroy(gameObject);
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval); 
        }
    }

    // Method to handle shooting
    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); 
        }
    }
}
