﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        AttackPhase1,
        TransitionToPhase2,
        AttackPhase2,
        Death
    }
    protected Collider2D collider;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected BossState currentState = BossState.Idle;
    [SerializeField]
    protected float maxHealth = 100f;
    [SerializeField]
    internal float currentHealth;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;

    protected bool isDead = false; 
    private float flashDuration = 0.1f;

    protected float attackCooldown = 4.5f;
    protected float phase2AttackCooldown = 2f;
    private float lastAttackTime;

    protected List<System.Action> phase1Attacks = new List<System.Action>();
    protected List<System.Action> phase2Attacks = new List<System.Action>();

    protected bool idleTimerStarted = false;
    protected bool isAttacking = false;
    protected float timeAlive = 0f;
    internal float finalTimeAlive = 0f;
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        InitializeAttacks();
    }

    protected virtual void Update()
    {
        timeAlive += Time.deltaTime;
        if (isDead) return;
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                break;

            case BossState.AttackPhase1:
                HandleAttackPhase1();
                break;

            case BossState.TransitionToPhase2:
                HandleTransitionToPhase2();
                break;

            case BossState.AttackPhase2:
                HandleAttackPhase2();
                break;

            case BossState.Death:
                HandleDeath();
                break;
        }
    }

    protected virtual void HandleIdleState()
    {
        if (!idleTimerStarted)
        {
            idleTimerStarted = true;
            Invoke("TransitionToPhase1", 1f); 
        }
    }
    protected void TransitionToPhase1()
    {
        if (currentState == BossState.Idle)
        {
            currentState = BossState.AttackPhase1;
        }
    }
    protected virtual void HandleAttackPhase1()
    {
        ExecuteRandomAttack(phase1Attacks);
        if (currentHealth <= maxHealth / 2)
        {
            currentState = BossState.TransitionToPhase2;
        }
    }

    protected virtual void HandleAttackPhase2()
    {
        ExecuteRandomAttack(phase2Attacks);
        if (currentHealth <= 0)
        {
            currentState = BossState.Death;
        }
    }

    protected virtual void HandleTransitionToPhase2()
    {
        attackCooldown = phase2AttackCooldown;
        currentState = BossState.AttackPhase2;

    }

    protected virtual void HandleDeath()
    {
        finalTimeAlive = Mathf.FloorToInt(timeAlive);
        isDead = true;
        rb.gravityScale = 1; 
        animator.SetTrigger("Death"); 
    }

    internal void TakeDamage(int damage)
    {
        if (isDead) return; 
        currentHealth -= damage;
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            Debug.Log("Boss is dead");
            HandleDeath();
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    protected void ExecuteRandomAttack(List<System.Action> attackList)
    {
        if (isDead) return;
        if (isAttacking) return;
        if (attackList.Count > 0 && Time.time >= lastAttackTime + attackCooldown)
        {
            int randomIndex = Random.Range(0, attackList.Count);
            attackList[randomIndex]?.Invoke();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void InitializeAttacks()
    {
    }
}
