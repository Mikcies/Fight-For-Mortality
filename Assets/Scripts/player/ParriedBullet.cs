using UnityEngine;

public class ParriedBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] int damage;
    private Rigidbody2D rb;
    private float direction;

    public void SetDirection(float dir)
    {
        direction = dir;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction; 
        transform.localScale = scale;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(bulletSpeed * direction, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            BossBase enemy = collision.gameObject.GetComponent<BossBase>();
            Enemy enemy1 = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                Debug.Log("Enemy hit by parried bullet");  
                enemy.TakeDamage(damage);
            }
            if (enemy1 != null)
            {
                Debug.Log("Enemy hit by parried bullet");
                enemy1.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
