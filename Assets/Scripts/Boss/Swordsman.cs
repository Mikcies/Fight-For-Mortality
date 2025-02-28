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

    [Header("Shockwave")]
    [SerializeField]
    GameObject shockwavePrefab;
    [SerializeField]
    Transform shootPoint;
    [SerializeField]
    float shockwaveSpeed = 5f;

    [Header("Counter")]
    [SerializeField]
    float blockDuration = 1f;
    bool isBlocking = false;
    [SerializeField]
    GameObject counterProjectilePrefab;
    [SerializeField]
    Transform counterShootPoint;
    [SerializeField]
    float counterProjectileSpeed = 5f;


    [Header("Death")]
    [SerializeField]
    GameObject lungObject;
    [SerializeField]
    Transform lungSpawnPoint;
    void Start()
    {
        base.Start();
    }

    protected override void HandleDeath()
    {
        finalTimeAlive = Mathf.FloorToInt(timeAlive);
        isDead = true;
        animator.SetTrigger("Death");
        Instantiate(lungObject, lungSpawnPoint);

    }

    protected override void InitializeAttacks()
    {
        phase1Attacks.Add(CounterStance);
        //phase1Attacks.Add(Shockwave);
        //phase1Attacks.Add(Charge);
        //phase1Attacks.Add(FallDown);
        //phase1Attacks.Add(Jump);
        //phase2Attacks.Add(FallDown);
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
        if (direction < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.linearVelocity += new Vector2(0f, jumpForce);
    }
    private void Shockwave()
    {
        animator.SetBool("Attack", true);
        StartCoroutine(ShockwaveRoutine());
    }
    private IEnumerator ShockwaveRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject shockwave = Instantiate(shockwavePrefab, shootPoint.position, Quaternion.identity);

        Rigidbody2D rb = shockwave.GetComponent<Rigidbody2D>();
        float moveDirection = transform.localScale.x > 0 ? 1f : -1f;
        rb.linearVelocity = new Vector2(shockwaveSpeed * moveDirection, 0);
        animator.SetBool("Attack", false);
    }
    private void CounterStance()
    {
        if (isBlocking) return;
        StartCoroutine(CounterRoutine());
    }

    private IEnumerator CounterRoutine()
    {
        isBlocking = true;
        animator.SetBool("Block", true);
        Debug.Log(isBlocking);

        yield return new WaitForSeconds(blockDuration);

        animator.SetBool("Block", false);
        isBlocking = false;
    }

    private void OnHit()
    {
        if (isBlocking)
        {
            Debug.Log("Counter attack triggered!");

            GameObject projectile = Instantiate(counterProjectilePrefab, counterShootPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            float direction = transform.localScale.x > 0 ? 1 : -1;
            rb.linearVelocity = new Vector2(counterProjectileSpeed * direction, 0); 

            rb.gravityScale = 0;

            animator.SetBool("Block", false);
            isBlocking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBlocking) return; 

        if (collision.CompareTag("PlayerBullet")) 
        {
            Debug.Log("Hit");
            OnHit();
        }
    }
}
