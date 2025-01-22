using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField]
    Animator animator;
    private bool isGrounded = false;

    internal bool canMove = true;

    [SerializeField]
    GameObject winCanvas;
    [SerializeField]
    GameObject Hpcounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            rb.linearVelocity = Vector2.zero; 
            animator.enabled = false;
            return;
        }

        animator.enabled = true;
        if (canMove) 
        {
            Move();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Move()
    {
        isGrounded = (Physics2D.Raycast(Ray.position, Vector2.down, 0.5f, floorLayerMask).collider != null);

        float xMove = 0;
        float yMove = rb.linearVelocity.y;

        if (Input.GetKey(KeyCode.D))
        {
            xMove = Walkspeed;
            animator.SetBool("Run", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            xMove = Walkspeed;
            animator.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            xMove = -Walkspeed;
            animator.SetBool("Run", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            xMove = -Walkspeed;
            animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            yMove = Jump;
            animator.SetBool("IsJumping", true); 
            animator.SetBool("IsFalling", false); 
        }

        if (!isGrounded && yMove > 0)
        {
            animator.SetBool("IsJumping", true); 
            animator.SetBool("IsFalling", false);
        }

        if (!isGrounded && yMove <= 0)
        {
            animator.SetBool("IsJumping", false); 
            animator.SetBool("IsFalling", true);  
        }

        if (isGrounded)
        {
            animator.SetBool("IsJumping", false); 
            animator.SetBool("IsFalling", false); 
        }
        if (xMove > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (xMove < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        rb.linearVelocity = new Vector2(xMove, yMove);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Organ"))
        {
            winCanvas.SetActive(true);
            Hpcounter.SetActive(false);
            Destroy(collision.gameObject);
            Time.timeScale = 0;
        }
    }
}
