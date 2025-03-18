using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPcontroll : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;  
    [SerializeField] Animator Animator;
    internal int currentHealth;
    [SerializeField] GameObject loseCanvas;

    private List<string> selectedAbilities = new List<string>(); 

    void Start()
    {
        SetAbilities(AbilitySelection.GetSelectedAbilities()); 
        ApplyAbilities(); 
        currentHealth = maxHealth;
    }

    public void SetAbilities(List<string> abilities)
    {
        selectedAbilities = abilities;
    }

    void ApplyAbilities()
    {
        if (selectedAbilities.Contains("ExtraHealth"))
        {
            maxHealth += 1; 
        }
    }

    internal void HandleDeath()
    {
        playerMove player = GetComponent<playerMove>();
        if (player != null)
        {
            player.enabled = false;
        }

        Animator.SetTrigger("Death");
        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        Time.timeScale = 0;
        loseCanvas.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("enemy"))
        {
            TakeDamage();
        }
        if(collision.gameObject.CompareTag("DummyBullet"))
        {
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            Animator.Play("Hurt");
        }
        if (currentHealth == 0)
        {
            HandleDeath();
        }
    }
}
