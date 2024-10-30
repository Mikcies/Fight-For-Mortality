using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    [SerializeField]
    internal float Walkspeed;

    [SerializeField]
    private float Jump;

    [SerializeField]
    private Transform Ray;

    [SerializeField]
    private LayerMask floorLayerMask;

    private bool isGrounded = false;

    internal bool canMove = true; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (canMove) 
        {
            Move();
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        }
    }

    private void Move()
    {
        isGrounded = (Physics2D.Raycast(Ray.position, Vector2.down, 0.5f, floorLayerMask).collider != null);

        float xMove = 0;
        float yMove = rb.velocity.y;

        if (Input.GetKey(KeyCode.D))
        {
            xMove = Walkspeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xMove = -Walkspeed;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            yMove = Jump;
        }

        if (xMove > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (xMove < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        rb.velocity = new Vector2(xMove, yMove);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 

    }
}
