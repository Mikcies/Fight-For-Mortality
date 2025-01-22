using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 0.5f;
    [SerializeField]
    private int attackDamage = 20;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private LayerMask enemyLayers;
    [SerializeField]
    private float attackRate = 0.5f;
    private float nextAttackTime = 0f;

    [SerializeField]
    private Animator animator;
    private Parry parry;

    private bool isAttacking = false;

    private void Start()
    {
        parry = GetComponent<Parry>();
    }
    void Update()
    {
        if (Time.time >= nextAttackTime && !isAttacking && !parry.isParrying)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                MeleeAttack();
            }
        }
    }

    void MeleeAttack()
    {
        if (isAttacking) return;  

        isAttacking = true;
        animator.SetBool("Attack", true);
        Collider2D hitEnemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);
        if (hitEnemy != null)
        {
            BossBase enemyBoss = hitEnemy.GetComponent<BossBase>();
            if (enemyBoss != null)
            {
                enemyBoss.TakeDamage(attackDamage);
            }
        }
        nextAttackTime = Time.time + 1 / attackRate;
        StartCoroutine(ResetAttackAnimation());
    }

    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Attack", false);
        isAttacking = false;  
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
