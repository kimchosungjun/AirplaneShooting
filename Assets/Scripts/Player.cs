using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    GameManager gameManager;
    GameObject fabExplosion;
    [Header("�÷��̾� ����"),SerializeField,Tooltip("�÷��̾� �̵��ӵ�")] float moveSpeed;
    Vector3 moveDir = new Vector3();
    Camera cam;
    [Space]

    [Header("<color=red>ȭ��</color>���")]
    [SerializeField] Vector2 viewPortLimitMin;
    [SerializeField] Vector2 viewPortLimitMax;

    [SerializeField, TextArea] string text;

    [Header("�Ѿ�"),SerializeField] GameObject fabBullet; // �Ѿ�
    [SerializeField] Transform dynamicObject;
    [SerializeField] bool autoFire = false;
    [SerializeField,Range(0f,5f)] float fireRateTime = 1f;
    float fireTimer = 0f;
    private void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
    }

    void Update()
    {

        Moving();
        DoAnimation();
        CheckMovePosition();
        Shoot();
    }

    public void Moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
        transform.position += moveDir*Time.deltaTime*moveSpeed;

        // transform.position => ���� ������
        // transform.localPosition => �� �����Ͱ� Root�����Ͷ�� ���� ������ ��ǥ�� ���, �� �����Ͱ� �ڽ� �����Ͷ�� �θ�κ����� �Ÿ��� ������ ��ǥ�� ��� 
    }

    public void DoAnimation()
    {
        anim.SetInteger("X", (int)moveDir.x);
    }

    public void CheckMovePosition()
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(transform.position);
        if (viewPortPos.x < viewPortLimitMin.x)
            viewPortPos.x = viewPortLimitMin.x;
        else if (viewPortPos.x > viewPortLimitMax.x)
            viewPortPos.x = viewPortLimitMax.x;
        if (viewPortPos.y < viewPortLimitMin.y)
            viewPortPos.y = viewPortLimitMin.y;
        else if (viewPortPos.y > viewPortLimitMax.y)
            viewPortPos.y = viewPortLimitMax.y;
        Vector3 fixedPos = cam.ViewportToWorldPoint(viewPortPos);
        transform.position = fixedPos;
    }
    private void Shoot()
    {
        if (!autoFire && Input.GetKeyDown(KeyCode.Space))
        {
            CreateBullet();
        }
        else if(autoFire)
        {
            fireTimer += Time.deltaTime; // 1�ʰ� ������ 1�� �� �� �ֵ��� �Ҽ������� frieTimer�� ���δ�.
            if (fireTimer > fireRateTime)
            {
                CreateBullet();
                fireTimer = 0f;
            }
        }
    }

    private void CreateBullet()
    {
        Instantiate(fabBullet, transform.position, Quaternion.identity, dynamicObject);
    }
}
