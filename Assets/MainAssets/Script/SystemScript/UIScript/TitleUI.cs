using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private GameObject titleLogo; // �^�C�g�����S��GameObject
    [SerializeField] private Button showButton;   // �^�C�g�����S��\������{�^��
    [SerializeField] private Button hideButton;   // �^�C�g�����S���\���ɂ���{�^��

    void Start()
    {
        // �{�^���̃N���b�N�C�x���g�Ƀ��\�b�h��o�^
        if (showButton != null)
        {
            showButton.onClick.AddListener(ShowTitleLogo);
        }

        if (hideButton != null)
        {
            hideButton.onClick.AddListener(HideTitleLogo);
        }
    }

    // �^�C�g�����S��\�����郁�\�b�h
    public void ShowTitleLogo()
    {
        if (titleLogo != null)
        {
            titleLogo.SetActive(true);
        }
    }

    // �^�C�g�����S���\���ɂ��郁�\�b�h
    public void HideTitleLogo()
    {
        if (titleLogo != null)
        {
            titleLogo.SetActive(false);
        }
    }
}
