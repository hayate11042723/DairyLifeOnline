using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    // �V���b�v�̃E�B���h�E
    [SerializeField] private Canvas shopCanvas;
    // �v���C���[�̈ړ��X�N���v�g
    [SerializeField] private MonoBehaviour playerMove;
    // �v���C���[�̍U���X�N���v�g
    [SerializeField] private MonoBehaviour playerAttack;
    // ��A�N�e�B�u�ɂ���UI�L�����o�X
    [SerializeField] private List<Canvas> falseCanvases;
    // �A�N�e�B�u�ɂ���UI�L�����o�X
    [SerializeField] private List<Canvas> trueCanvases;
    // NPC��Transform
    [SerializeField] private Transform npcTransform;
    // �v���C���[��Transform
    [SerializeField] private Transform playerTransform;

    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void OnButtonClick()
    {
        // Canvas���A�N�e�B�u�ɐݒ�
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(true);
            // ����Canvas���A�N�e�B�u�ɐݒ�
            foreach (var canvas in falseCanvases)
            {
                if (canvas != shopCanvas)
                {
                    canvas.gameObject.SetActive(false);
                }
            }
            // �v���C���[�̈ړ��ƍU���𖳌��ɂ���
            if (playerMove != null)
            {
                playerMove.enabled = false;
            }
            if (playerAttack != null)
            {
                playerAttack.enabled = false;
            }
            // �v���C���[�̌�����NPC�̕��֌�����
            if (playerTransform != null && npcTransform != null)
            {
                Vector3 directionToNPC = (npcTransform.position - playerTransform.position).normalized;
                playerTransform.forward = directionToNPC;

                // NPC�̌������v���C���[�̕��֌�����
                Vector3 directionToPlayer = (playerTransform.position - npcTransform.position).normalized;
                npcTransform.forward = directionToPlayer;
            }
        }
    }

    // Canvas����郁�\�b�h
    public void CloseShop()
    {
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(false);
            // ����Canvas���ăA�N�e�B�u�ɐݒ�
            foreach (var canvas in trueCanvases)
            {
                canvas.gameObject.SetActive(true);
            }
            // �v���C���[�̈ړ��ƍU�����ėL��������
            if (playerMove != null)
            {
                playerMove.enabled = true;
            }
            if (playerAttack != null)
            {
                playerAttack.enabled = true;
            }
        }
    }
}
