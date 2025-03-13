using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] GameObject playerBullet;
    [SerializeField] Transform bulletSpawnPoint;
    private Parry parryScript;

    private void Start()
    {
        parryScript = GetComponentInParent<Parry>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);

            GameObject bullet = Instantiate(playerBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            ParriedBullet bulletScript = bullet.GetComponent<ParriedBullet>();

            if (bulletScript != null)
            {
                float direction = transform.root.localScale.x > 0 ? 1f : -1f;
                bulletScript.SetDirection(direction);
            }

            if (parryScript != null)
            {
                parryScript.SuccessfulParry();
            }
        }
    }
}
