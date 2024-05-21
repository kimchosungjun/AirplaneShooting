using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    //���⿡ ������� or �÷��̾ �������
    //���ʵڿ� ������ٰ� ���������
    //ȭ������� ��������

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)//collision�� ��� �ݸ���
    {
        //���� ����� ��Ȯ�� �� �ʿ䰡 ����
        if (collision.tag == "Enemy")
        {
            Destroy(gameObject);//�Ѿ� ������ ����
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Hit(1);
        }
        if (collision.tag == "Player")
        {
            Destroy(gameObject);//�Ѿ� ������ ����
        }
    }

    void Start()
    {
        //Destroy(gameObject, 2.5f);
    }

    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
