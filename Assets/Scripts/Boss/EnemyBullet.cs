using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f; 
    public float lifetime = 2f; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed; 
        Destroy(gameObject, lifetime); 
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Floor")) 
        {
            Destroy(gameObject); 
        }
    }
}
