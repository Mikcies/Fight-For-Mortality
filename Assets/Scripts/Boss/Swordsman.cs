using System.Collections;
using UnityEngine;

public class Swordsman : BossBase
{
    [SerializeField]
    Transform Raycast;

    

    [Header("Charge")]
    float speed = 5f; 
    internal float direction = -1f; 
    internal bool IsCharging = false;

    [Header("Jump")]
    [SerializeField]
    float jumpForce;
    void Start()
    {
        base.Start();
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
    }

    protected override void InitializeAttacks()
    {
        //phase1Attacks.Add(Charge);
        phase1Attacks.Add(FallDown);
        phase1Attacks.Add(Jump);
        phase2Attacks.Add(FallDown);
    }

    private void FallDown()
    {
        if (IsOnFloor())
        {
            return;
        }
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
    }

    private bool IsOnFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(Raycast.position, Vector2.down, 1f);
        Debug.DrawLine(Raycast.position, Raycast.position + Vector3.down * 1f, Color.red, 15f);
        if (hit.collider != null && hit.collider.gameObject.name == "Floor")
        {
            return true;
        }
        return false;
    }
    private void Charge()
    {
        animator.SetBool("Charge", true);
        spriteRenderer.flipX = direction < 0;
        StartCoroutine(ChargeRoutine());
    }
    private IEnumerator ChargeRoutine()
    {
        IsCharging = true;
        while (IsCharging)
        {
            transform.position += Vector3.right * direction * speed * Time.deltaTime;
            yield return null;
        }
        animator.SetBool("Charge", false);
    }
    private void Jump()
    {
            Debug.Log("Boss skáče!");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertikální rychlosti
            rb.linearVelocity += new Vector2(0f, jumpForce); // Aplikace skoku
    }

}
