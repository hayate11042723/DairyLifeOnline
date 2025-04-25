using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusOnEnemy : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera; // ���z�J����

    [SerializeField]
    private Transform _playerTransform; // �v���C���[��Transform

    [SerializeField]
    private InputActionReference _clickAction; // ���N���b�N�A�N�V����

    [SerializeField]
    private InputActionReference _rightClickDragAction; // �E�h���b�O��InputAction

    [SerializeField]
    private CinemachineInputProvider _inputProvider; // Cinemachine��InputProvider

    private Transform _currentLookTarget; // ���݂̃^�[�Q�b�g
    private bool _isFocusedOnEnemy = false; // �J������Enemy�𒍎����Ă��邩
    private bool _isDragging = false; // �E�h���b�O�����ǂ���

    private void Start()
    {
        // ���z�J�������ݒ肳��Ă��Ȃ��ꍇ�A�V�[��������T��
        if (_cinemachineVirtualCamera == null)
        {
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        // InputProvider���ݒ肳��Ă��Ȃ��ꍇ�A�R���|�[�l���g����擾
        if (_inputProvider == null)
        {
            _inputProvider = GetComponent<CinemachineInputProvider>();
        }
    }

    private void OnEnable()
    {
        // �V�[���؂�ւ���ɉ��z�J�������Đݒ�
        if (_cinemachineVirtualCamera == null)
        {
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        // �C�x���g���X�i�[���ēo�^
        _clickAction.action.Enable();
        _clickAction.action.performed += OnLeftClickPerformed;

        _rightClickDragAction.action.Enable();
        _rightClickDragAction.action.started += OnRightClickDragStart;
        _rightClickDragAction.action.canceled += OnRightClickDragEnd;

        // InputProvider��L����
        if (_inputProvider != null)
        {
            _inputProvider.enabled = true;
        }
    }

    public void OnDisable()
    {
        // �A�N�V�����𖳌���
        _clickAction.action.performed -= OnLeftClickPerformed;
        _clickAction.action.Disable();

        _rightClickDragAction.action.started -= OnRightClickDragStart;
        _rightClickDragAction.action.canceled -= OnRightClickDragEnd;
        _rightClickDragAction.action.Disable();

        // InputProvider�𖳌���
        if (_inputProvider != null)
        {
            _inputProvider.enabled = false;
        }
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        // ���N���b�N���s��ꂽ�ꍇ�̏���
        if (context.control.name == "leftButton")
        {
            if (_isFocusedOnEnemy)
            {
                // ���ɓG�Ƀt�H�[�J�X���Ă���ꍇ�A�J���������Z�b�g
                ResetCamera();
            }
            else
            {
                // �N���b�N�����G�Ƀt�H�[�J�X�����݂�
                TryFocusOnClickedEnemy();
            }
        }
    }

    private void OnRightClickDragStart(InputAction.CallbackContext context)
    {
        // �E�N���b�N�h���b�O���J�n���ꂽ�ꍇ�̏���
        if (context.control.name == "rightButton")
        {
            _isDragging = true;
            if (_inputProvider != null)
            {
                _inputProvider.enabled = true; // CinemachineInputProvider��L����
            }
        }
    }

    private void OnRightClickDragEnd(InputAction.CallbackContext context)
    {
        // �E�N���b�N�h���b�O���I�������ꍇ�̏���
        if (context.control.name == "rightButton")
        {
            _isDragging = false;
            if (!_isFocusedOnEnemy && _inputProvider != null)
            {
                _inputProvider.enabled = true; // �^�[�Q�b�g���Ȃ��ꍇ�͈��������L��
            }
        }
    }

    private void TryFocusOnClickedEnemy()
    {
        // �E�h���b�O���̓N���b�N����
        if (_isDragging) return;

        // Raycast�ŃN���b�N�����I�u�W�F�N�g�����o
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Enemy�^�O���t���Ă���I�u�W�F�N�g�𔻒�
            if (hit.transform.CompareTag("Enemy"))
            {
                // �G�Ƀt�H�[�J�X
                FocusCameraOnEnemy(hit.transform);
            }
        }
    }

    private void FocusCameraOnEnemy(Transform enemyTransform)
    {
        if (_cinemachineVirtualCamera != null && enemyTransform != null)
        {
            // ���݂�CinemachineComponentBase���擾
            var composer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

            // Aim���[�h��ύX����� (POV���폜����DoNothing�ɕύX)
            if (composer != null)
            {
                _cinemachineVirtualCamera.DestroyCinemachineComponent<CinemachinePOV>();
            }

            // �V����Aim���w�� (�Ⴆ�΁AGroupComposer��ݒ�)
            _cinemachineVirtualCamera.AddCinemachineComponent<CinemachineGroupComposer>();

            // LookAt���N���b�N����Enemy�ɐݒ�
            _cinemachineVirtualCamera.LookAt = enemyTransform;
            _currentLookTarget = enemyTransform;

            // CinemachineInputProvider�𖳌����iEnemy�Ƀt�H�[�J�X���͑�����֎~�j
            if (_inputProvider != null)
            {
                _inputProvider.enabled = false;
            }

            _isFocusedOnEnemy = true;
        }
    }

    private void ResetCamera()
    {
        // ���݂�CinemachineComponentBase���擾
        var composer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineGroupComposer>();

        // Aim���[�h��ύX����� (POV���폜����DoNothing�ɕύX)
        if (composer != null)
        {
            _cinemachineVirtualCamera.DestroyCinemachineComponent<CinemachineGroupComposer>();
        }

        // �V����Aim���w�� (�Ⴆ�΁AGroupComposer��ݒ�)
        _cinemachineVirtualCamera.AddCinemachineComponent<CinemachinePOV>();

        // �J�������v���C���[�Ƀ��Z�b�g
        if (_cinemachineVirtualCamera != null)
        {
            _cinemachineVirtualCamera.LookAt = _playerTransform;
        }

        _currentLookTarget = null;
        _isFocusedOnEnemy = false;

        // CinemachineInputProvider��L����
        if (_inputProvider != null)
        {
            _inputProvider.enabled = true;
        }
    }
}
