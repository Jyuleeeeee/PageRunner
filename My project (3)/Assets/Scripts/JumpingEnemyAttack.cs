using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyAttack : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    Animator animator;
    CircleCollider2D circleCollider;
  
    public float jumpPower;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
            Rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            

            animator.SetTrigger("attack");
            circleCollider.enabled = false;
        }
        
    }
}
