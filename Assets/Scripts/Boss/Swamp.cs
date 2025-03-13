using UnityEngine;

public class Swamp : MonoBehaviour
{
    [SerializeField]
    HPcontroll player;
    [SerializeField]
    private Transform[] teleportPoints;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.currentHealth--;
            int randomIndex = Random.Range(0, teleportPoints.Length); 
            collision.gameObject.transform.position = teleportPoints[randomIndex].position;
            if(player.currentHealth == 0)
            {
                player.HandleDeath();
            }
        }
    }
}
