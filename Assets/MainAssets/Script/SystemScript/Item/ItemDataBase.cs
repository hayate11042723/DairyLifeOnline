using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    // �A�C�e���f�[�^�̃��X�g
    [SerializeField]
    private List<ItemData> itemList = new List<ItemData>();

    // �A�C�e�����X�g��Ԃ�
    public List<ItemData> GetItemList()
    {
        return itemList;
    }

    // �A�C�e��������A�C�e���f�[�^���擾���郁�\�b�h
    public ItemData GetItemByName(string itemName)
    {
        return itemList.Find(item => item.GetItemName() == itemName);
    }
}
