using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;//c#
using Newtonsoft.Json;

public class StartSceneManager : MonoBehaviour
{
    [Header("��ư")]
    [SerializeField] Button btnStart;
    [SerializeField] Button btnRanking;
    [SerializeField] Button btnExitRanking;
    [SerializeField] Button btnExit;

    [Header("��ũ ������")]
    [SerializeField] GameObject fabRank;
    [SerializeField] Transform contents;
    [SerializeField] GameObject viewRank;

    private string dataPath;
    
    private void Awake()
    {
        #region ��������
        //btnStart.onClick.AddListener(function);

        //UnityAction<float> action = (float _value) => 
        //{
        //    Debug.Log($"���ٽ��� ���� �Ǿ��� => {_value}");
        //};

        //���ٽ�
        //�̸����� �Լ�
        //action.Invoke(0.1f);//���ٽ��� Ư�� �̺�Ʈ�� invoke�� ���ؼ� ���డ��

        //btnStart.onClick.AddListener(() => 
        //{
        //    gameStart(1, 2, 3, 4, 5);
        //});

        //json
        //string ���ڿ�, Ű�� ����
        //{key:value};

        //save���, ���� ���� �̵��Ҷ� ������ �����ϴ� �����Ͱ� �ִٸ�

        //1.�÷��̾� �������� �̿��� ����Ƽ�� �����ϴ� ���
        //PlayerPrefs//����Ƽ�� ������ �����͸� �����ϵ��� ����Ƽ ���ο� ���� 

        //PlayerPrefs.SetInt("test", 999);//���� ������ 1���� ���� setint setfloat
        //�����͸� �������� �ʴ��� //test 999, ������ �����ϸ� �̵����ʹ� �����ǰ� �ҷ��ü� ����
        //int value = PlayerPrefs.GetInt("test", -1);//int�� ����Ʈ 0�� ���
        //Debug.Log(value);

        //PlayerPrefs.hasKey
        //PlayerPrefs.DeleteKey("test");

        //string path = Application.streamingAssetsPath;//os�� ���� �б��������� ����
        //~/Assets/StreamingAssets
        //File.WriteAllText(path + "/abc.json", "�׽�Ʈ22");
        //File.Delete(path + "/abc.json");
        //string result = File.ReadAllText(path + "/abc.json");
        //Debug.Log(result);

        //string path = Application.persistentDataPath + "/Jsons";//R/W�� ������ ������ġ
        //~/AppData/LocalLow/DefaultCompany/Class6/Jsons

        //if (Directory.Exists(path) == false)
        //{
        //    Directory.CreateDirectory(path);
        //}
        //if (File.Exists(path + "/Test/abc.json") == true)
        //{
        //    string result = File.ReadAllText(path + "Test/abc.json");
        //}
        //else//������ ������ �������� ����
        //{
        //    //���ο� ���� ��ġ�� �����͸� ����� �����
        //    File.Create(path + "/Test");//������ �������
        //}

        //string jsonData = JsonUtility.ToJson(cUserData);
        //{"Name":"������","Score":100}

        //cUserData user2 = new cUserData();
        //user2 = JsonUtility.FromJson<cUserData>(jsonData);

        //string jsonData = JsonUtility.ToJson(listUserData);
        //JsonUtility �� list�� json���� �����ϸ� Ʈ������ ������

        //List<cUserData> listUserData = new List<cUserData>();
        //listUserData.Add(new cUserData() { Name = "������", Score = 100 });
        //listUserData.Add(new cUserData() { Name = "�󸶹�", Score = 200 });

        //string jsonData = JsonConvert.SerializeObject(listUserData);

        //List<cUserData> afterData = JsonConvert.DeserializeObject<List<cUserData>>(jsonData);
        #endregion
        BtnAddListner();
        InitRankView();
    }

    /// <summary>
    /// ���� ���� ��ư�� �޼��� ����
    /// </summary>
    private void BtnAddListner()
    {
        btnExit.onClick.AddListener(GameExit);
        btnStart.onClick.AddListener(GameStart);
        btnRanking.onClick.AddListener(ShowRanking);
        btnExitRanking.onClick.AddListener(() => { viewRank.SetActive(false); });
    }

    /// <summary>
    /// ��ũ�� ����Ǿ� �ִٸ� ����� ��ũ �����͸� �̿��ؼ� ��ũ�並 ������ְ�
    /// ��ũ�� ����Ǿ� ���� �ʴٸ� ����ִ� ��ũ�� ����� ��ũ�並 �������
    /// </summary>
    private void InitRankView()
    {
        List<UserData> listUserData = null;
        ClearRankView();
        if (PlayerPrefs.HasKey(Tool.rankKey) == true)//��ũ �����Ͱ� ������ �Ǿ��־��ٸ�
        {
            listUserData = JsonConvert.DeserializeObject<List<UserData>>(PlayerPrefs.GetString(Tool.rankKey));
        }
        else//��ũ�����Ͱ� ����Ǿ� ���� �ʾҴٸ�
        {
            listUserData = new List<UserData>();
            int rankCount = Tool.rankCount;
            for (int iNum = 0; iNum < rankCount; ++iNum)
            {
                listUserData.Add(new UserData() { Name = "None", Score = 0 });
            }
        }

        int count = listUserData.Count;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            UserData data = listUserData[iNum];
            GameObject go = Instantiate(fabRank, contents);
            FabRanking fabRanking = go.GetComponent<FabRanking>();
            fabRanking.SetData((iNum + 1).ToString(), data.Name, data.Score);
        }
        viewRank.SetActive(false);
    }

    private void ClearRankView()
    {
        int count = contents.childCount;
        for (int idx = count - 1; idx > -1; idx--)
        {
            Destroy(contents.GetChild(idx).gameObject);
        }
    }

    private void GameStart()
    {
        
    }

    private void ShowRanking()
    {
        viewRank.SetActive(true);
    }

    /// <summary>
    /// �����͸� �̿��ؼ� ���� ���� 
    /// </summary>
    private void GameExit()
    {
        //�����Ϳ��� �÷��̸� ���� ���, ������ ���� ���
        //���带 ���ؼ� ������ ������ �������� �ȵ�
        //��ó��,�ڵ尡 ���ǿ� ���ؼ� ������ ���°�ó�� Ȥ�� �ִ°�ó�� 
        //�����ϰ� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else//����Ƽ �����Ϳ��� �������� �ʾ�����
        //���������� ���� ����
        Application.Quit();
#endif
    }
}

