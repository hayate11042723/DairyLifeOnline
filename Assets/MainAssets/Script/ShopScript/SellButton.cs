using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    [SerializeField] private GameObject itemKanriObject;
    [SerializeField] private GameObject ShopManager;
    [SerializeField] private PlayerStatus playerStatus; // �v���C���[�X�e�[�^�X���Q��
    [SerializeField] private Text playerMoneyText; // ��������\������e�L�X�g
    [SerializeField] private Text messageText; // ���b�Z�[�W��\������e�L�X�g
    [SerializeField] private Button sellButton; // ���p�{�^��

    private ItemKanri itemKanriScript;
    private ItemSellExplanation itemSellExplanationScript;

    void Start()
    {
        itemKanriScript = itemKanriObject.GetComponent<ItemKanri>();
        itemSellExplanationScript = ShopManager.GetComponent<ItemSellExplanation>();
        UpdatePlayerMoneyText();
        messageText.gameObject.SetActive(false); // ���b�Z�[�W���\���ɂ���
    }

    public void OnSellButtonClick()
    {
        Toggle activeToggle = itemSellExplanationScript.GetToggleGroup().ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            string toggleName = activeToggle.name;
            if (int.TryParse(toggleName, out int index))
            {
                // �C���f�b�N�X���͈͓����m�F
                var motimonoList = itemKanriScript.GetMotimonoList();
                if (index >= 0 && index < motimonoList.Count)
                {
                    ItemData selectedItem = motimonoList[index];
                    if (!itemKanriScript.IsInitialItem(selectedItem))
                    {
                        int sellingPrice = selectedItem.GetItemSellingPrice();
                        playerStatus.HAVEGOLD += sellingPrice; // �������𑝂₷
                        itemKanriScript.DecreaseItem(selectedItem, 1);
                        itemKanriScript.Motimonokoushin();
                        itemSellExplanationScript.Motimonokoushin();
                        itemSellExplanationScript.slotkoushin();
                        UpdatePlayerMoneyText(); // �������̕\�����X�V

                        // �A�C�e���̐������X�V
                        if (index < motimonoList.Count)
                        {
                            itemSellExplanationScript.DisplayItemExplanation(index);
                        }
                        else
                        {
                            // ���p��ɃA�C�e�����Ȃ��Ȃ����ꍇ�A�������N���A
                            itemSellExplanationScript.DisplayItemExplanation(-1);
                        }

                        messageText.text = ""; // ���b�Z�[�W���N���A
                    }
                    else
                    {
                        messageText.text = "�����A�C�e���͔��p�ł��܂���";
                        StartCoroutine(FadeOutMessage());
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
    }


    private void UpdatePlayerMoneyText()
    {
        playerMoneyText.text = $"������: {playerStatus.HAVEGOLD}G";
    }

    private IEnumerator FadeOutMessage()
    {
        messageText.gameObject.SetActive(true); // ���b�Z�[�W��\��
        sellButton.interactable = false; // ���p�{�^���𖳌��ɂ���

        Color originalColor = messageText.color;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / 2.0f) // 2�b�����ăt�F�[�h�A�E�g
        {
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        messageText.gameObject.SetActive(false); // ���b�Z�[�W���\���ɂ���
        messageText.color = originalColor; // ���̐F�ɖ߂�
        sellButton.interactable = true; // ���p�{�^�����ēx�L���ɂ���
    }

    // ���p��ʂɈڂ����Ƃ��ɃC���x���g�����X�V���郁�\�b�h
    public void OnEnterSellScreen()
    {
        itemSellExplanationScript.UpdateInventory();
    }
}

