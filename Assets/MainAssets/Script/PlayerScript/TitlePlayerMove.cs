using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayerMove : MonoBehaviour
{
    public float speed = 5.0f; // �v���C���[�̈ړ����x
    public GameObject enemy1; // �G�l�~�[1�̃Q�[���I�u�W�F�N�g
    public GameObject enemy2; // �G�l�~�[2�̃Q�[���I�u�W�F�N�g
    public float enemySpeed1 = 4.0f; // �G�l�~�[1�̈ړ����x
    public float enemySpeed2 = 3.5f; // �G�l�~�[2�̈ړ����x
    public Vector3 respawnPosition = new Vector3(10.0f, 0.0f, 0.0f); // �ďo������ʒu
    public Vector3 despawnPosition = new Vector3(-10.0f, 0.0f, 0.0f); // ������ʒu
    public Vector3 moveDirection = Vector3.left; // �v���C���[�̈ړ��������
    public Vector3 enemyMoveDirection1 = Vector3.left; // �G�l�~�[1�̈ړ��������
    public Vector3 enemyMoveDirection2 = Vector3.left; // �G�l�~�[2�̈ړ��������
    public Quaternion leftRotation = Quaternion.Euler(0, 0, 0); // �������̉�]
    public Quaternion rightRotation = Quaternion.Euler(0, 180, 0); // �E�����̉�]

    private Vector3 initialPlayerPosition; // �v���C���[�̏����ʒu
    private Vector3 initialEnemyPosition1; // �G�l�~�[1�̏����ʒu
    private Vector3 initialEnemyPosition2; // �G�l�~�[2�̏����ʒu
    private Animator playerAnimator; // �v���C���[�̃A�j���[�^�[
    private Animator enemyAnimator1; // �G�l�~�[1�̃A�j���[�^�[
    private Animator enemyAnimator2; // �G�l�~�[2�̃A�j���[�^�[

    // Start is called before the first frame update
    void Start()
    {
        // �G�l�~�[���ݒ肳��Ă��Ȃ��ꍇ�̓G���[���b�Z�[�W��\��
        if (enemy1 == null || enemy2 == null)
        {
            Debug.LogError("Enemy GameObject is not assigned.");
            return;
        }

        // �v���C���[�ƃG�l�~�[�̏����ʒu��ۑ�
        initialPlayerPosition = transform.position;
        initialEnemyPosition1 = enemy1.transform.position;
        initialEnemyPosition2 = enemy2.transform.position;

        // �v���C���[�ƃG�l�~�[�̃A�j���[�^�[���擾
        playerAnimator = GetComponent<Animator>();
        enemyAnimator1 = enemy1.GetComponent<Animator>();
        enemyAnimator2 = enemy2.GetComponent<Animator>();

        // �v���C���[�̃A�j���[�^�[���ݒ肳��Ă��Ȃ��ꍇ�̓G���[���b�Z�[�W��\��
        if (playerAnimator == null)
        {
            Debug.LogError("Player Animator is not assigned.");
        }

        // �G�l�~�[�̃A�j���[�^�[���ݒ肳��Ă��Ȃ��ꍇ�̓G���[���b�Z�[�W��\��
        if (enemyAnimator1 == null)
        {
            Debug.LogError("Enemy1 Animator is not assigned.");
        }
        if (enemyAnimator2 == null)
        {
            Debug.LogError("Enemy2 Animator is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�ƃG�l�~�[���ړ�������
        MovePlayer();
        MoveEnemy1();
        MoveEnemy2();
        // �v���C���[�ƃG�l�~�[�̐܂�Ԃ����`�F�b�N
        CheckBoundaries();
    }

    // �v���C���[���ړ�������
    void MovePlayer()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        // �v���C���[���ړ����Ă���Ƃ��ɃA�j���[�V�������Đ�
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("run", true);
        }
        // �v���C���[���ړ��������������
        if (moveDirection.x > 0)
        {
            transform.rotation = rightRotation;
        }
        else if (moveDirection.x < 0)
        {
            transform.rotation = leftRotation;
        }
    }

    // �G�l�~�[1���ړ�������
    void MoveEnemy1()
    {
        enemy1.transform.Translate(enemyMoveDirection1 * enemySpeed1 * Time.deltaTime, Space.World);
        // �G�l�~�[1���ړ����Ă���Ƃ��ɃA�j���[�V�������Đ�
        if (enemyAnimator1 != null)
        {
            enemyAnimator1.SetBool("run", true);
        }
        // �G�l�~�[1���ړ��������������
        if (enemyMoveDirection1.x > 0)
        {
            enemy1.transform.rotation = rightRotation;
        }
        else if (enemyMoveDirection1.x < 0)
        {
            enemy1.transform.rotation = leftRotation;
        }
    }

    // �G�l�~�[2���ړ�������
    void MoveEnemy2()
    {
        enemy2.transform.Translate(enemyMoveDirection2 * enemySpeed2 * Time.deltaTime, Space.World);
        // �G�l�~�[2���ړ����Ă���Ƃ��ɃA�j���[�V�������Đ�
        if (enemyAnimator2 != null)
        {
            enemyAnimator2.SetBool("run", true);
        }
        // �G�l�~�[2���ړ��������������
        if (enemyMoveDirection2.x > 0)
        {
            enemy2.transform.rotation = rightRotation;
        }
        else if (enemyMoveDirection2.x < 0)
        {
            enemy2.transform.rotation = leftRotation;
        }
    }

    // �v���C���[�ƃG�l�~�[�����E�ɒB�����ꍇ�ɐ܂�Ԃ�
    void CheckBoundaries()
    {
        if (transform.position.x < despawnPosition.x || transform.position.x > respawnPosition.x)
        {
            moveDirection = -moveDirection; // �v���C���[�̈ړ������𔽓]
        }

        if (enemy1.transform.position.x < despawnPosition.x || enemy1.transform.position.x > respawnPosition.x)
        {
            enemyMoveDirection1 = -enemyMoveDirection1; // �G�l�~�[1�̈ړ������𔽓]
        }

        if (enemy2.transform.position.x < despawnPosition.x || enemy2.transform.position.x > respawnPosition.x)
        {
            enemyMoveDirection2 = -enemyMoveDirection2; // �G�l�~�[2�̈ړ������𔽓]
        }
    }
}

