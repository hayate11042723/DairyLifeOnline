using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporaryButtonScene : MonoBehaviour
{
    // �J�ڐ�̃V�[�������w�肷��
    public string nextSceneName;

    // �J�ڐ�̃V�[���Œ�������I�u�W�F�N�g�̖��O
    public string targetObjectName;

    // �I�u�W�F�N�g�̐V�����ʒu
    public Vector3 newPosition;

    // �{�^�����������Ƃ��ɌĂяo����郁�\�b�h
    public void ChangeScene()
    {
        // �I�u�W�F�N�g�̈ʒu����ۑ�
        PlayerPrefs.SetString("TargetObjectName", targetObjectName);
        PlayerPrefs.SetFloat("NewPositionX", newPosition.x);
        PlayerPrefs.SetFloat("NewPositionY", newPosition.y);
        PlayerPrefs.SetFloat("NewPositionZ", newPosition.z);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(nextSceneName);
    }

    // �V�[�������[�h���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �ۑ����ꂽ�I�u�W�F�N�g�̈ʒu�����擾
        string targetObjectName = PlayerPrefs.GetString("TargetObjectName", "");
        if (!string.IsNullOrEmpty(targetObjectName))
        {
            GameObject targetObject = GameObject.Find(targetObjectName);
            if (targetObject != null)
            {
                float newPositionX = PlayerPrefs.GetFloat("NewPositionX");
                float newPositionY = PlayerPrefs.GetFloat("NewPositionY");
                float newPositionZ = PlayerPrefs.GetFloat("NewPositionZ");
                Vector3 newPosition = new Vector3(newPositionX, newPositionY, newPositionZ);

                // �I�u�W�F�N�g�̈ʒu�𒲐�
                targetObject.transform.position = newPosition;
            }
        }

        // �C�x���g�̉���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}