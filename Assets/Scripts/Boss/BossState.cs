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

    [SerializeField] 
    protected BossState currentState = BossState.Idle;
    [SerializeField]
    protected float maxHealth = 100f;
    [SerializeField]
    protected float currentHealth;

    SpriteRenderer spriteRenderer;
    float flashDuration = 0.1f;

    protected float attackCooldown = 2f;
    private float lastAttackTime; 

    protected Rigidbody2D rb;
    protected List<System.Action> phase1Attacks = new List<System.Action>();
    protected List<System.Action> phase2Attacks = new List<System.Action>();

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeAttacks();
    }

    protected virtual void Update()
    {
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
        currentState = BossState.AttackPhase2;
    }

    protected virtual void HandleDeath()
    {
        Destroy(gameObject);
    }

    internal void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
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