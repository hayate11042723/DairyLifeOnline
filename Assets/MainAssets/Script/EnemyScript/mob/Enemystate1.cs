using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemystate1 : MonoBehaviour
{
    [SerializeField] Animator EnemyController;
    [SerializeField] CharaStatus charadata;
    [SerializeField] MonoBehaviour Enemykinsetu;

    int State;
    IEnemyAction Enemykoudou;

    void Start()
    {
        EnemyController = GetComponent<Animator>();
        //IEnemy�̃C���^�[�t�F�[�X��錾�����X�N���v�g����ɓ���AEnemykoudou�֑���B
        Enemykoudou = GetComponent<IEnemyAction>();
    }

    void Update()
    {
        //�R���[�`����Enemytime�̎��Ԃ����ҋ@
        StartCoroutine(Enemytime());

        //Attack�p�����[�^�̒l������0���傫���Ȃ�U�����Ȃ̂�return;���ď������I������B
        int Attack = EnemyController.GetInteger("attack");
        if (Attack > 0)
        {
            return;
        }

        //�l�������Ă��Ȃ��ꍇ�ɔ���null�`�F�b�N�B
        if (Enemykoudou != null)
        {
            //�X�N���v�g��EnemyAIkoudou()���Ăяo���A�Ԃ��Ă����l��State�ɑ������B
            State = Enemykoudou.EnemyAIkoudou();

            //Switch����State�̒l�ɉ����ď�������B
            switch (State)
            {
                //State��0�Ȃ��~�B
                case 0:

                    EnemyController.SetInteger("attack", 0);
                    break;

                //State��1�Ȃ�U���B
                case 1:

                    EnemyController.SetInteger("attack", 1);
                    break;
            }
        }
    }

    IEnumerator Enemytime()
    {
        yield return new WaitForSeconds(charadata.EnemyTime);
    }
}