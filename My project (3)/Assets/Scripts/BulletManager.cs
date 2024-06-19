using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed;
    private int direction;
    public LayerMask isLayer;

    void Start()
    {
        Invoke("DestroyBullet", 4f);
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 0.1f, isLayer); ;
        if(ray.collider != null)
        {
            if (ray.collider.gameObject.CompareTag("enemy"))
            {
                PlayerManager.bulletAttack = true;
                PlayerManager.enemy = ray.collider.gameObject.transform;
            }
            else if(ray.collider.gameObject.CompareTag("chicken"))
            {
                PlayerManager.bulletAttack = true;
                PlayerManager.enemy = ray.collider.gameObject.transform;
                PlayerManager.jumpItem = true;
            }

            DestroyBullet();
        }

        if(transform.rotation.y == 0)
            transform.Translate(transform.right * speed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
