using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] GameObject playerBullet;
    [SerializeField] Transform bulletSpawnPoint;
    private Parry parryScript; // Reference na Parry skript na hráči

    private void Start()
    {
        parryScript = GetComponentInParent<Parry>(); // Předpokládáme, že Parry skript je na rodiči (hráč)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);

            Instantiate(playerBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            // Volání úspěšného parry, aby se zamezilo penalizaci
            if (parryScript != null)
            {
                parryScript.SuccessfulParry();
            }
        }
    }
}
