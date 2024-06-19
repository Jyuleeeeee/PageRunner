using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    new AudioSource audio;

    public AudioClip[] audioClips;

    public GameManager gameManager;

    public static bool hasKey = false;

    public float speed;
    public float jumpPower;
    public int jumpCount = 0;
    public int maxJump = 1;
    public static bool jumpItem = false;

    public Vector2 playerVector;
    
    private bool onLadder;
    public float climbSpeed;

    public float gravity;

    public GameObject bullet;
    public Transform weaponPos;
    public int maxBullet;
    public int magazine;
    public static bool bulletAttack;
    public static Transform enemy;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();  
        boxCollider = GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();

        magazine = maxBullet;
    }

    void Update()
    {
        // Jump
        if (Input.GetKey(KeyCode.Space) && jumpCount == 0) // space�� �������鼭 jumpCount�� maxJump�� ���� �ʴ� ��쿡�� ���� ����
        {
            Rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
            animator.SetTrigger("space");
            PlaySound(0);
        }
        else if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift) && jumpCount < maxJump && jumpItem)
        {
            Rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetTrigger("space");
            PlaySound(0);

            jumpItem = false;
        }
        if (jumpItem)
        {
        if (maxJump == 1)
                maxJump = 2;
        }
        else if (!jumpItem)
            maxJump = 1;

        // Moving Animation
        if (Rigidbody.velocity.normalized.x == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = (Input.GetAxisRaw("Horizontal") == -1); // ���� Ű�̸� ��������Ʈ�� flip

            if (spriteRenderer.flipX == true)
                weaponPos.rotation = Quaternion.Euler(0, 180, 0);
            else
                weaponPos.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Ladder climb
        if (onLadder) // ��ٸ��� ��ġ�ϰ� ������
        {
            Rigidbody.gravityScale = 0f; // �߷°��� 0���� �ٲ� ��ٸ��� �پ����� �� �ְ� ��

            playerVector.y = Input.GetAxisRaw("Vertical");

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, playerVector.y * climbSpeed);
        }
        if(!onLadder) // ��ٸ��� ����� 
        {
            Rigidbody.gravityScale = gravity; // �߷°� ����
        }

        // Fire Bullet
        if (Input.GetMouseButtonDown(0) && magazine > 0)
        {            
            Instantiate(bullet, weaponPos.position, weaponPos.rotation);
            magazine--;

            PlaySound(2);
            
        }
        if(bulletAttack)
        {
            OnAttack(enemy);
            bulletAttack = false;
            gameManager.stagePoint += 150;
        }
    }

    void FixedUpdate()
    {
        // Move
        playerVector.x = Input.GetAxisRaw("Horizontal");

        Rigidbody.velocity = new Vector2(playerVector.x * speed, Rigidbody.velocity.y);
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        // Collision with Platform
        if (collision.gameObject.CompareTag("platform") || collision.gameObject.CompareTag("mush"))
        {
            if (jumpCount > 0)
                jumpCount--;
            else
                jumpCount = 0;
        }

        // Collision with Enemy
        if (collision.gameObject.CompareTag("enemy"))
        {
            // enemy�� �Ӹ��� ���� ���
            if(jumpCount > 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);

                gameManager.stagePoint += 300;
            }
            else // enemy�� ����� �浹�ϴ� ���
                OnDamaged(collision.transform.position);
        }

        // Collision with Chicken
        if(collision.gameObject.CompareTag("chicken"))
        {
            if (jumpCount > 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);

                gameManager.stagePoint += 300;
            }
            else
            {
                OnDamaged(collision.transform.position);

                jumpItem = true;
            }
        }

        // Collision with Trap
        if (collision.gameObject.CompareTag("trap"))
            OnDamaged(collision.transform.position);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Collision with Ladder
        if (collision.CompareTag("ladder"))
            onLadder = true;

        // Collision with Coin
        if (collision.CompareTag("coin"))
        {
            gameManager.stagePoint += 100;

            collision.gameObject.SetActive(false);

            PlaySound(4);
        }

        // Collision with Key
        if(collision.CompareTag("key"))
        {
            hasKey = true;

            collision.gameObject.SetActive(false);

            PlaySound(5);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Collision with FinishDoor
        if (collision.CompareTag("Finish") && hasKey)
        {
            if (Input.GetKey(KeyCode.E))
            {
                PlaySound(6);

                gameManager.NextStage();

                hasKey = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("ladder"))
            onLadder = false;
    }

    void OnAttack(Transform enemy)
    {
        if (enemy == null) return;
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
        PlaySound(3);
    }

    void OnDamaged(Vector2 targetPosition)
    {
        gameManager.HpDown();

        PlaySound(1);

        // Invincible state
        gameObject.layer = 12; // layer�� invinciblePlayer�� �����Ͽ� �������·� ����

        spriteRenderer.color = new Color(1, 1, 1, 0.5f); // ����ȭ

        int direction = (transform.position.x - targetPosition.x) > 0 ? 1 : -1; // Ÿ�ٰ� �ε��� �ݴ� ����
        Rigidbody.AddForce(new Vector2(direction, 1) * 5, ForceMode2D.Impulse); // �з���

        Invoke("OffDamaged", 3f); // 3�� �� ���� ����
    }

    void OffDamaged()
    {
        // Release of Invincible State
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void Die()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        boxCollider.enabled = false;

        Rigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        animator.SetBool("Alive", false);    

        Invoke("ColliderOn", 2f); // ������ �������� �� ���� ����
    }

    void ColliderOn()
    {
        boxCollider.enabled = true;
    }

    void PlaySound(int index)
    {
        audio.clip = audioClips[index];
        audio.Play();
    }
}