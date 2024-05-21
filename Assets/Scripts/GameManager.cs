using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;//null ä�������
    [Header("�����")]
    [SerializeField] List<GameObject> listEnemy;
    GameObject fabExplosion;//���� �����͸� ������ �ִ� ������ private�� �����ϰ�

    [Header("�� ���� ����")]
    [SerializeField] bool isSpawn = false;//������ �����ϰų� ���ϴ� ������ ������ �̰���
    //true �� �����ϸ� ������ ���̻� ������ �ʰ��ϴ� �뵵�� Ȱ��

    [Header("�� ���� �ð�")]
    float enemySpawnTimer = 0.0f;//0�ʿ��� ���۵Ǵ� Ÿ�̸�
    [SerializeField, Range(0.1f,5f)] float spawnTime = 1.0f;

    [Header("�� ���� ��ġ")]
    [SerializeField] Transform trsSpawnPosition;
    [SerializeField] Transform trsDynamicObject;

    [Header("��� ������")]
    [SerializeField] List<GameObject> listItem;

    [Header("��� Ȯ��")]
    [SerializeField, Range(0.0f, 100.0f)] float itemDropRate;

    Limiter limiter;
    public Limiter _Limiter
    {
        get { return limiter; }
        set { limiter = value; }
    }

    Player player;
    public Player _Player
    {
        get { return player; }
        set { player = value; }
    }

    public GameObject FabExplosion//������ ���� Ȥ�� �����;��Ҷ��� �Լ��μ� ��밡��
    {
        get
        {
            return fabExplosion;
        }
        //set { fabExplosion = value; }
    }

    //�ν������� ���� ������ ������ ���Լ��� ���� ȣ��
    //private void OnValidate()
    //{
    //    if (Application.isPlaying == false) return;

    //    if (spawnTime < 0.1f)
    //    {
    //        spawnTime = 0.1f;
    //    }
    //}

    private void Awake()
    {
        //1���� �����ؾ���
        if (Instance == null)
        {
            Instance = this;
        }
        else//�ν��Ͻ��� �̹� �����Ѵٸ� ���� ����������
        {
            //Destroy(this);//�̷��� ������Ʈ�� ������
            Destroy(gameObject);//������Ʈ�� �������鼭 ��ũ��Ʈ�� ���� ������
        }

        fabExplosion = Resources.Load<GameObject>("Effect/Test/fabExplosion");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()//�����Ӵ� �ѹ� ����Ǵ� �Լ�
    {
        createEnemy();
    }

    private void createEnemy()
    {
        if (isSpawn == false) return;

        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer > spawnTime)
        {
            //���� ����
            int count = listEnemy.Count; //���� ���� 0 ~ 2
            int iRand = Random.Range(0, count);//0, 3

            Vector3 defulatPos = trsSpawnPosition.position;//y => 7 
            float x = Random.Range(limiter.WorldPosLimitMin.x, limiter.WorldPosLimitMax.x);//x => -2.4 ~ 2.4
            defulatPos.x = x;

            Instantiate(listEnemy[iRand], defulatPos, Quaternion.identity, trsDynamicObject);
            //������ ��ġ, ���̳��� ������Ʈ ��ġ�� �ʿ�

            //�ֻ����� ����
            float rate = Random.Range(0.0f, 100.0f);
            if (rate <= itemDropRate)
            {
                // ���Ⱑ �������� ������ ����
            }

            enemySpawnTimer = 0.0f;
        }
    }
}
