using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;//null ä�������
    [Header("�����")]
    [SerializeField] List<GameObject> listEnemy;
    GameObject fabExplosion;//���� �����͸� ������ �ִ� ������ private�� �����ϰ�

    [Header("�� ���� ����")]
    [SerializeField] bool isSpawn = false;//������ �����ϰų� ���ϴ� ������ ������ �̰���
    bool isSpawnBoss = false;//������ ���� ���ӿ� ���� ������
    //true �� �����ϸ� ������ ���̻� ������ �ʰ��ϴ� �뵵�� Ȱ��

    [Header("�� ���� �ð�")]
    float enemySpawnTimer = 0.0f;//0�ʿ��� ���۵Ǵ� Ÿ�̸�
    [SerializeField, Range(0.1f,5f)] float spawnTime = 1.0f;

    [Header("�� ���� ��ġ")]
    [SerializeField] Transform trsSpawnPosition;
    [SerializeField] Transform trsDynamicObject;

    [Header("��Ӿ�����")]
    [SerializeField] List<GameObject> listItem;

    [Header("��� Ȯ��")]
    [SerializeField, Range(0.0f, 100.0f)] float itemDropRate;

    [Header("ü�� ������")]
    [SerializeField] FunctionHP functionHP;
    [SerializeField] Slider slider;

    [Header("���� ������")]
    [SerializeField] Transform trsBossPostion;
    public Transform TrsBossPostion => trsBossPostion;//get ���


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

    [Header("�������� ����")]
    [SerializeField] int killCount = 100;
    int curKillCount = 0;
    [SerializeField] TMP_Text textSlider;

    public bool GetPlayerPosition(out Vector3 _pos)
    {
        _pos = default;
        if (player == null)
        {
            return false;
        }
        else
        {
            _pos = player.transform.position;
            return true;
        }
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

        initSlider();
    }

    private void initSlider()
    {
        slider.minValue = 0;
        slider.maxValue = killCount;
        slider.value = 0;
        textSlider.text = $"{curKillCount.ToString("d4")} / {killCount.ToString("d4")}";
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

            GameObject go = Instantiate(listEnemy[iRand], defulatPos, Quaternion.identity, trsDynamicObject);
            //������ ��ġ, ���̳��� ������Ʈ ��ġ�� �ʿ�

            //�ֻ����� ����
            float rate = Random.Range(0.0f, 100.0f);
            if (rate <= itemDropRate)
            {
                //���Ⱑ �������� ������ ����
                Enemy goSc = go.GetComponent<Enemy>();
                goSc.SetItem();
            }

            enemySpawnTimer = 0.0f;
        }
    }

    public void createItem(Vector3 _pos)
    {
        int count = listItem.Count;
        int iRand = Random.Range(0, count);
        Instantiate(listItem[iRand], _pos, Quaternion.identity, trsDynamicObject);
    }

    public void SetHp(float _maxHp, float _curHp)
    {
        //��� hp���� �˷������
        functionHP.SetHp(_maxHp, _curHp);
    }

    public void AddKillCount()
    {
        curKillCount++;
        modifySlider();
        checkSpawnBoss();
    }

    private void modifySlider()
    {
        slider.value = curKillCount;
        textSlider.text = $"{curKillCount.ToString("d4")} / {killCount.ToString("d4")}";
    }

    private void checkSpawnBoss()
    {
        if (isSpawnBoss == false && curKillCount >= killCount)//���� ����
        {
            isSpawn = false;
            isSpawnBoss = true;

            //���� ����
        }
    }
}
