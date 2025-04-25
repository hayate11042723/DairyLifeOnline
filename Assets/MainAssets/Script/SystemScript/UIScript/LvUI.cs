using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] PlayerStatus charadata; // �v���C���[�̃X�e�[�^�X�f�[�^
    [SerializeField] Text NAME; // �v���C���[�̖��O��\������e�L�X�g
    [SerializeField] Text LV; // �v���C���[�̃��x����\������e�L�X�g

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�̖��O�ƃ��x�����擾
        string charaname = charadata.NAME;
        int lv = charadata.LV;

        // �e�L�X�g�v�f�ɖ��O�ƃ��x����\��
        NAME.text = charaname;
        string lvtext = ($"LV{lv}");
        LV.text = lvtext;
    }
}