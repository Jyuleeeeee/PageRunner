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
        Vector2 frontVec = new Vector2(Rigidbody.position.x + nextMove * 0.5f, Rigidbody.position.y); // enemy�� �̵����� ���� ����
        RaycastHit2D downRay = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));
        RaycastHit2D frontRay = Physics2D.Raycast(frontVec, Vector2.right, 1, LayerMask.GetMask("platform"));
        if (downRay.collider == null || frontRay.collider == true) // ���� �Ʒ��� platform�� ���ų� ���� ������
        {
            nextMove *= -1; // �̵����� ��ȯ
            spriteRenderer.flipX = (nextMove == -1);

            CancelInvoke(); // ���� invoke�� ���߰�
            Invoke("MoveAuto", 3f); // �ٽ� ����
        }
    }

    void MoveAuto()
    {
        float duration = 0f;

        nextMove = Random.Range(-1, 2); // �̵����� �������� ����

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

        Invoke("MoveAuto", duration); // ��͸� ���� ���������� ������ �ٲٸ� ������ �� �ֵ��� ��
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
