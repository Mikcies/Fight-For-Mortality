using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField]    
    float attackRange = 0.5f;
    [SerializeField]
    int attackDamage = 20;
    [SerializeField]
    Transform attackPoint;
    [SerializeField]
    LayerMask enemyLayers;
    [SerializeField]
    float attackRate = 2f;
    [SerializeField]
    float nextAttackTime = 0f;

    [SerializeField]
    Animator animator;
    Parry parry;

    void Update()
    {
        parry = GetComponent<Parry>();

        if (Time.time >= nextAttackTime && !parry.isParrying)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                meleeAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void meleeAttack()
    {
        animator.SetBool("Attack", true);
        Debug.Log("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BossBase>().TakeDamage(attackDamage);
            Debug.Log("We hit " + enemy.name);
        }
        StartCoroutine(ResetAttackAnimation());
    }
    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack", false);
    }


private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
