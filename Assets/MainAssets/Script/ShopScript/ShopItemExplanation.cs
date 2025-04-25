using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemExplanation : MonoBehaviour
{
    // �A�C�e���f�[�^�x�[�X�̎擾
    [SerializeField] private ItemDataBase itemDataBase;

    // �A�C�e�������\����
    [SerializeField] private Text itemname;
    [SerializeField] private Text itemsetumei;
    [SerializeField] private Text itemBuyPrice;
    [SerializeField] private Text itemSellPrice;

    // �A�C�e���̐�����\�����郁�\�b�h
    public void DisplayItemExplanation(int itemIndex)
    {
        // �A�C�e���C���f�b�N�X���L�����ǂ������`�F�b�N
        if (itemIndex < 0 || itemIndex >= itemDataBase.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
            return;
        }

        // �A�C�e���f�[�^���擾
        ItemData item = itemDataBase.GetItemList()[itemIndex];

        // �A�C�e���̖��̂Ɛ������e�L�X�g�ɐݒ�
        itemname.text = item.GetItemName();
        itemsetumei.text = item.GetItemExplanation();
        itemBuyPrice.text = "�w�����i: " + item.GetItemBuyingPrice().ToString();
        itemSellPrice.text = "���p���i: " + item.GetItemSellingPrice().ToString();
    }
}
