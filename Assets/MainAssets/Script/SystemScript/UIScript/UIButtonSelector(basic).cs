using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSelector0 : MonoBehaviour, IPointerEnterHandler
{
    public Button[] buttons; // �{�^���̔z��
    public RawImage cursor; // �J�[�\���I�u�W�F�N�g
    public Vector3 cursorOffset; // �J�[�\���̃I�t�Z�b�g
    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X

    private void Start()
    {
        if (buttons.Length > 0)
        {
            SelectButton(currentIndex);
        }
    }

    private void Update()
    {
        // ������L�[
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.up.wasPressedThisFrame == true)
        {
            MoveSelection(-1);
        }

        // �������L�[
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.down.wasPressedThisFrame == true)
        {
            MoveSelection(1);
        }

        // Enter�L�[�܂���A�{�^��
        if (Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true)
        {
            buttons[currentIndex].onClick.Invoke();
        }

        // �}�E�X���N���b�N
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            buttons[currentIndex].onClick.Invoke();
        }
    }

    private void MoveSelection(int direction)
    {
        // �C���f�b�N�X���X�V
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = buttons.Length - 1;
        if (currentIndex >= buttons.Length) currentIndex = 0;

        SelectButton(currentIndex);
    }

    private void SelectButton(int index)
    {
        // EventSystem�Ń{�^����I��
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

        // �J�[�\����I�𒆂̃{�^���̉��Ɉړ�
        if (cursor != null)
        {
            cursor.rectTransform.position = buttons[index].transform.position + cursorOffset;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (eventData.pointerEnter == buttons[i].gameObject)
            {
                currentIndex = i;
                SelectButton(currentIndex);
                break;
            }
        }
    }
}
