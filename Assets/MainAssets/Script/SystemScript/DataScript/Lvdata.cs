using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create LvData")]
public class Lvdata : ScriptableObject
{
    // ���X�g��錾���A�����̌o���l�N���X(PlayerExpTable)������
    public List<PlayerExpTable> playerExpTable = new List<PlayerExpTable>();

    //�N���X���C���X�y�N�^�[�ɕ\��
    [System.Serializable]
    // �e���x���ɒB����܂ł̕K�v�o���l�������Ă�������N���X
    public class PlayerExpTable
    {
        public int level;
        public int exp;
    }
}