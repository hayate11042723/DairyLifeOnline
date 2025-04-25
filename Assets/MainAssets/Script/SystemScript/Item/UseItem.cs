using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{
    [SerializeField] private GameObject itemKanriObject;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerDamage playerDamage; // PlayerDamage���Q��
    [SerializeField] private Button usePotionButton;
    [SerializeField] private Text messageText; // ���b�Z�[�W�\���p��Text

    private ItemKanri itemKanriScript;
    private Slider hpSlider; // HP�X���C�_�[
    private Coroutine messageCoroutine; // ���b�Z�[�W�\���p�̃R���[�`��

    private const float ButtonReenableDelay = 1f; // �{�^�����ēx�L���ɂ���܂ł̒x������

    void Start()
    {
        itemKanriScript = itemKanriObject.GetComponent<ItemKanri>();
        if (itemKanriScript == null)
        {
            return;
        }

        // �^�O���g�p����HP�X���C�_�[���擾
        GameObject hpSliderObject = GameObject.FindWithTag("PlayerSlider");
        if (hpSliderObject == null)
        {
            return;
        }

        hpSlider = hpSliderObject.GetComponent<Slider>();
        if (hpSlider == null)
        {
            return;
        }

        // �����̃��X�i�[���N���A���Ă���V�������X�i�[��ǉ�
        usePotionButton.onClick.RemoveAllListeners();
        usePotionButton.onClick.AddListener(OnUsePotionButtonClick);

        // Toggle�̏�Ԃ��ς�邽�тɃ{�^���̕\�����X�V
        var toggleGroup = itemKanriScript.GetToggleGroup();
        if (toggleGroup == null)
        {
            return;
        }

        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(delegate { UpdateUsePotionButtonVisibility(); });
        }

        // ������Ԃ̃{�^���\�����X�V
        UpdateUsePotionButtonVisibility();
        // ���b�Z�[�W���\���ɂ���
        messageText.gameObject.SetActive(false);
    }

    void OnUsePotionButtonClick()
    {
        // �{�^�����ꎞ�I�ɖ����ɂ���
        usePotionButton.interactable = false;

        // HP��MAX�̏ꍇ�̓|�[�V�������g�p�ł��Ȃ�
        if (playerStatus.HP >= playerStatus.MAXHP)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            messageCoroutine = StartCoroutine(ShowMessage("����ȏ�񕜂��܂���"));
            StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
            return;
        }

        // �C���x���g�����Ń|�[�V������I�����Ă��邩�m�F
        var toggleGroup = itemKanriScript.GetToggleGroup();
        if (toggleGroup == null)
        {
            StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
            return;
        }

        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            string toggleName = activeToggle.name;
            if (int.TryParse(toggleName, out int index))
            {
                var motimonoList = itemKanriScript.GetMotimonoList();
                if (motimonoList == null)
                {
                    StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
                    return;
                }

                // �C���f�b�N�X��1����n�܂�ꍇ�̒���
                if (index > 0 && index <= motimonoList.Count)
                {
                    ItemData selectedItem = motimonoList[index - 1];
                    if (selectedItem.GetItemType() == ItemData.itemtype.Portion)
                    {
                        // �|�[�V������1����
                        itemKanriScript.DecreaseItem(selectedItem, 1);
                        itemKanriScript.Motimonokoushin();
                        itemKanriScript.slotkoushin();

                        // �v���C���[��HP��6����
                        int healAmount = Mathf.FloorToInt(playerStatus.MAXHP * 0.6f);
                        playerStatus.HP = Mathf.Min(playerStatus.HP + healAmount, playerStatus.MAXHP);

                        // HP�X���C�_�[���X�V
                        if (hpSlider != null)
                        {
                            hpSlider.value = (float)playerStatus.HP / playerStatus.MAXHP;
                        }

                        // HP�e�L�X�g���X�V
                        playerDamage.UpdateHPText();
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid index: {index}. MotimonoList size: {motimonoList.Count}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid toggle name: {toggleName}");
            }
        }

        // ��莞�Ԍ�Ƀ{�^�����ēx�L���ɂ���
        StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
    }

    void UpdateUsePotionButtonVisibility()
    {
        Debug.Log("Updating button visibility...");
        var toggleGroup = itemKanriScript.GetToggleGroup();
        if (toggleGroup == null)
        {
            Debug.LogWarning("ToggleGroup is null.");
            usePotionButton.gameObject.SetActive(false);
            return;
        }

        // �A�N�e�B�u�ȃg�O�����擾
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            string toggleName = activeToggle.name;
            Debug.Log($"Active toggle: {toggleName}");

            // �g�O�������C���f�b�N�X�Ƃ��ĉ���
            if (int.TryParse(toggleName, out int index))
            {
                var motimonoList = itemKanriScript.GetMotimonoList();
                if (motimonoList == null)
                {
                    Debug.LogWarning("MotimonoList is null.");
                    usePotionButton.gameObject.SetActive(false);
                    return;
                }

                // �C���f�b�N�X���͈͓����m�F
                if (index > 0 && index <= motimonoList.Count)
                {
                    ItemData selectedItem = motimonoList[index - 1];
                    if (selectedItem != null)
                    {
                        // �I�����ꂽ�A�C�e�����|�[�V�������ǂ������m�F
                        bool isPotion = selectedItem.GetItemType() == ItemData.itemtype.Portion;
                        Debug.Log($"Selected item: {selectedItem.GetItemName()}, IsPotion: {isPotion}");
                        usePotionButton.gameObject.SetActive(isPotion);
                        return;
                    }
                    else
                    {
                        Debug.LogWarning($"Selected item is null. Index: {index}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Index out of range: {index}. MotimonoList size: {motimonoList.Count}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid toggle name: {toggleName}");
            }
        }
        else
        {
            Debug.LogWarning("No active toggle found.");
        }

        // �|�[�V�������I������Ă��Ȃ��ꍇ�̓{�^�����\���ɂ���
        usePotionButton.gameObject.SetActive(false);
    }


    IEnumerator ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        Color originalColor = messageText.color;
        messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);

        yield return new WaitForSeconds(2);

        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, t);
            yield return null;
        }

        messageText.gameObject.SetActive(false);
    }

    IEnumerator ReenableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        usePotionButton.interactable = true;
    }
}