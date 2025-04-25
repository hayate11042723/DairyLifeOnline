using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBack : MonoBehaviour
{
    public GameObject currentCanvas; // ���ݕ\������Ă���L�����o�X
    public GameObject targetCanvas;  // �؂�ւ���̃L�����o�X
    public GameObject inventoryCanvas; // Inventory���ڂ���̃L�����o�X
    public GameObject itemKanriUI;   // �C���x���g��UI

    private Transform originalParent; // ���̐e�v�f��ێ�����ϐ�

    void Start()
    {
        // itemKanriUI���ݒ肳��Ă��邩�m�F
        if (itemKanriUI == null)
        {
            Debug.LogError("itemKanriUI is not assigned.");
            return;
        }

        // ���̐e�v�f��ۑ�
        originalParent = itemKanriUI.transform.parent;
    }

    public void SwitchCanvas()
    {
        // ���݂�Canvas���\���ɂ��A�{�^���𖳌���
        if (currentCanvas != null)
        {
            SetButtonsInteractable(currentCanvas, false);
            currentCanvas.SetActive(false);
        }

        // �؂�ւ����Canvas��\�����A�{�^����L����
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true);
            SetButtonsInteractable(targetCanvas, true);
        }

        // Inventory���ڂ����Canvas�ɐݒ�
        if (inventoryCanvas != null && itemKanriUI != null)
        {
            itemKanriUI.transform.SetParent(inventoryCanvas.transform, false);
            Debug.Log($"Inventory moved to the inventory Canvas: {inventoryCanvas.name}");
        }
    }

    public void ResetInventoryParent()
    {
        // Inventory�����̐e�v�f�ɖ߂�
        if (itemKanriUI != null && originalParent != null)
        {
            itemKanriUI.transform.SetParent(originalParent, false);
            Debug.Log("Inventory returned to its original parent.");
        }
    }

    private void SetButtonsInteractable(GameObject canvas, bool interactable)
    {
        Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }
    }
}
