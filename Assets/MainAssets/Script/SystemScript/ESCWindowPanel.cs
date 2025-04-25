using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ESCWindowPanel : MonoBehaviour
{
    // �\��/��\����؂�ւ���Ώۂ̃p�l��
    public GameObject panel;

    // �g�O���p�̓��̓A�N�V����
    public InputAction toggleAction;

    // �{�^��
    public Button closeButton;

    private void OnEnable()
    {
        // ���̓A�N�V������L����
        toggleAction.Enable();
        toggleAction.performed += OnToggle;

        // �{�^���̃N���b�N�C�x���g��o�^
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel);
        }
    }

    private void OnDisable()
    {
        // ���̓A�N�V�����𖳌���
        toggleAction.performed -= OnToggle;
        toggleAction.Disable();

        // �{�^���̃N���b�N�C�x���g������
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(HidePanel);
        }
    }

    private void OnToggle(InputAction.CallbackContext context)
    {
        // �p�l���̃A�N�e�B�u��Ԃ�؂�ւ���
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    public void HidePanel()
    {
        // �p�l�����\���ɂ���
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
