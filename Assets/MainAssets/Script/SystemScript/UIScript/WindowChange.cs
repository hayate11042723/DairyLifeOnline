using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject currentCanvas; // ���ݕ\������Ă���L�����o�X
    public GameObject targetCanvas;  // �؂�ւ���̃L�����o�X

    public void SwitchCanvas()
    {
        if (currentCanvas != null)
        {
            SetButtonsInteractable(currentCanvas, false); // ���݂̃L�����o�X���̃{�^���𖳌��ɂ���
            currentCanvas.SetActive(false); // ���݂̃L�����o�X���\��
        }
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true);  // �؂�ւ���̃L�����o�X��\��
            SetButtonsInteractable(targetCanvas, true); // �؂�ւ���̃L�����o�X���̃{�^����L���ɂ���
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
