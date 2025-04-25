using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSelector : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public Button[] buttons; // �{�^���̔z��
    public RawImage cursor; // �J�[�\���I�u�W�F�N�g
    public Vector3 cursorOffset; // �J�[�\���̃I�t�Z�b�g
    public GameObject canvas; // Canvas�I�u�W�F�N�g
    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private Coroutine blinkingCoroutine; // �_�ŃR���[�`��

    private float enterKeyCooldown = 0.5f; // Enter�L�[�̃N�[���_�E�����ԁi�b�j
    private float lastEnterKeyPressTime = -1f; // �Ō��Enter�L�[�������ꂽ����

    private void Start()
    {
        if (buttons.Length > 0)
        {
            SelectButton(currentIndex); // �ŏ��̃{�^����I��
        }
    }

    private void Update()
    {
        // Canvas����\���ɂȂ����ꍇ�A�I�����ŏ��̃{�^���ɖ߂�
        if (canvas != null && !canvas.gameObject.activeInHierarchy)
        {
            currentIndex = 0;
            SelectButton(currentIndex);
            return; // Canvas����\���̏ꍇ�A���̓��͏������s��Ȃ�
        }

        // ������L�[
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.up.wasPressedThisFrame == true)
        {
            MoveSelection(-1); // �I������Ɉړ�
        }

        // �������L�[
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.down.wasPressedThisFrame == true)
        {
            MoveSelection(1); // �I�������Ɉړ�
        }

        // Enter�L�[�܂���A�{�^��
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            buttons[currentIndex].onClick.Invoke(); // ���ݑI�𒆂̃{�^�����N���b�N
            lastEnterKeyPressTime = Time.time; // �Ō��Enter�L�[�������ꂽ���Ԃ��X�V
        }
    }

    private void MoveSelection(int direction)
    {
        // �_�ł����Z�b�g
        StopBlinkingEffect();

        // �C���f�b�N�X���X�V
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = buttons.Length - 1;
        if (currentIndex >= buttons.Length) currentIndex = 0;

        SelectButton(currentIndex); // �V�����{�^����I��
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

        // �_�ł��J�n
        StartBlinkingEffect(buttons[index]);
    }

    private void StartBlinkingEffect(Button button)
    {
        blinkingCoroutine = StartCoroutine(BlinkEffect(button)); // �_�ŃR���[�`�����J�n
    }

    private void StopBlinkingEffect()
    {
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine); // �_�ŃR���[�`�����~
            blinkingCoroutine = null;
        }

        // �S�{�^���̃e�L�X�g�̖��x�����Z�b�g
        foreach (var btn in buttons)
        {
            var text = btn.GetComponentInChildren<Text>();
            if (text != null)
            {
                var color = text.color;
                color.a = 1f; // ���x�����Z�b�g�i���S�ɖ��邢��ԁj
                text.color = color;
            }
        }
    }

    private IEnumerator BlinkEffect(Button button)
    {
        var text = button.GetComponentInChildren<Text>();
        if (text == null) yield break;

        // �_�Ń��[�v
        while (true)
        {
            // ���邭����
            for (float i = 0.3f; i <= 1f; i += 0.05f)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(0.05f);
            }

            // �Â�����
            for (float i = 1f; i >= 0.3f; i -= 0.05f)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void SetTextBrightness(Text text, float brightness)
    {
        var color = text.color;
        color.a = brightness; // �A���t�@�l�Ŗ��x�𒲐�
        text.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (eventData.pointerEnter == buttons[i].gameObject)
            {
                currentIndex = i;
                SelectButton(currentIndex); // �}�E�X���z�o�[�����{�^����I��

                // �J�[�\����I�𒆂̃{�^���̉��Ɉړ�
                if (cursor != null)
                {
                    cursor.rectTransform.position = buttons[i].transform.position + cursorOffset;
                }

                break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (eventData.pointerPress == buttons[i].gameObject)
            {
                buttons[i].onClick.Invoke(); // �}�E�X���N���b�N�����{�^�����N���b�N
                break;
            }
        }
    }
}

