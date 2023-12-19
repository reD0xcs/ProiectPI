using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    private Transform attackPoint;
    private int attackDamage = 40;
    private float attackRate = 2.0f;
    private float nextAttackTime = 0f;
    public float attackRange = 2.05f;
    public LayerMask enemyLayers;
    [SerializeField] private AudioSource attackSound;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        attackPoint = GetComponent<Transform>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

        
        void Attack()
        {
            anim.SetTrigger("Attack");
            attackSound.Play();

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                // Check if the enemy has the Enemy component
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.takeDamage(attackDamage);
                }
                else
                {
                    // Log a warning if the enemy doesn't have the required component
                    Debug.LogWarning("Enemy object is missing the Enemy component.");
                }
            }
        }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}