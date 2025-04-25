using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public Vector3 pos; // �V�[���J�ڌ�̃v���C���[�̈ʒu
    public GameObject cameraObject; // �J�����I�u�W�F�N�g
    GameObject[] tagObjects; // "Player"�^�O�����I�u�W�F�N�g�̔z��
    public string tagname; // �V�[���J�ڂ̃g���K�[�ƂȂ�I�u�W�F�N�g�̃^�O��
    public string sceneName; // �J�ڐ�̃V�[����

    private void Awake()
    {
        // "Player"�^�O�����I�u�W�F�N�g�����ׂĎ擾
        tagObjects = GameObject.FindGameObjectsWithTag("Player");

        if (tagObjects.Length >= 2)
        {
            // 2�ȏ��"Player"�^�O�����I�u�W�F�N�g�����݂���ꍇ
            // �폜�O�Ƀ��X�i�[������
            var existingScript = tagObjects[1].GetComponent<FocusOnEnemy>();
            if (existingScript != null)
            {
                existingScript.enabled = false;
                existingScript.OnDisable();
            }

            // 2�Ԗڂ�"Player"�^�O�����I�u�W�F�N�g���폜
            Destroy(tagObjects[1].gameObject);
            // �J�����I�u�W�F�N�g���폜
            Destroy(cameraObject);
        }
        else
        {
            // �I�u�W�F�N�g���V�[���J�ڌ���j�����Ȃ��悤�ɐݒ�
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(cameraObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // �V�[���J�ڂ̏���
        if (other.CompareTag(tagname))
        {
            // �V�[���̓ǂݍ���
            SceneManager.LoadScene(sceneName);
            // Player�̑J�ڌ�̍��W��ݒ�
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
            // �J�����̑J�ڌ�̍��W��ݒ�
            cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 3f);
        }
    }
}