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

            Instantiate(playerBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            if (parryScript != null)
            {
                parryScript.SuccessfulParry();
            }
        }
    }
}
