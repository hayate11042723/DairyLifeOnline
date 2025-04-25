using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemData")]
public class ItemData : ScriptableObject
{
    public enum itemtype
    {
        Sword, Armor, Portion
    }

    [SerializeField]
    private string ItemName; // �A�C�e���̖��O
    [SerializeField]
    private itemtype ItemType; // �A�C�e���̃^�C�v
    [SerializeField]
    private Sprite ItemIcon; // �A�C�e���̃A�C�R��
    [SerializeField]
    private string ItemExplanation; // �A�C�e���̐���
    [SerializeField]
    private int ItemLimit; // �A�C�e���̎��Ă�ő��
    [SerializeField]
    private int ItemBuyingPrice; // �A�C�e���̍w�����i
    [SerializeField]
    private int ItemSellingPrice; // �A�C�e���̔̔����i
    [SerializeField]
    private int ATK; // �U���� (Sword�̏ꍇ)
    [SerializeField]
    private int DFE; // �h��� (Armor�̏ꍇ)

    public string GetItemName()
    {
        return ItemName;
    }

    public itemtype GetItemType()
    {
        return ItemType;
    }

    public Sprite GetItemIcon()
    {
        return ItemIcon;
    }

    public string GetItemExplanation()
    {
        return ItemExplanation;
    }

    public int GetItemLimit()
    {
        return ItemLimit;
    }

    public int GetItemBuyingPrice()
    {
        return ItemBuyingPrice;
    }

    public int GetItemSellingPrice()
    {
        return ItemSellingPrice;
    }

    public int GetATK()
    {
        if (ItemType == itemtype.Sword)
        {
            return ATK;
        }
        return 0;
    }

    public int GetDFE()
    {
        if (ItemType == itemtype.Armor)
        {
            return DFE;
        }
        return 0;
    }
}
