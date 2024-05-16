using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float damage;
    
    void Update()
    {
        // ������ �ڵ�
        // �Ʒ� �ڵ�� ����
        // transform.position += transform.rotation * Vector3.up * moveSpeed * Time.deltaTime;      
        transform.position += transform.up * moveSpeed * Time.deltaTime;      
    }

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null)
                return;
            enemy.Hit(damage);
            Destroy(gameObject);
        }
    }
    
}
