using UnityEngine;

public class ParriedBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;

    Rigidbody2D rb;
    [SerializeField]
    int damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * bulletSpeed; 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            BossBase enemy = collision.gameObject.GetComponent<BossBase>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}

