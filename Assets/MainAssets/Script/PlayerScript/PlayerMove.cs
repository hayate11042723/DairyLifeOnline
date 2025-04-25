using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float movementSpeed;
    public float d_movementSpeed;
    private Rigidbody rb;
    private bool moving;
    private bool d_moving;
    private float horizontal;
    private float vertical;

    public Animator PlayerAnimator;
    private PlayerAttack playerAttack; // PlayerAttack�̃C���X�^���X��ǉ�

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>(); // PlayerAttack�̃R���|�[�l���g���擾
    }

    public void Update()
    {
        if (playerAttack != null && playerAttack.isAttacking) // �U�����͓��͂𖳎�
        {
            horizontal = 0;
            vertical = 0;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (moving)
        {
            // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            Vector3 moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

            // �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
            rb.velocity = moveForward * movementSpeed + new Vector3(0, rb.velocity.y, 0);

            // �L�����N�^�[�̌�����i�s������
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (d_moving)
        {
            // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            Vector3 moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

            // �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
            rb.velocity = moveForward * d_movementSpeed + new Vector3(0, rb.velocity.y, 0);

            // �L�����N�^�[�̌�����i�s������
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerAttack != null && playerAttack.isAttacking) // �U�����͓��͂𖳎�
        {
            return;
        }

        Vector2 movementInput = context.ReadValue<Vector2>();
        horizontal = movementInput.x;
        vertical = movementInput.y;

        if (context.performed)
        {
            moving = true;
            PlayerAnimator.SetBool("run", true);
        }
        else if (context.canceled)
        {
            moving = false;
            PlayerAnimator.SetBool("run", false);
        }
    }
}
