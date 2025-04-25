using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Collider WeaponCollider;
    public SowrdEffect swordEffect; // SowrdEffect�̎Q�Ƃ�ǉ�
    public float detectionRadius = 5f; // �G�����o���锼�a
    public Transform lockedOnEnemy; // ���b�N�I�����ꂽ�G�l�~�[��ێ�����ϐ�

    // �U�������ǂ����̃t���O
    public bool isAttacking = false; // �t���O��public�ɕύX

    // �A�j���[�V�������ς��Ȃ����Ԃ��v������^�C�}�[
    private float animationTimer = 0f;
    private float animationThreshold = 3f; // 4�b��臒l
    private string currentAnimation = "idle";

    private void Update()
    {
        CheckAnimationState();
    }

    // ����̓����蔻����I���ɂ���
    void AttackFlagON()
    {
        WeaponCollider.enabled = true;

        // SowrdEffect��AttackFlagON���\�b�h���Ăяo��
        if (swordEffect != null)
        {
            swordEffect.AttackFlagON();
        }
    }

    // ����̓����蔻����I�t�ɂ���
    void AttackFlagOFF()
    {
        WeaponCollider.enabled = false;

        // SowrdEffect��AttackFlagOFF���\�b�h���Ăяo��
        if (swordEffect != null)
        {
            swordEffect.AttackFlagOFF();
        }

        // �U���A�j���[�V�����I����Ƀt���O�����Z�b�g
        isAttacking = false;

        // �A�j���[�V�����t���O�����Z�b�g
        PlayerAnimator.SetBool("attack_I", false);
        PlayerAnimator.SetBool("attack_K", false);
        PlayerAnimator.SetBool("attack_R", false);

        // �A�j���[�V������idle�ɖ߂�
        SetAnimation("idle");
    }

    // �U���i�ꌂ�j�̓��͏���
    public void OnAttack_I(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // �U�����łȂ���Ύ��s
        {
            isAttacking = true; // �U�����t���O��ݒ�
            FaceTarget(); // �^�[�Q�b�g�Ɍ���
            SetAnimation("attack_I");
        }
    }

    // �U���i��]���j�̓��͏���
    public void OnAttack_K(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // �U�����łȂ���Ύ��s
        {
            isAttacking = true; // �U�����t���O��ݒ�
            FaceTarget(); // �^�[�Q�b�g�Ɍ���
            SetAnimation("attack_K");
        }
    }

    // �U���i�A���j�̓��͏���
    public void OnAttack_R(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // �U�����łȂ���Ύ��s
        {
            isAttacking = true; // �U�����t���O��ݒ�
            FaceTarget(); // �^�[�Q�b�g�Ɍ���
            SetAnimation("attack_R");
        }
    }

    private void SetAnimation(string animationName)
    {
        if (currentAnimation != animationName)
        {
            PlayerAnimator.SetBool(currentAnimation, false);
            PlayerAnimator.SetBool(animationName, true);
            currentAnimation = animationName;
            animationTimer = 0f; // �^�C�}�[�����Z�b�g
        }
    }

    private void CheckAnimationState()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationThreshold)
        {
            // ��u�����A�j���[�V�����t���O��false�ɂ��Č��ɖ߂�
            PlayerAnimator.SetBool(currentAnimation, false);
            StartCoroutine(ResetAnimationFlag());
            animationTimer = 0f; // �^�C�}�[�����Z�b�g
        }
    }

    private IEnumerator ResetAnimationFlag()
    {
        yield return null; // 1�t���[���҂�
        PlayerAnimator.SetBool(currentAnimation, true);
    }

    // �^�[�Q�b�g�Ɍ������\�b�h
    private void FaceTarget()
    {
        if (lockedOnEnemy != null)
        {
            Vector3 direction = (lockedOnEnemy.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation; // ��C�Ɍ���
        }
        else
        {
            FaceNearestEnemy(); // ���b�N�I������Ă��Ȃ��ꍇ�͋߂��̓G�Ɍ���
        }
    }

    // �߂��̓G�Ɍ������\�b�h
    private void FaceNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Collider nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hitCollider;
                }
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation; // ��C�Ɍ���
        }
    }
}
