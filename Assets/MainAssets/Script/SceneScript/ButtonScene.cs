using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScene : MonoBehaviour
{
    // �J�ڐ�̃V�[�������w�肷��
    public string nextSceneName;
    public Vector3 pos;
    private GameObject playerObject;
    private GameObject cameraObject;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

        if (playerObject == null || cameraObject == null)
        {
            Debug.LogError("Player�܂��̓J�����I�u�W�F�N�g��������܂���B");
            return;
        }
        DontDestroyOnLoad(playerObject);
        DontDestroyOnLoad(cameraObject);
    }

    // �{�^�����������Ƃ��ɌĂяo����郁�\�b�h
    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
        // Player�̑J�ڌ�̍��W
        playerObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        // �J�����̑J�ڌ�̍��W
        cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 3f);
    }
}