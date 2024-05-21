using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float hp;

    Sprite defaultSprite;
    [SerializeField] Sprite hitSprite;
    SpriteRenderer spriteRenderer;
    GameObject fabExplosion;
    GameManager gameManager;
    bool haveItem = false;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        if (haveItem == true)
        {
            spriteRenderer.color = new Color(0.3f, 0.5f, 1f, 1f);
        }
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
    }

    void Update()
    {
        moving();
    }

    private void moving()
    {
        transform.position -= transform.up * moveSpeed * Time.deltaTime;

        //transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        //transform.position += transform.rotation * Vector3.down * moveSpeed * Time.deltaTime;
    }

    public void Hit(float _damage)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
            //�Ŵ����κ��� �޾ƿ� ���� ������ �� ��ġ�� �����ϰ� �θ�� ������� ���̾ �������
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //���簢��
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//���� ��ü�� �̹��� ���̸� �־���
        }
        else
        {
            //hit ���� ��������Ʈ ������
            spriteRenderer.sprite = hitSprite;
            //�ణ�� �ð��� �����ڿ� � �Լ��� �����ϰ� ������
            Invoke("setDefaultSprite", 0.04f);
        }
    }

    private void setDefaultSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }
}
