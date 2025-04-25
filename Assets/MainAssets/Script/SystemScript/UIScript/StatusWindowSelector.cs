using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusWindowSelector : MonoBehaviour, IPointerClickHandler
{
    // �{�^���̃��X�g
    public List<Button> buttons;
    // Canvas�I�u�W�F�N�g
    [SerializeField] Canvas canvas;
    // ����
    [SerializeField] float time;
    // ���ݑI�𒆂̃{�^���̃C���f�b�N�X
    private int currentIndex = 0;
    // �{�^�������Œ����ǂ����̃t���O
    private bool isBlinking = false;
    // ���ł̑��x
    private float blinkSpeed = 1f;
    // Enter�L�[�̃N�[���_�E�����ԁi�b�j
    private float enterKeyCooldown = 0.5f;
    // �Ō��Enter�L�[�������ꂽ����
    private float lastEnterKeyPressTime = -1f;

    // Start is called before the first frame update
    void Start()
    {
        // �{�^�������݂���ꍇ�A�ŏ��̃{�^�����n�C���C�g
        if (buttons.Count > 0)
        {
            HighlightButton(currentIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Canvas����\���ɂȂ����ꍇ�A�I�����ŏ��̃{�^���ɖ߂�
        if (canvas != null && !canvas.gameObject.activeInHierarchy)
        {
            currentIndex = 0;
            HighlightButton(currentIndex);
            return; // Canvas����\���̏ꍇ�A���̓��͏������s��Ȃ�
        }

        // �E���L�[�������ꂽ�ꍇ�A���̃{�^����I��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelection(1);
        }
        // �����L�[�������ꂽ�ꍇ�A�O�̃{�^����I��
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelection(-1);
        }

        // Enter�L�[�܂���A�{�^��
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            buttons[currentIndex].onClick.Invoke();
            lastEnterKeyPressTime = Time.time; // �Ō��Enter�L�[�������ꂽ���Ԃ��X�V
        }
    }

    // �I����ύX���郁�\�b�h
    private void ChangeSelection(int direction)
    {
        // ���݂̃{�^���̃n�C���C�g������
        UnhighlightButton(currentIndex);
        // �V�����C���f�b�N�X���v�Z
        currentIndex = (currentIndex + direction + buttons.Count) % buttons.Count;
        // �V�����{�^�����n�C���C�g
        HighlightButton(currentIndex);
    }

    // �{�^�����n�C���C�g���郁�\�b�h
    private void HighlightButton(int index)
    {
        // ���ł��J�n����Ă��Ȃ��ꍇ�A���ł��J�n
        if (!isBlinking)
        {
            StartCoroutine(BlinkButton(buttons[index]));
        }
    }

    // �{�^���̃n�C���C�g���������郁�\�b�h
    private void UnhighlightButton(int index)
    {
        // �S�ẴR���[�`�����~
        StopAllCoroutines();
        isBlinking = false;
        // �{�^���̃e�L�X�g�̓����x�����ɖ߂�
        Text buttonText = buttons[index].GetComponentInChildren<Text>();
        Color color = buttonText.color;
        color.a = 1f;
        buttonText.color = color;
    }

    // �{�^���𖾖ł�����R���[�`��
    private IEnumerator BlinkButton(Button button)
    {
        isBlinking = true;
        Text buttonText = button.GetComponentInChildren<Text>();
        float alpha = 1f;
        bool fadingOut = true;

        while (isBlinking)
        {
            // �����x���������ƕω�������
            if (fadingOut)
            {
                alpha -= Time.deltaTime * blinkSpeed;
                if (alpha <= time)
                {
                    alpha = time;
                    fadingOut = false;
                }
            }
            else
            {
                alpha += Time.deltaTime * blinkSpeed;
                if (alpha >= 1f)
                {
                    alpha = 1f;
                    fadingOut = true;
                }
            }

            Color color = buttonText.color;
            color.a = alpha;
            buttonText.color = color;

            yield return null;
        }
    }

    // �{�^�������肷�郁�\�b�h
    private void SelectButton()
    {
        // ���ݑI�𒆂̃{�^���̃N���b�N�C�x���g���Ăяo��
        buttons[currentIndex].onClick.Invoke();
    }

    // �{�^�����N���b�N���ꂽ�Ƃ��̏���
    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (eventData.pointerPress == buttons[i].gameObject)
            {
                currentIndex = i;
                SelectButton();
                break;
            }
        }
    }
}
