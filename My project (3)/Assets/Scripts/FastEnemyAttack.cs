using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemyAttack : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;

    public EnemyMove enemyMove;
    public float maxSpeed;
    private bool isRunning;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            enemyMove.speed = maxSpeed;

            if (collision.transform.position.x >= transform.position.x)
            {
                enemyMove.nextMove = 1;

                if(spriteRenderer.flipX == true)
                    spriteRenderer.flipX = false;
            }
            else
            {
                enemyMove.nextMove -= 1;

                if (spriteRenderer.flipX == false)
                    spriteRenderer.flipX = true;
            }

            capsuleCollider.enabled = false;
            isRunning = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRunning == true && collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("attack");

            enemyMove.Invoke("OnDamaged", 2f);
            Invoke("Die", 4f);
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
