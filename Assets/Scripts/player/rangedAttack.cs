using UnityEngine;

public class rangedAttack : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    float projectileSpeed = 10f;
    [SerializeField]
    float shootCooldown = 0.25f;

    private float lastShootTime;

    private Transform playerTransform;
    Parry parry;
    private void Start()
    {
        parry = GetComponent<Parry>();
        playerTransform = GetComponent<Transform>();
    }
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        if (!parry.isParrying)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= lastShootTime + shootCooldown)
            {
                Shoot();
                lastShootTime = Time.time; 
            }
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (playerTransform.localScale.x < 0)
        {
            rb.linearVelocity = -firePoint.right * projectileSpeed;
            projectile.transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f); 
        }
        else
        {
            rb.linearVelocity = firePoint.right * projectileSpeed;
            projectile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f); 
        }
    }

}
