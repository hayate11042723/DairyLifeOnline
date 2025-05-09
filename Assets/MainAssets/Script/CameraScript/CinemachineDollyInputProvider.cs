using Cinemachine;
using UnityEngine;

/// <summary>
/// CinemachineFramingTransposer �̃J�����O������̃R���g���[����t�^����N���X�B
/// ���̃X�N���v�g���g�p���邱�ƂŁA�v���C���[�̓��͂ɉ����ăJ���������𓮓I�ɒ����\�B
/// </summary>
public class CinemachineDollyInputProvider : MonoBehaviour
{
    // Cinemachine �̉��z�J�����B
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera = null;

    // ���[�U�[���͂��󂯎�邽�߂̃v���o�C�_�B
    [SerializeField]
    private CinemachineInputProvider _cinemachineInputProvider = null;

    // �J���������̒������x�B
    [SerializeField]
    private float _sensitivity;

    // �J���������̍ŏ��l�B
    [SerializeField]
    private float _minDistance;

    // �J���������̍ő�l�B
    [SerializeField]
    private float _maxDistance;

    /// <summary>
    /// �J�������x�̃v���p�e�B�B�O������̎擾�Ɛݒ肪�\�B
    /// </summary>
    public float Sensitivity
    {
        get => _sensitivity;
        set => _sensitivity = value;
    }

    /// <summary>
    /// �I�u�W�F�N�g���L�������ꂽ�ۂɌĂяo����郁�\�b�h�B
    /// </summary>
    private void OnEnable()
    {
        _cinemachineInputProvider.ZAxis.action.Enable();
        Debug.Log("Enable");
    }

    /// <summary>
    /// �X�N���v�g�������������ۂɌĂяo����郁�\�b�h�B
    /// CinemachineFramingTransposer �R���|�[�l���g���擾���A�J���������̒������W�b�N��ݒ�B
    /// </summary>
    private void Awake()
    {
        // ���z�J�������� FramingTransposer �R���|�[�l���g���擾�B
        var cinemachineComponent = _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (cinemachineComponent is CinemachineFramingTransposer cinemachineFramingTransposer)
        {
            // Z ���̓��̓A�N�V�����Ƀ��X�i�[��o�^�B
            _cinemachineInputProvider.ZAxis.action.performed += context =>
            {
                // ���݂̃J�����������v�Z���A�ݒ肳�ꂽ�͈͓��ɌŒ�B
                float distance = Mathf.Clamp(
                    cinemachineFramingTransposer.m_CameraDistance + context.ReadValue<float>() * _sensitivity,
                    _minDistance,
                    _maxDistance
                );

                // �v�Z�����������J�����ɓK�p�B
                cinemachineFramingTransposer.m_CameraDistance = distance;
            };
        }
        else
        {
            // FramingTransposer �R���|�[�l���g��������Ȃ��ꍇ�͌x����\���B
            Debug.LogWarning($"{nameof(CinemachineFramingTransposer)} �����݂��܂���B");
        }
    }
}
