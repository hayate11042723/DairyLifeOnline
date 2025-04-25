using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // Player��CharaStatus��ǂݍ���
    [SerializeField] private PlayerStatus playerdata;
    [SerializeField] private GameObject hitEffectPrefab; // �q�b�g���ɕ\������G�t�F�N�g��Prefab
    [SerializeField] private Vector3 hitEffectScale = Vector3.one; // �q�b�g�G�t�F�N�g�̃X�P�[��

    private void OnTriggerEnter(Collider other)
    {
        // other�̃Q�[���I�u�W�F�N�g�̃C���^�[�t�F�[�X���Ăяo��
        IDamageable damageable = other.GetComponent<IDamageable>();

        // damageable��null�l�������Ă��Ȃ����`�F�b�N
        if (damageable != null)
        {
            // damageable��damage�������\�b�h���Ăяo���B�����Ƃ���Player��ATK���w��
            damageable.Damage(playerdata.ATK);

            // �q�b�g�G�t�F�N�g�𐶐�
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            GameObject hitEffect = Instantiate(hitEffectPrefab, contactPoint, Quaternion.identity);
            hitEffect.transform.localScale = hitEffectScale; // �G�t�F�N�g�̃X�P�[����ݒ�
        }
    }
}
