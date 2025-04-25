using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuy : MonoBehaviour
{
    // �A�C�e���f�[�^�x�[�X
    [SerializeField] private ItemDataBase itemDataBase;
    // �A�C�e���Ǘ��X�N���v�g
    [SerializeField] private ItemKanri itemManager;
    // �v���C���[�X�e�[�^�X
    [SerializeField] private PlayerStatus playerStatus;
    // ��������\������e�L�X�g
    [SerializeField] private Text playerMoneyText;
    // ���b�Z�[�W��\������e�L�X�g
    [SerializeField] private Text messageText;
    // �N���b�N�C�x���g�̃f�o�E���X�p
    private bool isBuying = false;
    // ���݂̃t�F�[�h�A�E�g�R���[�`�����Ǘ����邽�߂̃t�B�[���h
    private Coroutine fadeOutCoroutine;

    void Start()
    {
        messageText.gameObject.SetActive(false); // ���b�Z�[�W���\���ɂ���
        UpdatePlayerMoneyText(); // �������̏����\����ݒ�
    }

    // �A�C�e�����w�����郁�\�b�h
    public void BuyItem(int itemIndex)
    {
        // �f�o�E���X����
        if (isBuying) return;
        isBuying = true;

        // �f�o�b�O���O
        Debug.Log("BuyItem called");

        // �A�C�e���C���f�b�N�X���L�����ǂ������`�F�b�N
        if (itemIndex < 0 || itemIndex >= itemDataBase.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
            isBuying = false;
            return;
        }

        // �w������A�C�e�����擾
        ItemData item = itemDataBase.GetItemList()[itemIndex];
        // �A�C�e���̍w�����i���擾
        int itemPrice = item.GetItemBuyingPrice();

        // �v���C���[�̏��������A�C�e���̉��i�ȏォ�ǂ������`�F�b�N
        if (playerStatus.HAVEGOLD >= itemPrice)
        {
            // �����������炷
            playerStatus.HAVEGOLD -= itemPrice;
            // �A�C�e����ǉ�
            itemManager.AddItem(item);
            Debug.Log($"Bought {item.GetItemName()} for {itemPrice} gold. Remaining gold: {playerStatus.HAVEGOLD}");
            UpdatePlayerMoneyText(); // �������̕\�����X�V
        }
        else
        {
            // ������������Ȃ��ꍇ�̃��b�Z�[�W
            messageText.text = "������������܂���";
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine); // ���݂̃t�F�[�h�A�E�g�R���[�`�����~
            }
            fadeOutCoroutine = StartCoroutine(FadeOutMessage()); // �V�����t�F�[�h�A�E�g�R���[�`�����J�n
        }

        // �f�o�E���X����
        StartCoroutine(ResetIsBuying());
    }

    // �f�o�E���X�����p�̃R���[�`��
    private IEnumerator ResetIsBuying()
    {
        yield return new WaitForSeconds(0.1f); // 0.1�b�ҋ@
        isBuying = false;
    }

    // �������̕\�����X�V���郁�\�b�h
    private void UpdatePlayerMoneyText()
    {
        playerMoneyText.text = $"������: {playerStatus.HAVEGOLD}G";
    }

    // ���b�Z�[�W���t�F�[�h�A�E�g������R���[�`��
    private IEnumerator FadeOutMessage()
    {
        messageText.gameObject.SetActive(true); // ���b�Z�[�W��\��

        Color originalColor = messageText.color;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / 2.0f) // 2�b�����ăt�F�[�h�A�E�g
        {
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        messageText.gameObject.SetActive(false); // ���b�Z�[�W���\���ɂ���
        messageText.color = originalColor; // ���̐F�ɖ߂�
    }
}