using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] List<GameManager> EnemyList = new List<GameManager>();
    GameObject fabExlposion;
    public GameObject FabExplosion { get { return fabExlposion; } } // �Լ��� ��밡��, CBV

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        fabExlposion = Resources.Load<GameObject>("Effect/FabExplosion");    
    }

}
