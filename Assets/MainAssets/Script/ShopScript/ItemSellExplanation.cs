using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSellExplanation : MonoBehaviour
{
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private ToggleGroup togglegroup;
    [SerializeField] private Text itemname;
    [SerializeField] private Text itemsetumei;
    [SerializeField] private Text itemBuyingPrice;
    [SerializeField] private Text itemSellingPrice;
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();
    private List<ItemData> MotimonoList = new List<ItemData>();
    private const int InitialWoodenSwordIndex = 0;
    private const int InitialClothArmorIndex = 3;

    void Start()
    {
        // ����������
    }

    public void UpdateItemKazu(Dictionary<ItemData, int> newItemKazu)
    {
        itemkazu = newItemKazu;
    }
    public void Motimonokoushin()
    {
        // ���������X�g���N���A
        MotimonoList.Clear();

        // �����Ă������1�ȏ�̃A�C�e�������������X�g�ɒǉ�����
        foreach (var item in itemkazu)
        {
            if (item.Value > 0)
            {
                MotimonoList.Add(item.Key);
            }
        }

        // ���������X�g�̓��e�����O�ɏo��
        Debug.Log("Updated MotimonoList:");
        foreach (var item in MotimonoList)
        {
            Debug.Log($"Item: {item.GetItemName()}, Count: {itemkazu[item]}");
        }
    }


    public void slotkoushin()
    {
        // �X���b�g�X�V����
    }

    public void DisplayItemExplanation(int itemIndex)
    {
        // �A�C�e���̐�����\�����鏈��
    }

    public List<ItemData> GetItemList()
    {
        return itemDataBase.GetItemList();
    }

    public ToggleGroup GetToggleGroup()
    {
        return togglegroup;
    }

    public void UpdateInventory()
    {
        // �C���x���g�����X�V���鏈��
    }
}
