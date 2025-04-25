using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAI : MonoBehaviour
{
    // �v���C���[���m�G���A�̎l�_�̍��W
    [SerializeField] private Vector3 areaPoint1;
    [SerializeField] private Vector3 areaPoint2;
    [SerializeField] private Vector3 areaPoint3;
    [SerializeField] private Vector3 areaPoint4;
    // basic�U���R���C�_�[
    [SerializeField] private Collider basicCollider;
    // tail�U���R���C�_�[
    [SerializeField] private Collider tailCollider;
    // �v���C���[�̈ړ����x
    [SerializeField] private float moveSpeed;
    // �v���C���[�ɋ߂Â���~����
    [SerializeField] private float StopDistance;
    // �v���C���[���G���A�O�ɏo�Ă���walk�A�j���[�V�������Đ�����܂ł̎���
    [SerializeField] private float timeToStartWalking;
    // �U���Ԋu�i�b�j
    [SerializeField] private float attackInterval;
    // �����������ԁi�b�j
    [SerializeField] private float reorientTime;
    // �U����Ɍ������Œ肷�鎞�ԁi�b�j
    [SerializeField] private float postAttackIdleTime;
    // �^�[�Q�b�g�̃^�O
    [SerializeField] private string TargetTag = "Player";
    // �������郌�C���[
    [SerializeField] private string ignoreLayerName = "markar";
    // �X���C�_�[Canvas
    [SerializeField] private Canvas SliderCanvas;
    [SerializeField] private Animator EnemyAnimator;
    // �G���A���̃v���C���[Transform
    private Transform playerTransform;
    private bool isPlayerInArea = false;
    private bool nearPlayer = false;
    private float timeSincePlayerLeft = 0f;
    private bool isReturningToInitialPosition = false;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isIdle = false;
    private Quaternion fixedRotation;

    // �G�̏����ʒu��ۑ�
    private Vector3 enemyInitialPosition;
    private Quaternion enemyInitialRotation;

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[��Transform���擾
        GameObject player = GameObject.FindGameObjectWithTag(TargetTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // �G�̏����ʒu�Ɖ�]��ۑ�
        enemyInitialPosition = transform.position;
        enemyInitialRotation = transform.rotation;

        // �X���C�_�[���\���ɂ���
        SliderCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[���G���A���ɂ��邩�ǂ����𔻒�
        isPlayerInArea = IsPlayerInArea();

        // �v���C���[���G���A���ɂ���ꍇ
        if (isPlayerInArea && playerTransform != null)
        {
            // �v���C���[���G���A���ɂ���Ƃ��ɃX���C�_�[��\��
            SliderCanvas.gameObject.SetActive(true);

            // �v���C���[�Ƃ̋������v�Z
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // �U�����܂��̓A�C�h����ԂłȂ��ꍇ�̂݃v���C���[�̕���������
            if (!isAttacking && !isIdle)
            {
                transform.LookAt(playerTransform);
            }

            // �U�����܂��̓A�C�h����ԂłȂ��ꍇ�݈̂ړ�
            if (!isAttacking && !isIdle)
            {
                // �v���C���[����~������艓���ꍇ
                if (distanceToPlayer > StopDistance)
                {
                    // �v���C���[�Ɍ������Đi��
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    nearPlayer = false;
                    EnemyAnimator.SetBool("getUp", true); // �A�j���[�V�������Đ�
                    EnemyAnimator.SetBool("run", true); // �A�j���[�V�������Đ�
                    EnemyAnimator.SetBool("walk", false); // walk�A�j���[�V�������~
                }
                else
                {
                    // �v���C���[����~�������ɂ���ꍇ
                    if (!nearPlayer)
                    {
                        nearPlayer = true;
                        EnemyAnimator.SetBool("run", false); // �A�j���[�V�������~
                    }
                }
            }

            // �v���C���[���G���A���ɂ���Ԃ̓^�C�}�[�����Z�b�g
            timeSincePlayerLeft = 0f;
            isReturningToInitialPosition = false;

            // �U�����s��
            if (canAttack)
            {
                PerformAttack();
                StartCoroutine(AttackCooldown());
            }
        }
        else
        {
            // �v���C���[���G���A�O�ɂ���ꍇ
            timeSincePlayerLeft += Time.deltaTime;

            // �v���C���[���G���A�O�ɂ�����X���C�_�[���\��
            SliderCanvas.gameObject.SetActive(false);

            // �v���C���[���G���A�O�ɏo�Ă����莞�Ԍo�߂�����walk�A�j���[�V�������Đ�
            if (timeSincePlayerLeft >= timeToStartWalking && !isReturningToInitialPosition)
            {
                // walk�A�j���[�V�������Đ����A���̈ʒu�ɖ߂�n�߂�
                EnemyAnimator.SetBool("walk", true); // walk�A�j���[�V�������Đ�
                isReturningToInitialPosition = true;
            }

            if (isReturningToInitialPosition)
            {
                // ���̈ʒu�ɖ߂�
                float distanceToInitial = Vector3.Distance(transform.position, enemyInitialPosition);
                if (distanceToInitial > 0.1f)
                {
                    // ���̈ʒu�Ɍ������Đi��
                    Vector3 directionToInitial = (enemyInitialPosition - transform.position).normalized;
                    transform.position += directionToInitial * moveSpeed * Time.deltaTime;

                    // �i�s����������
                    transform.rotation = Quaternion.LookRotation(directionToInitial);

                    EnemyAnimator.SetBool("run", false); // run�A�j���[�V�������~
                }
                else
                {
                    // ���̈ʒu�ɓ���������Sleep�A�j���[�V�������Đ�
                    transform.rotation = enemyInitialRotation; // ������]�ɖ߂�
                    EnemyAnimator.SetBool("walk", false); // walk�A�j���[�V�������~
                    EnemyAnimator.SetBool("getUp", false); // getUp�A�j���[�V�������~
                    EnemyAnimator.SetBool("sleep", true); // Sleep�A�j���[�V�������Đ�
                    EnemyAnimator.SetBool("isSleep", true); // isSleep�t���O���Z�b�g
                    isReturningToInitialPosition = false; // ���Z�b�g
                }
            }
        }
    }

    // �v���C���[���G���A�ɓ������Ƃ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag) && other.gameObject.layer != LayerMask.NameToLayer(ignoreLayerName))
        {
            isPlayerInArea = true;
            EnemyAnimator.SetBool("getUp", true); // �A�j���[�V�������Đ�
            EnemyAnimator.SetBool("sleep", false); // Sleep�A�j���[�V�������~
            Debug.Log("Player entered the area");
        }
    }

    // �v���C���[���G���A����o���Ƃ�
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag) && other.gameObject.layer != LayerMask.NameToLayer(ignoreLayerName))
        {
            isPlayerInArea = false;
            nearPlayer = false;
            EnemyAnimator.SetBool("run", false); // �A�j���[�V�������~
            timeSincePlayerLeft = 0f; // �^�C�}�[�����Z�b�g
            isReturningToInitialPosition = false; // �t���O�����Z�b�g
            Debug.Log("Player exited the area");
        }
    }

    // �v���C���[���G���A���ɂ��邩�ǂ����𔻒肷�郁�\�b�h
    private bool IsPlayerInArea()
    {
        if (playerTransform == null) return false;

        Vector3 playerPos = playerTransform.position;

        // �l�_�̍��W�ň͂܂ꂽ�̈���Ƀv���C���[�����邩�ǂ����𔻒�
        bool isInside = IsPointInPolygon(playerPos, new Vector3[] { areaPoint1, areaPoint2, areaPoint3, areaPoint4 });
        return isInside;
    }

    // �_�����p�`���ɂ��邩�ǂ����𔻒肷�郁�\�b�h
    private bool IsPointInPolygon(Vector3 point, Vector3[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        float pointX = point.x, pointZ = point.z;
        float startX, startZ, endX, endZ;
        Vector3 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endZ = endPoint.z;
        while (i < polygonLength)
        {
            startX = endX; startZ = endZ;
            endPoint = polygon[i++];
            endX = endPoint.x; endZ = endPoint.z;
            inside ^= (endZ > pointZ ^ startZ > pointZ) && ((pointX - startX) < (endX - startX) * (pointZ - startZ) / (endZ - startZ));
        }
        return inside;
    }

    // �U�����s�����\�b�h
    private void PerformAttack()
    {
        bool isInBasicCollider = basicCollider.bounds.Contains(playerTransform.position);
        bool isInTailCollider = tailCollider.bounds.Contains(playerTransform.position);

        if (isInBasicCollider && isInTailCollider)
        {
            // �����̃R���C�_�[�ɏd�Ȃ��Ă���ꍇ�̓����_���ōU��
            int attackType = Random.Range(0, 2); // 0 or 1
            if (attackType == 0)
            {
                // basic�U��
                EnemyAnimator.SetBool("basic", true);
                StartCoroutine(ResetAttack("basic"));
            }
            else
            {
                // tail�U��
                EnemyAnimator.SetBool("tail", true);
                StartCoroutine(ResetAttack("tail"));
            }
        }
        else if (isInBasicCollider)
        {
            // basic�U��
            EnemyAnimator.SetBool("basic", true);
            StartCoroutine(ResetAttack("basic"));
        }
        else if (isInTailCollider)
        {
            // tail�U��
            EnemyAnimator.SetBool("tail", true);
            StartCoroutine(ResetAttack("tail"));
        }
    }

    // �U�������Z�b�g���郁�\�b�h
    private IEnumerator ResetAttack(string attackType)
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.0f); // �A�j���[�V�����̒����ɉ����Ē���
        EnemyAnimator.SetBool(attackType, false);
        isAttacking = false;

        // �����ƃ|�W�V�������Œ肵�A�A�C�h����Ԃɂ���
        isIdle = true;
        fixedRotation = transform.rotation; // ���݂̉�]��ۑ�
        EnemyAnimator.SetBool("idle", true); // �A�C�h���A�j���[�V�������Đ�
        EnemyAnimator.SetBool("run", false);
        EnemyAnimator.SetBool("walk", false);
        EnemyAnimator.SetBool("getUp", false);
        EnemyAnimator.SetBool("sleep", false);
        yield return new WaitForSeconds(postAttackIdleTime); // �A�C�h����Ԃőҋ@
        isIdle = false;
        EnemyAnimator.SetBool("idle", false); // �A�C�h���A�j���[�V�������~
    }

    // �U���̃N�[���_�E�����Ǘ����郁�\�b�h
    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }
}