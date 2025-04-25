using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindow : MonoBehaviour
{
    // �V���b�v�̃E�B���h�E
    [SerializeField] private Canvas shopCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Canvas���A�N�e�B�u�ɐݒ�
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[���R���C�_�[���ɓ�������
        if(other.CompareTag("Player"))
        {
            // Canvas���A�N�e�B�u�ɐݒ�
            if (shopCanvas != null)
            {
                shopCanvas.gameObject.SetActive(true);
            }
        }
    }
}
