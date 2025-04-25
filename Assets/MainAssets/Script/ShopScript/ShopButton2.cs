using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton2 : MonoBehaviour
{
    // �V���b�v�̃E�B���h�E
    [SerializeField] private Canvas shopCanvas;
    // ����UI�L�����o�X
    [SerializeField] private List<Canvas> otherCanvases;

    // �{�^�����N���b�N���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void OnButtonClick()
    {
        // Canvas���A�N�e�B�u�ɐݒ�
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(true);
            // ����Canvas���A�N�e�B�u�ɐݒ�
            foreach (var canvas in otherCanvases)
            {
                if (canvas != shopCanvas)
                {
                    canvas.gameObject.SetActive(false);
                }
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
            foreach (var canvas in otherCanvases)
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }
}
