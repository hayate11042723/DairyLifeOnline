using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMuki : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ肷��J����
    public Camera targetCamera;

    // Update is called once per frame
    void Update()
    {
        if (targetCamera != null)
        {
            // �J�����̌������擾
            Vector3 cameraForward = targetCamera.transform.forward;
            cameraForward.y = 0; // �㉺�̌����𖳎�
            cameraForward.Normalize(); // ���K��

            // �J�����̌����ɍ��킹�ăI�u�W�F�N�g�̌�����ݒ�
            transform.rotation = Quaternion.LookRotation(cameraForward);
        }
    }
}