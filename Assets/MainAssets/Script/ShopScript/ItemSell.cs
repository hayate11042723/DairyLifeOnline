using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSell : MonoBehaviour
{
    // �A�C�e���Ǘ��X�N���v�g
    [SerializeField] private ItemKanri itemManager;
    // �v���C���[�X�e�[�^�X
    [SerializeField] private PlayerStatus playerStatus;
    // �C���x���g��
    [SerializeField] private GameObject inventory;
    // �����Canvas
    [SerializeField] private Canvas targetCanvas;
    // ���̐e�I�u�W�F�N�g
    private Transform originalParent;

    private void Start()
    {
        // �C���x���g���̌��̐e�I�u�W�F�N�g��ۑ�
        originalParent = inventory.transform.parent;
    }

    // �A�C�e���𔄂郁�\�b�h
    public void SellItem(int itemIndex)
    {
        // �A�C�e���C���f�b�N�X���L�����ǂ������`�F�b�N
        if (itemIndex < 0 || itemIndex >= itemManager.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
            return;
        }

        // ���p����A�C�e�����擾
        ItemData item = itemManager.GetItemList()[itemIndex];
        // �A�C�e���̔��p���i���擾
        int itemPrice = item.GetItemSellingPrice();

        // �v���C���[���A�C�e�����������Ă��邩�ǂ������`�F�b�N
        if (itemManager.HasItem(item))
        {
            // �A�C�e�����폜
            itemManager.RemoveItem(item);
            // �������𑝂₷
            playerStatus.HAVEGOLD += itemPrice;
            Debug.Log($"Sold {item.GetItemName()} for {itemPrice} gold. Total gold: {playerStatus.HAVEGOLD}");
        }
        else
        {
            // �A�C�e�����������Ă��Ȃ��ꍇ�̃��b�Z�[�W
            Debug.Log("You don't have this item to sell");
        }
    }

    // �C���x���g��������Canvas�̎q�v�f�ɂ��郁�\�b�h
    public void MoveInventoryToCanvas()
    {
        inventory.transform.SetParent(targetCanvas.transform, false);
    }

    // �C���x���g�������̐e�I�u�W�F�N�g�ɖ߂����\�b�h
    public void ResetInventoryParent()
    {
        inventory.transform.SetParent(originalParent, false);
    }
}