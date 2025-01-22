using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{
    private int bounceCount = 0; 
    private int maxBounces = 3;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

        bounceCount++;

        if (bounceCount >= maxBounces)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 randomForce = new Vector2(Random.Range(-10f, 5f), Random.Range(1f, 10f)); 
        rb.AddForce(randomForce, ForceMode2D.Impulse);
    }
}
