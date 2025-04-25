using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CharaAttackBoss : MonoBehaviour
{
    // �V���A�������Ă���Bcharadata���w��B
    [SerializeField] private CharaStatus charadata;
    // ���̂������Ă���Q�[���I�u�W�F�N�g(�e)���w��B
    [SerializeField] GameObject AttackChara;
    // �q�b�g���ɕ\������G�t�F�N�g��Prefab
    [SerializeField] private GameObject hitEffectPrefab;
    // �q�b�g�G�t�F�N�g�̔����ʒu�̃I�t�Z�b�g
    [SerializeField] private Vector3 hitEffectOffset;
    // �q�b�g�G�t�F�N�g�̃X�P�[��
    [SerializeField] private Vector3 hitEffectScale = Vector3.one;

    int HHcount; // �c��q�b�g��
    int ATK; // �U����
    ICharaAttack Hcount; // ICharaAttack�C���^�[�t�F�[�X

    void Start()
    {
        if (charadata != null)
        {
            // charadata��ATK�����B
            ATK = charadata.ATK;
        }

        // ICharaAttack�̃C���^�[�t�F�[�X����`���ꂽ�R���|�[�l���g(�X�N���v�g)��Hcount�ɑ���B
        Hcount = AttackChara.GetComponent<ICharaAttack>();
    }

    // �̂��Q�[���I�u�W�F�N�g�ɐN�������u�ԂɌĂяo��
    void OnTriggerEnter(Collider other)
    {
        // ICharaAttack�̃C���^�[�t�F�[�X����`���ꂽ�X�N���v�g��HitCount()���Ăяo���A�Ԃ�l(�c��q�b�g��)��HHcount�ɑ������B
        HHcount = Hcount.HitCount();

        // HHcount��0�ȉ��Ȃ�������Ƀq�b�g���Ă���Breturn;�ŏ������I��点��B
        if (HHcount <= 0)
        {
            return;
        }

        if (other.tag == "Player")
        {
            // �R���C�_�[�̂���Q�[���I�u�W�F�N�g�̃C���^�[�t�F�[�X���Ăяo��
            IDamageable damageable = other.GetComponent<IDamageable>();
            // damageable��null�l�������Ă��Ȃ����`�F�b�N
            if (damageable != null)
            {
                // �_���[�W������̂��m�肵���̂Ńq�b�g��-1
                Hcount.HitCountdown();
                // damageable�̃_���[�W�������\�b�h���Ăяo���B�����Ƃ���charadata��ATK���w��
                damageable.Damage(ATK);

                // �q�b�g�G�t�F�N�g�𐶐�
                Vector3 hitPoint = other.ClosestPoint(transform.position) + hitEffectOffset;
                GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
                hitEffect.transform.localScale = hitEffectScale; // �G�t�F�N�g�̃X�P�[����ݒ�
            }
        }
    }
}

