using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public float speed;
    public int nextMove;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        MoveAuto();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move
        Rigidbody.velocity = new Vector2(nextMove * speed, Rigidbody.velocity.y);

        // Platform Check
        Vector2 frontVec = new Vector2(Rigidbody.position.x + nextMove * 0.5f, Rigidbody.position.y); // enemy의 이동방향 기준 앞쪽
        RaycastHit2D downRay = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));
        RaycastHit2D frontRay = Physics2D.Raycast(frontVec, Vector2.right, 1, LayerMask.GetMask("platform"));
        if (downRay.collider == null || frontRay.collider == true) // 전방 아래에 platform이 없거나 벽에 닿으면
        {
            nextMove *= -1; // 이동방향 전환
            spriteRenderer.flipX = (nextMove == -1);

            CancelInvoke(); // 기존 invoke를 멈추고
            Invoke("MoveAuto", 3f); // 다시 실행
        }
    }

    void MoveAuto()
    {
        float duration = 0f;

        nextMove = Random.Range(-1, 2); // 이동방향 랜덤으로 설정

        if (nextMove == -1 || nextMove == 1)
        {
            duration = Random.Range(3f, 6f);

            animator.SetBool("isMoving", true);
            spriteRenderer.flipX = (nextMove == -1);
        }
        else if (nextMove == 0)
        {
            duration = 1f;

            animator.SetBool("isMoving", false);
        }

        Invoke("MoveAuto", duration); // 재귀를 통해 지속적으로 방향을 바꾸며 움직일 수 있도록 함
    }

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        boxCollider.enabled = false;

        Rigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        nextMove = 0;

        CancelInvoke();
        Invoke("Die", 4f);
    }

    void Die()
    {
        this.gameObject.SetActive(false);
    }
}
