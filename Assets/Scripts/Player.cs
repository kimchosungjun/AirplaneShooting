using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    
    [Header("�÷��̾� ����"), SerializeField, Tooltip("�÷��̾��� �̵��ӵ�")] float moveSpeed;

    Vector3 moveDir;

    [Header("�Ѿ�")]
    [SerializeField] GameObject fabBullet;//�÷��̾ �����ؼ� ����� ���� �Ѿ�
    [SerializeField] Transform dynamicObject;
    [SerializeField] bool autoFire = false;//�ڵ����ݱ��
    [SerializeField] float fireRateTime = 0.5f;//�̽ð��� ������ �Ѿ��� �߻��
    float fireTimer = 0;

    GameManager gameManager;
    GameObject fabExplosion;
    Limiter limiter;

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //private static  void initCode()
    //{
    //    Debug.Log("initCode");
    //}

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    private void Start()
    {
        //cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
        gameManager._Player = this;
    }

    void Update()
    {
        moving();
        doAnimation();
        checkPlayerPos();

        shoot();
    }

    /// <summary>
    /// �÷��̾� ��ü�� �⵿�� �����մϴ�.
    /// </summary>
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");//���� Ȥ�� ������ �Է�// -1 0 1
        moveDir.y = Input.GetAxisRaw("Vertical");//�� Ȥ�� �Ʒ� �Է� // -1 0 1

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// �ִϸ��̼ǿ� � �ִϸ��̼��� �������� �Ķ���͸� ���� �մϴ�.
    /// </summary>
    private void doAnimation()//�ϳ��� �Լ����� �ϳ��� ���
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
    }

    private void checkPlayerPos()
    {
        if (limiter == null)
        {
            limiter = gameManager._Limiter;
        }
        transform.position = limiter.checkMovePosition(transform.position);
    }

    private void shoot()
    {
        if (autoFire == false && Input.GetKeyDown(KeyCode.Space) == true)//������ �����̽� Ű�� �����ٸ�
        {
            createBullet();
        }
        else if (autoFire == true)
        {
            //�����ð��� ������ �Ѿ� �ѹ� �߻�
            fireTimer += Time.deltaTime;//1�ʰ� ������ 1�� �ɼ��ֵ��� �Ҽ������� fireTimer�� ����
            if(fireTimer > fireRateTime) 
            {
                createBullet();
                fireTimer = 0;
            }
        }
    }

    private void createBullet()//�Ѿ��� �����Ѵ�
    {
        Instantiate(fabBullet, transform.position, Quaternion.identity, dynamicObject);
    }
}
