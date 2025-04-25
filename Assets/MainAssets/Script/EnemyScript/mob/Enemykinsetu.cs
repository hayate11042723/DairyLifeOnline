using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemykinsetu : MonoBehaviour, ICharaAttack
{
    [SerializeField] Animator EnemyController;
    [SerializeField] Collider AttackCollider;
    int Hcount;   //�U���q�b�g��
    bool Attacktime;   //�U����

    public int HitCount()
    {
        //���݂̎c��q�b�g����Ԃ��B
        return Hcount;
    }

    public void HitCountdown()
    {
        //�_���[�W�����邱�Ƃ��m�肵���ۂɎc��q�b�g�������炷�B
        --Hcount;
    }
    public bool Attacktimekanshi()
    {
        //�U�������U�����łȂ�����Ԃ��B
        return Attacktime;
    }

    void Start()
    {
        EnemyController = GetComponent<Animator>();
    }

    void AttackStart()
    {
        AttackCollider.enabled = true;
        Hcount = 1;
        Attacktime = true;
    }

    void Hit()
    {
        Hcount = 0;
        Attacktime = false;
    }

    void AttackEnd()
    {
        AttackCollider.enabled = false;
        //�A�j���[�V�����I�� Attack�p�����[�^��0�ɂ���B
        EnemyController.SetInteger("attack", 0);
    }
}
