using UnityEngine;

public class teleport : MonoBehaviour
{
    [SerializeField]
    Swordsman sword;
    [SerializeField]
    Transform placeToPort;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (sword.IsCharging)
            {
                sword.IsCharging = false;
                sword.direction *= -1;
                collision.gameObject.transform.position = placeToPort.position;
            }
        }
    }
}
