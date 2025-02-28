using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppablePlatforms : MonoBehaviour
{
    private Collider2D platformCollider;
    private bool playerOnPlatform;

    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") < 0 && playerOnPlatform)
        {
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    private IEnumerator DisableColliderTemporarily()
    {
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        yield return new WaitForSeconds(0.8f);
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }
}
