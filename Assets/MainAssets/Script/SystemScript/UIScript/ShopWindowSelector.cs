using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ShopWindowSelector : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] float waitForSeconds; // �_�ł̊Ԋu
    [SerializeField] float iSeconds; // �_�ł̖��邳�̕ω���
    [SerializeField] float enterKeyCooldown = 0.5f; // Enter�L�[�̃N�[���_�E�����ԁi�b�j
    [SerializeField] float mouseClickCooldown = 0.5f; // �}�E�X���N���b�N�̃N�[���_�E�����ԁi�b�j
    [SerializeField] float clickBrightnessDuration = 0.1f; // �N���b�N���̍ʓx�ύX�̎������ԁi�b�j

    public Button[] buttons; // �{�^���̔z��
    public RawImage cursor; // �J�[�\���I�u�W�F�N�g
    public Vector3 cursorOffset; // �J�[�\���̃I�t�Z�b�g
    private int currentIndex = 0; // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private Coroutine blinkingCoroutine; // �_�ŃR���[�`��

    private float lastEnterKeyPressTime = -1f; // �Ō��Enter�L�[�������ꂽ����
    private float lastMouseClickTime = -1f; // �Ō�Ƀ}�E�X���N���b�N�������ꂽ����

    private void Start()
    {
        if (buttons.Length > 0)
        {
            SelectButton(currentIndex); // �ŏ��̃{�^����I��
        }
    }

    private void Update()
    {
        // ������L�[
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.up.wasPressedThisFrame == true)
        {
            MoveSelection(-1, 0);
        }

        // �������L�[
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.down.wasPressedThisFrame == true)
        {
            MoveSelection(1, 0);
        }

        // �������L�[
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.left.wasPressedThisFrame == true)
        {
            MoveSelection(0, -1);
        }

        // �E�����L�[
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.right.wasPressedThisFrame == true)
        {
            MoveSelection(0, 1);
        }

        // Enter�L�[�܂���A�{�^��
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            StartCoroutine(ClickEffect(buttons[currentIndex])); // �N���b�N�G�t�F�N�g���J�n
            buttons[currentIndex].onClick.Invoke(); // �{�^���̃N���b�N�C�x���g�𔭐�������
            lastEnterKeyPressTime = Time.time; // �Ō��Enter�L�[�������ꂽ���Ԃ��X�V
        }

        // �}�E�X���N���b�N
        if (Mouse.current.leftButton.wasReleasedThisFrame && Time.time - lastMouseClickTime > mouseClickCooldown)
        {
            // �}�E�X�N���b�N�ʒu�ɂ���{�^����I��
            SelectButtonUnderMouse();
            lastMouseClickTime = Time.time; // �Ō�Ƀ}�E�X���N���b�N�������ꂽ���Ԃ��X�V
        }
    }

    // �{�^���̑I�����ړ����郁�\�b�h
    private void MoveSelection(int verticalDirection, int horizontalDirection)
    {
        // �_�ł����Z�b�g
        StopBlinkingEffect();

        // �C���f�b�N�X���X�V
        int newIndex = currentIndex + verticalDirection * GetHorizontalCount() + horizontalDirection;

        // �͈̓`�F�b�N
        if (newIndex < 0)
        {
            newIndex = buttons.Length - 1;
        }
        else if (newIndex >= buttons.Length)
        {
            newIndex = 0;
        }

        currentIndex = newIndex;
        SelectButton(currentIndex);
    }

    // �{�^����I�����郁�\�b�h
    private void SelectButton(int index)
    {
        // ���݂̑I�����N���A
        EventSystem.current.SetSelectedGameObject(null);

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

    // �}�E�X�ʒu�ɂ���{�^����I�����郁�\�b�h
    private void SelectButtonUnderMouse()
    {
        // �}�E�X�ʒu�ɂ���{�^�������o
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                // �{�^�������������ꍇ�A���̃{�^����I�����A�N���b�N�C�x���g�𔭐�������
                currentIndex = System.Array.IndexOf(buttons, button);
                SelectButton(currentIndex);
                StartCoroutine(ClickEffect(button));
                button.onClick.Invoke();
                break;
            }
        }
    }

    // �_�ŃG�t�F�N�g���J�n���郁�\�b�h
    private void StartBlinkingEffect(Button button)
    {
        blinkingCoroutine = StartCoroutine(BlinkEffect(button));
    }

    // �_�ŃG�t�F�N�g���~���郁�\�b�h
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

    // �_�ŃG�t�F�N�g�̃R���[�`��
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

    // �e�L�X�g�̖��x��ݒ肷�郁�\�b�h
    private void SetTextBrightness(Text text, float brightness)
    {
        var color = text.color;
        color.a = brightness; // �A���t�@�l�Ŗ��x�𒲐�
        text.color = color;
    }

    // �N���b�N�G�t�F�N�g�̃R���[�`��
    private IEnumerator ClickEffect(Button button)
    {
        var text = button.GetComponentInChildren<Text>();
        if (text == null) yield break;

        // �ʓx����u���Ƃ�
        SetTextBrightness(text, 0.5f);
        yield return new WaitForSeconds(clickBrightnessDuration);
        // �ʓx�����ɖ߂�
        SetTextBrightness(text, 1f);
    }

    // ���������̃{�^�������擾���郁�\�b�h
    private int GetHorizontalCount()
    {
        // �����ł͉���4�Ƃ��Ă��܂����A���ۂ̃{�^���z�u�ɍ��킹�ĕύX���Ă�������
        return 4;
    }

    // �}�E�X���{�^���ɏd�Ȃ����Ƃ��ɌĂяo����郁�\�b�h
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
}
