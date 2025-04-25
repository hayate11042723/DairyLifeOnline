using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ItemKanri : MonoBehaviour
{
    // �A�C�e���f�[�^�x�[�X
    [SerializeField] private ItemDataBase itemDataBase;
    // �A�C�e���A�C�R���̔z��
    [SerializeField] private GameObject[] icons = new GameObject[IconArraySize];
    // �g�O���O���[�v�̃��X�g
    [SerializeField] private List<ToggleGroup> toggleGroups;
    // �g�O���̃��X�g
    [SerializeField] private List<Toggle> toggles;
    // �A�C�e�����\����
    [SerializeField] private List<Text> itemname;
    // �A�C�e�������\����
    [SerializeField] private List<Text> itemsetumei;
    // �A�C�e�����Ǘ�
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();
    // ���������X�g
    private List<ItemData> MotimonoList = new List<ItemData>();
    // �����A�C�e�����X�g
    private List<ItemData> initialItems = new List<ItemData>();
    // �A�C�R���̔z��
    private Image[] Icons = new Image[IconArraySize];
    // ��X���b�g�̐F
    [SerializeField] private Color emptySlotColor;
    // ���܂��Ă���X���b�g�̐F
    [SerializeField] private Color filledSlotColor;
    // �����A�C�e���̃C���f�b�N�X
    private const int InitialWoodenSwordIndex = 0;
    private const int InitialDefaultClothesIndex = 3;
    // �A�C�R���z��̃T�C�Y
    private const int IconArraySize = 24;

    // �A�C�e���f�[�^�x�[�X�̎擾
    public Image[] GetIcons()
    {
        // �A�C�R����Image�R���|�[�l���g���擾���ĕԂ�
        Image[] iconImages = new Image[icons.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] != null)
            {
                iconImages[i] = icons[i].GetComponent<Image>();
            }
        }
        return iconImages;
    }

    void Start()
    {
        LoadItemData();

        // �������A�C�e������
        foreach (var item in itemDataBase.GetItemList())
        {
            // �A�C�e������S��0��
            if (!itemkazu.ContainsKey(item))
            {
                itemkazu[item] = 0;
            }
        }

        // �����Ă��鏉���A�C�e���ݒ�
        var initialWoodenSword = itemDataBase.GetItemList()[InitialWoodenSwordIndex];
        var initialDefaultClothes = itemDataBase.GetItemList()[InitialDefaultClothesIndex];
        itemkazu[initialWoodenSword] = 1; // �؂̌��̐���1�ɂ���
        itemkazu[initialDefaultClothes] = 1; // DefaultClothes�̐���1�ɂ���

        // �����A�C�e�����X�g�ɒǉ�
        initialItems.Add(initialWoodenSword);
        initialItems.Add(initialDefaultClothes);

        // �A�C�R����Image�R���|�[�l���g���擾
        for (int i = 0; i < IconArraySize; i++)
        {
            if (icons[i] != null)
            {
                Icons[i] = icons[i].GetComponent<Image>();
            }
            else
            {
                Debug.LogError($"Icon at index {i} is not assigned.");
            }
        }

        // �������X�V�������Ăяo��
        Motimonokoushin();
        slotkoushin(); // �X���b�g�X�V�������Ăяo��
    }

    void OnDestroy()
    {
        SaveItemData();
    }

    // �������X�V����
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

        // �A�C�R�����X�V
        UpdateIcons();
    }

    // �A�C�R�����X�V���郁�\�b�h
    private void UpdateIcons()
    {
        for (int i = 0; i < IconArraySize; i++)
        {
            if (Icons[i] != null)
            {
                if (i < MotimonoList.Count)
                {
                    Icons[i].sprite = MotimonoList[i].GetItemIcon();
                    Icons[i].color = filledSlotColor;
                }
                else
                {
                    Icons[i].sprite = null;
                    Icons[i].color = emptySlotColor;
                }
            }
        }
    }

    // �X���b�g�X�V����
    public void slotkoushin()
    {
        int displayIndex = 0;

        // �e�g�O���O���[�v������
        foreach (var toggleGroup in toggleGroups)
        {
            // �I�����ꂽ�g�O�����擾
            List<Toggle> selectedToggles = toggleGroup.ActiveToggles().ToList();

            foreach (var toggle in selectedToggles)
            {
                if (displayIndex < itemname.Count)
                {
                    // �g�O���̖��O����C���f�b�N�X���擾
                    if (int.TryParse(toggle.name, out int index))
                    {
                        // �C���f�b�N�X���͈͓����m�F
                        if (index >= 0 && index < MotimonoList.Count)
                        {
                            // �I�����ꂽ�A�C�e���̏����擾
                            string itemName = MotimonoList[index].GetItemName();
                            string itemDescription = MotimonoList[index].GetItemExplanation();
                            int itemCount = itemkazu[MotimonoList[index]];

                            // �A�C�e�����Ɛ�����ݒ�
                            itemname[displayIndex].text = $"{itemName} �~ {itemCount}";
                            itemsetumei[displayIndex].text = itemDescription;
                        }
                        else
                        {
                            itemname[displayIndex].text = "";
                            itemsetumei[displayIndex].text = "";
                        }
                    }
                    else
                    {
                        itemname[displayIndex].text = "";
                        itemsetumei[displayIndex].text = "";
                    }

                    displayIndex++;
                }
            }
        }

        // �c��̕\��������ɂ���
        for (int i = displayIndex; i < itemname.Count; i++)
        {
            itemname[i].text = "";
            itemsetumei[i].text = "";
        }
    }
    // �A�C�e�����X�g���擾���郁�\�b�h
    public List<ItemData> GetItemList()
    {
        return itemDataBase.GetItemList();
    }

    // ���������X�g�̃A�N�Z�T
    public List<ItemData> GetMotimonoList()
    {
        return MotimonoList;
    }

    // �A�C�e�������炷���\�b�h
    public void DecreaseItem(ItemData item, int amount)
    {
        if (itemkazu.ContainsKey(item))
        {
            itemkazu[item] -= amount;
            if (itemkazu[item] <= 0)
            {
                itemkazu[item] = 0;
            }
        }
        Motimonokoushin();
    }

    // �A�C�e�����������Ă��邩�ǂ������`�F�b�N���郁�\�b�h
    public bool HasItem(ItemData item)
    {
        return itemkazu.ContainsKey(item) && itemkazu[item] > 0;
    }

    // �A�C�e�����폜���郁�\�b�h
    public void RemoveItem(ItemData item)
    {
        if (itemkazu.ContainsKey(item))
        {
            itemkazu[item]--;
            if (itemkazu[item] <= 0)
            {
                itemkazu[item] = 0;
            }
        }
        Motimonokoushin();
    }

    // �A�C�e����ǉ����郁�\�b�h
    public void AddItem(ItemData item)
    {
        if (itemkazu.ContainsKey(item))
        {
            itemkazu[item]++;
        }
        else
        {
            itemkazu[item] = 1;
        }
        Motimonokoushin();
        // �A�C�e���������X�V
        var itemSellExplanation = FindObjectOfType<ItemSellExplanation>();
        itemSellExplanation.UpdateItemKazu(itemkazu);
        itemSellExplanation.Motimonokoushin();
        itemSellExplanation.slotkoushin();
    }

    // �����A�C�e�����ǂ������`�F�b�N���郁�\�b�h
    public bool IsInitialItem(ItemData item)
    {
        return initialItems.Contains(item);
    }

    public ToggleGroup GetToggleGroup()
    {
        if (toggles != null && toggles.Count > 0)
        {
            return toggles[0].group; // �ŏ��̃g�O���̃O���[�v��Ԃ�  
        }
        return null; // �g�O�������݂��Ȃ��ꍇ�� null ��Ԃ�  

    }

    // �A�C�e���f�[�^��ۑ����郁�\�b�h
    private void SaveItemData()
    {
        string path = Path.Combine(Application.persistentDataPath, "itemData.json");
        string json = JsonUtility.ToJson(new SerializableDictionary<string, int>(itemkazu.ToDictionary(k => k.Key.name, v => v.Value)));
        File.WriteAllText(path, json);
    }

    // �A�C�e���f�[�^�𕜌����郁�\�b�h
    private void LoadItemData()
    {
        string path = Path.Combine(Application.persistentDataPath, "itemData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<SerializableDictionary<string, int>>(json).ToDictionary();

            // �A�C�e���f�[�^�x�[�X����A�C�e�����������ĕ���
            foreach (var item in itemDataBase.GetItemList())
            {
                if (loadedData.ContainsKey(item.name))
                {
                    itemkazu[item] = loadedData[item.name];
                }
            }
        }

        // �A�C�e���f�[�^�����[�h������ɃA�C�e���̐������X�V
        Motimonokoushin();
        slotkoushin();
    }

    // �A�C�e��������A�C�e���f�[�^���擾���郁�\�b�h
    public ItemData GetItemByName(string itemName)
    {
        return itemDataBase.GetItemList().Find(item => item.GetItemName() == itemName);
    }
}

// �V���A���C�Y�\�Ȏ����N���X
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        this.dictionary = dictionary;
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return dictionary;
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
    }
}
