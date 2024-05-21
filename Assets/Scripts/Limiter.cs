using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Limiter : MonoBehaviour
{
    //rect bound
    [Header("ȭ�� ���")]//viewport ����
    [SerializeField] Vector2 viewPortLimitMin;
    [SerializeField] Vector2 viewPortLimitMax;

    Vector2 worldPosLimitMin;//���� �����ʹ� �� ������ ����������
    public Vector2 WorldPosLimitMin//�� �����ʹ� ������ �������� �Լ��� �۵�
    { 
        get 
        { 
            return worldPosLimitMin; 
        } 
    }

    Vector2 worldPosLimitMax;
    public Vector2 WorldPosLimitMax => worldPosLimitMax;


    Camera cam;
    GameManager gameManager;

    private void Start()
    {
        cam = Camera.main;
        gameManager = GameManager.Instance;
        gameManager._Limiter = this;

        initWorldPos();
    }

    /// <summary>
    /// ���ӽ��۽� ������Ʈ�� ȭ�� ��� �������� ���� ���������� �ʱ�ȭ �մϴ�.
    /// </summary>
    private void initWorldPos()
    {
        worldPosLimitMin = cam.ViewportToWorldPoint(viewPortLimitMin);
        worldPosLimitMax = cam.ViewportToWorldPoint(viewPortLimitMax);
    }

    /// <summary>
    /// �ڵ忡 ���� �÷��̾��ɸ��Ͱ� ī�޶� ������ �̵����� ���ϵ��� ����
    /// </summary>
    public Vector3 checkMovePosition(Vector3 _pos)
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(_pos);

        if (viewPortPos.x < viewPortLimitMin.x)//0~1
        {
            viewPortPos.x = viewPortLimitMin.x;
        }
        else if (viewPortPos.x > viewPortLimitMax.x)
        {
            viewPortPos.x = viewPortLimitMax.x;
        }

        if (viewPortPos.y < viewPortLimitMin.y)
        {
            viewPortPos.y = viewPortLimitMin.y;
        }
        else if (viewPortPos.y > viewPortLimitMax.y)
        {
            viewPortPos.y = viewPortLimitMax.y;
        }

        return cam.ViewportToWorldPoint(viewPortPos);
    }

    //Ʃ��
    public (bool _x, bool _y) IsReflectItem(Vector3 _pos, Vector3 _dir)//ȭ���迡 ��Ұų� ȭ������� �����ٸ� �ݻ��ؾ��Ѵٰ� �˷���
    {
        bool rX = false; 
        bool rY = false;

        if ((_pos.x < worldPosLimitMin.x && _dir.x < 0) || (_pos.x > worldPosLimitMax.x && _dir.x > 0))
        {
            rX = true;
        }

        if ((_pos.y < worldPosLimitMin.y && _dir.y < 0) || (_pos.y > worldPosLimitMax.y && _dir.y > 0))
        {
            rY = true;
        }

        return (rX, rY);
    }
}
