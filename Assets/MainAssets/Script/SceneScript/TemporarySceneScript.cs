using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporarySceneScript : MonoBehaviour
{
    public string tagname; // �V�[���J�ڂ̃g���K�[�ƂȂ�I�u�W�F�N�g�̃^�O��
    public string sceneName; // �J�ڐ�̃V�[����

    // �J�ڐ�̃V�[���Œ�������I�u�W�F�N�g�̖��O
    public string targetObjectName;

    // �I�u�W�F�N�g�̐V�����ʒu
    public Vector3 newPosition;

    // �I�u�W�F�N�g�̐V��������
    public Vector3 newRotation;

    private void OnTriggerStay(Collider other)
    {
        // �V�[���J�ڂ̏���
        if (other.CompareTag(tagname))
        {
            // �I�u�W�F�N�g�̈ʒu����ۑ�
            PlayerPrefs.SetString("TargetObjectName", targetObjectName);
            PlayerPrefs.SetFloat("NewPositionX", newPosition.x);
            PlayerPrefs.SetFloat("NewPositionY", newPosition.y);
            PlayerPrefs.SetFloat("NewPositionZ", newPosition.z);

            // �I�u�W�F�N�g�̌�������ۑ�
            PlayerPrefs.SetFloat("NewRotationX", newRotation.x);
            PlayerPrefs.SetFloat("NewRotationY", newRotation.y);
            PlayerPrefs.SetFloat("NewRotationZ", newRotation.z);

            // �V�[���̓ǂݍ���
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName);
        }
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

                float newRotationX = PlayerPrefs.GetFloat("NewRotationX");
                float newRotationY = PlayerPrefs.GetFloat("NewRotationY");
                float newRotationZ = PlayerPrefs.GetFloat("NewRotationZ");
                Quaternion newRotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);

                // �I�u�W�F�N�g�̈ʒu�ƌ����𒲐�
                targetObject.transform.position = newPosition;
                targetObject.transform.rotation = newRotation;
            }
        }

        // �C�x���g�̉���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
