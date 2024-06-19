using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushManager : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            animator.SetTrigger("Trigger");
    }
}
