using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float Timer;
    [SerializeField] private float ChangeTime;
    [SerializeField] private float EnemySpeed;

    [SerializeField] private float StopDistance; // �v���C���[�ɋ߂Â���~����

    [SerializeField] private string TargetTag = "Player"; // �^�[�Q�b�g�̃^�O
    private GameObject targetObject; // �^�[�Q�b�g��GameObject

    [SerializeField] private Animator EnemyAnimator;

    // Update is called once per frame
    void Update()
    {
        var speed = Vector3.zero;
        var rot = transform.eulerAngles;

        if (targetObject) // �v���C���[��ǂ�������
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetObject.transform.position);

            transform.LookAt(targetObject.transform); // ��Ƀv���C���[�̕���������
            rot = transform.eulerAngles;

            if (distanceToPlayer > StopDistance) // ��~������艓���ꍇ�݈̂ړ�
            {
                speed.z = EnemySpeed; // �v���C���[�����ɐi��
            }
            else
            {
                speed = Vector3.zero; // ��~
            }
        }
        else // �����_���ړ�
        {
            Timer += Time.deltaTime;
            if (Timer >= ChangeTime)
            {
                float rand = Random.Range(0, 360); // �����_���ȕ����ɉ�]
                rot.y = rand;
                Timer = 0;
            }

            speed.z = EnemySpeed * 0.5f; // �����_���ړ����̑��x�i�����ɒ����j
        }

        rot.x = 0;
        rot.z = 0;
        transform.eulerAngles = rot;

        this.transform.Translate(speed * Time.deltaTime, Space.Self); // �t���[�����Ƃ̑��x�𔽉f
    }

    // �R���C�_�[�Ƀv���C���[���������Ƃ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            targetObject = other.gameObject;
            EnemyAnimator.SetBool("run", true); // �A�j���[�V�������Đ�
        }
    }

    // �R���C�_�[����v���C���[���o���Ƃ�
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            targetObject = null;
            EnemyAnimator.SetBool("run", false); // �A�j���[�V�������~
        }
    }
}

