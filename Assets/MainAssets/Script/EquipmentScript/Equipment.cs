using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [SerializeField] private GameObject itemKanriUI; // �A�C�e���Ǘ�UI
    [SerializeField] private PlayerStatus playerStatus; // �v���C���[�̃X�e�[�^�X�f�[�^
    [SerializeField] private Image weaponEquipMark; // ���푕����������Image UI
    [SerializeField] private Image armorEquipMark;  // �h�����������Image UI
    [SerializeField] private StatusWindow statusWindow; // �X�e�[�^�X�E�B���h�E

    private ItemData currentWeapon; // ���ݑ������Ă��镐��
    private ItemData currentArmor;  // ���ݑ������Ă���h��

    void Start()
    {
        // ����������ݒ�
        SetInitialEquipment();

        // ��{�X�e�[�^�X��������
        playerStatus.InitializeBaseStats();
    }


    private void SetInitialEquipment()
    {
        // �A�C�e���Ǘ��X�N���v�g���擾
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return;
        }

        // �����������擾
        var woodenSword = itemKanri.GetItemByName("Wooden Sword");
        var clothArmor = itemKanri.GetItemByName("DefaultClothes");

        // ����𑕔�
        if (woodenSword != null)
        {
            EquipWeapon(woodenSword);
        }
        else
        {
            Debug.LogWarning("Initial weapon (Wooden Sword) not found.");
        }

        // �h��𑕔�
        if (clothArmor != null)
        {
            EquipArmor(clothArmor);
        }
        else
        {
            Debug.LogWarning("Initial armor (DefaultClothes) not found.");
        }
    }

    public void EquipSelectedItem()
    {
        // �A�C�e���Ǘ��X�N���v�g���擾
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return;
        }

        // �I������Ă���A�C�e�����擾
        var selectedItem = GetSelectedItem(itemKanri);
        if (selectedItem == null)
        {
            Debug.LogWarning("No item is selected or item is not valid.");
            return;
        }

        // �����\���ǂ����𔻒�
        if (selectedItem.GetItemType() == ItemData.itemtype.Sword)
        {
            EquipWeapon(selectedItem);
        }
        else if (selectedItem.GetItemType() == ItemData.itemtype.Armor)
        {
            EquipArmor(selectedItem);
        }
        else
        {
            Debug.LogWarning("Selected item is not equippable.");
        }
    }

    private ItemData GetSelectedItem(ItemKanri itemKanri)
    {
        // �g�O���O���[�v����I������Ă���A�C�e�����擾
        var toggleGroup = itemKanri.GetToggleGroup();
        if (toggleGroup == null)
        {
            Debug.LogWarning("ToggleGroup is null.");
            return null;
        }

        var activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle == null)
        {
            Debug.LogWarning("No active toggle found.");
            return null;
        }

        if (int.TryParse(activeToggle.name, out int index))
        {
            var motimonoList = itemKanri.GetMotimonoList();
            if (index > 0 && index <= motimonoList.Count)
            {
                return motimonoList[index - 1];
            }
        }

        return null;
    }

    private void EquipWeapon(ItemData newWeapon)
    {
        // �����̕��������
        if (currentWeapon != null)
        {
            UnequipWeapon(); // ���݂̕�����O��
        }

        // �V��������𑕔�
        currentWeapon = newWeapon;
        Debug.Log($"Equipped weapon: {newWeapon.GetItemName()}");

        // �������}�[�N���X�V
        UpdateEquipMark(weaponEquipMark, newWeapon);

        // �v���C���[�̃X�e�[�^�X�����Z�b�g
        ResetPlayerStatus();
    }

    private void EquipArmor(ItemData newArmor)
    {
        // �����̖h�������
        if (currentArmor != null)
        {
            UnequipArmor(); // ���݂̖h����O��
        }

        // �V�����h��𑕔�
        currentArmor = newArmor;
        Debug.Log($"Equipped armor: {newArmor.GetItemName()}");

        // �������}�[�N���X�V
        UpdateEquipMark(armorEquipMark, newArmor);

        // �v���C���[�̃X�e�[�^�X�����Z�b�g
        ResetPlayerStatus();
    }

    private void RemoveItemFromEquipment(ItemData item)
    {
        // �������������鏈���i�K�v�ɉ����ăC���x���g���ɖ߂��Ȃǁj
        Debug.Log($"Removed item from equipment: {item.GetItemName()}");
    }

    private void UpdateEquipMark(Image equipMark, ItemData item)
    {
        // �������}�[�N���A�C�e���̃A�C�R���ɏd�˂�
        if (equipMark != null)
        {
            // �A�C�R���̈ʒu���擾
            var itemIcon = FindItemIcon(item);
            if (itemIcon != null)
            {
                // �������}�[�N���A�C�R���̈ʒu�Ɉړ�
                RectTransform equipMarkRect = equipMark.GetComponent<RectTransform>();
                RectTransform itemIconRect = itemIcon.GetComponent<RectTransform>();

                if (equipMarkRect != null && itemIconRect != null)
                {
                    equipMarkRect.SetParent(itemIconRect, false); // �A�C�R���̎q�v�f�ɐݒ�
                    equipMarkRect.anchoredPosition = Vector2.zero; // �A�C�R���̒����ɔz�u
                    equipMarkRect.sizeDelta = itemIconRect.sizeDelta; // �A�C�R���Ɠ����T�C�Y�ɐݒ�
                }

                equipMark.gameObject.SetActive(true); // �������}�[�N��\��
            }
        }
    }

    private GameObject FindItemIcon(ItemData item)
    {
        // �A�C�e���̃A�C�R��������
        ItemKanri itemKanri = itemKanriUI.GetComponent<ItemKanri>();
        if (itemKanri == null)
        {
            Debug.LogError("ItemKanri component is not found on itemKanriUI.");
            return null;
        }

        var icons = itemKanri.GetIcons(); // �A�C�R���̃��X�g���擾
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] != null && icons[i].sprite == item.GetItemIcon())
            {
                return icons[i].gameObject; // �Y������A�C�R����Ԃ�
            }
        }

        return null; // �Y������A�C�R����������Ȃ��ꍇ
    }


    private void UpdatePlayerStatus()
    {
        // �v���C���[�̃X�e�[�^�X���X�V
        playerStatus.ATK = currentWeapon != null ? playerStatus.BaseATK * currentWeapon.GetATK() : playerStatus.BaseATK;
        playerStatus.DEF = currentArmor != null ? playerStatus.BaseDEF * currentArmor.GetDFE() : playerStatus.BaseDEF;

        Debug.Log($"Player status updated: ATK={playerStatus.ATK}, DEF={playerStatus.DEF}");

        // �X�e�[�^�X�E�B���h�E���X�V
        if (statusWindow != null)
        {
            statusWindow.UpdateUI();
        }
    }

    private void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Debug.Log($"Unequipping weapon: {currentWeapon.GetItemName()}");
            RemoveItemFromEquipment(currentWeapon);
            currentWeapon = null;

            // �X�e�[�^�X�����Z�b�g
            ResetPlayerStatus();

            weaponEquipMark.gameObject.SetActive(false); // �������}�[�N���\��
        }
    }

    private void UnequipArmor()
    {
        if (currentArmor != null)
        {
            Debug.Log($"Unequipping armor: {currentArmor.GetItemName()}");
            RemoveItemFromEquipment(currentArmor);
            currentArmor = null;

            // �X�e�[�^�X�����Z�b�g
            ResetPlayerStatus();

            armorEquipMark.gameObject.SetActive(false); // �������}�[�N���\��
        }
    }

    private void ResetWeaponStats()
    {
        // ����ɂ��X�e�[�^�X�㏸�l�����Z�b�g
        playerStatus.ATK = playerStatus.BaseATK; // BaseATK�͌���ATK�l
    }

    private void ResetArmorStats()
    {
        // �h��ɂ��X�e�[�^�X�㏸�l�����Z�b�g
        playerStatus.DEF = playerStatus.BaseDEF; // BaseDEF�͌���DEF�l
    }

    private void ResetPlayerStatus()
    {
        // ���킪��������Ă��Ȃ��ꍇ�A��{�l�Ƀ��Z�b�g
        playerStatus.ATK = currentWeapon != null ? playerStatus.BaseATK + currentWeapon.GetATK() : playerStatus.BaseATK;

        // �h���������Ă��Ȃ��ꍇ�A��{�l�Ƀ��Z�b�g
        playerStatus.DEF = currentArmor != null ? playerStatus.BaseDEF + currentArmor.GetDFE() : playerStatus.BaseDEF;

        Debug.Log($"Player status reset: ATK={playerStatus.ATK}, DEF={playerStatus.DEF}");

        // �X�e�[�^�X�E�B���h�E���X�V
        if (statusWindow != null)
        {
            statusWindow.UpdateUI();
        }
    }

}
