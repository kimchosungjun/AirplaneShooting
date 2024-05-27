using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    Transform trsBossPosition;//������ ��ġ

    bool isMovingTrsBossPosition = false;//������ ����ġ���� �̵��� �Ϸ��ߴ���
    Vector3 createPos = Vector3.zero;
    float timer = 0.0f;

    protected override void Start()
    {
        gameManager = GameManager.Instance;
        trsBossPosition = gameManager.TrsBossPostion;
        fabExplosion = gameManager.FabExplosion;
        createPos = transform.position;
    }

    protected override void moving()
    {
        if(isMovingTrsBossPosition == false) //��ġ���� �̵� 
        {
            if (timer < 1.0f)
            { 
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(createPos, trsBossPosition.position, timer);
            }
        }
        else//�̵� �Ϸ��� �¿�� �̵��ϸ鼭 ���Ͽ� ���� ����
        {

        }
    }

    public override void Hit(float _damage)
    {
        if(isDied == true)
        {
            return;
        }

        hp -= _damage;

        if (hp <= 0)
        {
            isDied = true;
            Destroy(gameObject);
            //�Ŵ����κ��� �޾ƿ� ���� ������ �� ��ġ�� �����ϰ� �θ�� ������� ���̾ �������
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //���簢��
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//���� ��ü�� �̹��� ���̸� �־���

            //gameManager.AddKillCount();//������ �׾��ٰ� ���� //�ٽ� ������ �⵿�ϵ��� ����
        }
        else
        {
            //�� ģ���� ��������Ʈ�� Ȱ���ϴ°��� �ƴ϶� ��������Ʈ �ִϸ��̼��� Ȱ�������� �ִϸ��̼ǿ��� ��Ʈ �ִ��� ����
        }
    }
}
