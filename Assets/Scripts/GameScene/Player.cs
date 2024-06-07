using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region ������Ʈ
    GameManager gameManager;
    Limiter limiter;
    Animator anim;
    SpriteRenderer spriteRenderer;
    #endregion

    #region ����
    [Header("�÷��̾� ����")] 
    [SerializeField, Tooltip("�̵� �ӵ�")] float moveSpeed;
    [SerializeField, Tooltip("�ִ� ü��")] int maxHp = 3;
    [SerializeField, Tooltip("���� ü��, ���� �� Onvalidate ȣ��")] int curHp;
    Vector3 moveDir; // Horizontal, Vertical �Է�
    int beforeHp;  

    [Header("�÷��̾� �ǰ� ����")]
    GameObject fabExplosion;
    [SerializeField] float invincibiltyTime = 1f;//�����ð�
    bool invincibilty; 
    float invincibiltyTimer;

    [Header("�Ѿ�")]
    [SerializeField] GameObject fabBullet;
    [SerializeField] GameObject fabBullet2;
    [SerializeField] GameObject fabBullet3;
    [SerializeField] GameObject fabBullet4;
    [SerializeField] GameObject fabBullet5;
    [SerializeField, Tooltip("�����Ǵ� ������Ʈ�� �θ�")] Transform dynamicObject;
    [SerializeField, Tooltip("�ڵ� ���� ����")] bool autoFire = false; 
    [SerializeField, Tooltip("�Ѿ� ���� �ð�")] float fireRateTime = 0.5f; 
    [SerializeField, Tooltip("�Ѿ��� �߻�Ǵ� ��ġ")] Transform shootTrs;
    float fireTimer = 0;
    
    [Header("�÷��̾� ����")]
    [SerializeField] int minLevel = 1;
    [SerializeField] int maxLevel = 5;
    [SerializeField, Range(1, 5)] int curLevel;
    #endregion

    /// <summary>
    /// �ν����Ϳ��� ����� ������ ����� ȣ��
    /// </summary>

    #region ����Ƽ ���� ���
    private void OnValidate()
    {
        if (Application.isPlaying == false)
        {
            return;
        }

        if (beforeHp != curHp)
        {
            beforeHp = curHp;
            GameManager.Instance.SetHp(maxHp, curHp);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tool.GetTag(GameTags.Enemy))
        {
            Hit();
        }
        else if (collision.tag == Tool.GetTag(GameTags.Item))
        {
            Item item = collision.GetComponent<Item>();
            Destroy(item.gameObject); //�� �Լ��� �� �Լ��� ��� ���� ��ġ�� �Ǹ� �����ش޶� ��� �����ϴ� ���
            if (item.GetItemType() == Item.eItemType.PowerUp)
            {
                curLevel++;
                if(curLevel > maxLevel) 
                {
                    curLevel = maxLevel;
                }
            }
            else if (item.GetItemType() == Item.eItemType.HpRecovery)
            {
                curHp++;
                if (curHp > maxHp)
                {
                    curHp = maxHp;
                }
                gameManager.SetHp(maxHp, curHp);
            }
        }
    }
    #endregion

    #region ����Ƽ ������ ����Ŭ
    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        curHp = maxHp;
        curLevel = minLevel;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
        gameManager._Player = this;
    }

    void Update()
    {
        Moving();
        DoAnimation();
        CheckPlayerPos();

        Shoot();
        CheckInvincibilty();
    }
    #endregion

    private void CheckInvincibilty()//�����϶��� �۵��Ͽ� �����ð��� ������ ���� �ٽ� ������ Ǯ����
    {
        if (invincibilty == false) return;

        if (invincibiltyTimer > 0f)
        {
            invincibiltyTimer -= Time.deltaTime;
            if (invincibiltyTimer <= 0f)
            {
                setSprInvincibilty(false);
            }
        }
    }

    private void setSprInvincibilty(bool _value)
    {
        Color color = spriteRenderer.color;
        if (_value == true)//������ �Ȱ�ó�� ������ �ٿ� �������� �����̶� �˷���
        {
            color.a = 0.5f;
            invincibilty = true;
            invincibiltyTimer = invincibiltyTime;
        }
        else
        {
            color.a = 1.0f;
            invincibilty = false;
            invincibiltyTimer = 0f;
        }
        spriteRenderer.color = color;
    }

    /// <summary>
    /// �÷��̾� ��ü�� �⵿�� �����մϴ�.
    /// </summary>
    private void Moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");//���� Ȥ�� ������ �Է�// -1 0 1
        moveDir.y = Input.GetAxisRaw("Vertical");//�� Ȥ�� �Ʒ� �Է� // -1 0 1

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// �ִϸ��̼ǿ� � �ִϸ��̼��� �������� �Ķ���͸� ���� �մϴ�.
    /// </summary>
    private void DoAnimation()//�ϳ��� �Լ����� �ϳ��� ���
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
    }

    private void CheckPlayerPos()
    {
        if (limiter == null)
        {
            limiter = gameManager._Limiter;
        }
        transform.position = limiter.checkMovePosition(transform.position, false);
    }

    private void Shoot()
    {
        if (autoFire == false && Input.GetKeyDown(KeyCode.Space) == true)//������ �����̽� Ű�� �����ٸ�
        {
            CreateBullet();
        }
        else if (autoFire == true)
        {
            //�����ð��� ������ �Ѿ� �ѹ� �߻�
            fireTimer += Time.deltaTime;//1�ʰ� ������ 1�� �ɼ��ֵ��� �Ҽ������� fireTimer�� ����
            if (fireTimer > fireRateTime)
            {
                CreateBullet();
                fireTimer = 0;
            }
        }
    }

    private void CreateBullet()//�Ѿ��� �����Ѵ�
    {
        if (curLevel == 1)
        {
            GameObject go = Instantiate(fabBullet, shootTrs.position, Quaternion.identity, dynamicObject);
            Bullet goSc = go.GetComponent<Bullet>();
            goSc.ShootPlayer();
            //instBullet(shootTrs.position, Quaternion.identity);
        }
        else if (curLevel == 2)
        {
            Instantiate(fabBullet2, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);
        }
        else if (curLevel == 3)
        {
            Instantiate(fabBullet3, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position, Quaternion.identity);
            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);
        }
        else if (curLevel == 4)
        {
            Instantiate(fabBullet4, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position, Quaternion.identity);
            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);

            //Vector3 lv4Pos = shootTrsLevel4.position;
            //instBullet(lv4Pos, new Vector3(0, 0, angleBullet));

            //Vector3 lv4localPos = shootTrsLevel4.localPosition;
            //lv4localPos.x *= -1;
            //lv4localPos += transform.position;

            //instBullet(lv4localPos, new Vector3(0, 0, -angleBullet));
        }
        else if (curLevel == 5)
        {
            Instantiate(fabBullet5, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position, Quaternion.identity);
            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);

            //Vector3 lv4Pos = shootTrsLevel4.position;
            //instBullet(lv4Pos, new Vector3(0, 0, angleBullet));

            //Vector3 lv4localPos = shootTrsLevel4.localPosition;
            //lv4localPos.x *= -1;
            //lv4localPos += transform.position;

            //instBullet(lv4localPos, new Vector3(0, 0, -angleBullet));

            //Vector3 lv5Pos = shootTrsLevel5.position;
            //instBullet(lv5Pos, new Vector3(0, 0, angleBullet));

            //Vector3 lv5localPos = shootTrsLevel5.localPosition;
            //lv5localPos.x *= -1;
            //lv5localPos += transform.position;
            //instBullet(lv5localPos, new Vector3(0, 0, -angleBullet));
        }
    }

    private void InstantiateBullet(Vector3 _pos, Vector3 _angle)
    {
        GameObject go = Instantiate(fabBullet, _pos, Quaternion.Euler(_angle), dynamicObject);
        Bullet goSc = go.GetComponent<Bullet>();
        goSc.ShootPlayer();
    }
    private void InstantiateBullet(Vector3 _pos, Quaternion _quat)
    {
        GameObject go = Instantiate(fabBullet, _pos, _quat, dynamicObject);
        Bullet goSc = go.GetComponent<Bullet>();
        goSc.ShootPlayer();
    }

    public void Hit()
    {
        //�������¶�� �������� ���� ����
        if (invincibilty == true) return;

        setSprInvincibilty(true);
        
        curHp--;
        if(curHp < 0)
        {
            curHp = 0;
        }
        GameManager.Instance.SetHp(maxHp, curHp);

        curLevel--;
        if(curLevel < minLevel)
        {
            curLevel = minLevel;
        }

        if (curHp == 0)
        {
            Destroy(gameObject);
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //���簢��
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//���� ��ü�� �̹��� ���̸� �־���
        }
    }
}
