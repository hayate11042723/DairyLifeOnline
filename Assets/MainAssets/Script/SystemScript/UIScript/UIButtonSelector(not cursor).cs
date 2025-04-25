using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSelector2 : MonoBehaviour
{
    [SerializeField] float waitForSeconds;
    [SerializeField] float iSeconds;
    [SerializeField] float enterKeyCooldown = 0.5f; // Enter�L�[�̃N�[���_�E�����ԁi�b�j
    [SerializeField] float mouseClickCooldown = 0.5f; // �}�E�X���N���b�N�̃N�[���_�E�����ԁi�b�j

    public Button[] buttons; // �{�^���̔z��
    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private Coroutine blinkingCoroutine; // �_�ŃR���[�`��

    private float lastEnterKeyPressTime = -1f; // �Ō��Enter�L�[�������ꂽ����
    private float lastMouseClickTime = -1f; // �Ō�Ƀ}�E�X���N���b�N�������ꂽ����

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
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            buttons[currentIndex].onClick.Invoke();
            lastEnterKeyPressTime = Time.time; // �Ō��Enter�L�[�������ꂽ���Ԃ��X�V
        }

        // �}�E�X���N���b�N
        if (Mouse.current.leftButton.wasReleasedThisFrame && Time.time - lastMouseClickTime > mouseClickCooldown)
        {
            buttons[currentIndex].onClick.Invoke();
            lastMouseClickTime = Time.time; // �Ō�Ƀ}�E�X���N���b�N�������ꂽ���Ԃ��X�V
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

        SelectButton(currentIndex);
    }

    private void SelectButton(int index)
    {
        // EventSystem�Ń{�^����I��
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

        // �_�ł��J�n
        StartBlinkingEffect(buttons[index]);
    }

    private void StartBlinkingEffect(Button button)
    {
        blinkingCoroutine = StartCoroutine(BlinkEffect(button));
    }

    private void StopBlinkingEffect()
    {
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
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
            for (float i = iSeconds; i <= 1f; i += waitForSeconds)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(waitForSeconds);
            }

            // �Â�����
            for (float i = 1f; i >= iSeconds; i -= waitForSeconds)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(waitForSeconds);
            }
        }
    }

    private void SetTextBrightness(Text text, float brightness)
    {
        var color = text.color;
        color.a = brightness; // �A���t�@�l�Ŗ��x�𒲐�
        text.color = color;
    }
}

