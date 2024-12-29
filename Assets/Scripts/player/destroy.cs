using UnityEngine;

public class destroy : MonoBehaviour
{
    [SerializeField]
    int damage;

    void OnCollisionEnter2D(Collision2D collision)
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
